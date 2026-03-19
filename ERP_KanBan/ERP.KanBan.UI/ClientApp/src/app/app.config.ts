import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { HttpClient, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { firstValueFrom, tap } from 'rxjs';

export const GlobalConfig = {
  apiBaseUrl: '', // 初始为空，由 config.json 覆盖
  language: 'zh-TW',
  theme: 'dark',

  getURL(path: string): string {
    const base = this.apiBaseUrl.replace(/\/+$/, '');
    const p = path.startsWith('/') ? path : `/${path}`;
    return `${base}${p}`;
  }
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideAppInitializer(() => {
      const http = inject(HttpClient);
      return firstValueFrom(
        http.get<any>('/assets/config.json').pipe(
          tap((cfg: any) => {
            // 完全保留原始初始化逻辑，只从 config.json 读取配置
            GlobalConfig.apiBaseUrl = (cfg.apiBaseUrl || '').replace(/\/+$/, '');
            GlobalConfig.language = cfg.language || 'zh-TW';
            GlobalConfig.theme = cfg.theme || 'dark';
            console.log('[Config Loaded]', GlobalConfig);
          })
        )
      ).catch(err => {
        console.warn('config.json 加载失败，使用默认配置', err);
        // 兜底：如果 config.json 不存在，手动设置 apiBaseUrl
        GlobalConfig.apiBaseUrl = 'https://localhost:7040';
        return Promise.resolve();
      });
    })
  ]
};