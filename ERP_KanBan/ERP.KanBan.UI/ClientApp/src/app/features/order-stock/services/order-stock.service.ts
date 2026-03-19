import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GlobalConfig } from '../../../app.config';

@Injectable({
  providedIn: 'root',
})
export class OrderStockService {
  private http = inject(HttpClient);

  getOrdersStock(options: any) {
    const url = GlobalConfig.getURL('/kanban/OrdersStock/OrdersStockStyle');
    const encoded = encodeURIComponent(JSON.stringify(options));
    return this.http.get(`${url}?${encoded}`);
  }
}

