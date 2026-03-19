import { Component, DestroyRef, inject } from '@angular/core';
import { DataFilters } from '../../../../core/services/dmg/models/data/data-filters';
import { OrderStockService } from '../../services/order-stock.service';
import { OrderStockStyleItemComponent } from "./order-stock-style-item/order-stock-style-item.component";
import { OrderStockStyleHeadComponent } from "./order-stock-style-head/order-stock-style-head.component";
import { interval, startWith, switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  standalone: true,
  selector: 'app-order-stock-style',
  templateUrl: './order-stock-style.component.html',
  styleUrl: './order-stock-style.component.css',
  imports: [OrderStockStyleItemComponent, OrderStockStyleHeadComponent],
})
export class OrderStockStyleComponent {
  private service = inject(OrderStockService);
  private destroyRef = inject(DestroyRef);

  title = '成品倉庫存狀況';
  data: any[] = [];
  readonly refreshMs = 600_000;

  ngOnInit() {
    interval(this.refreshMs)
      .pipe(
        startWith(0),
        switchMap(() => this.getDate()),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: (res: any) => {
          this.data = Array.isArray(res) ? res : res?.data?.rows || [];
        },
        error: err => console.error('数据请求失败:', err),
      });
  }

  getDate() {
    const filterItems = [
      { field: 'LocaleId', operator: 'eq' as const, value: 10 },
    ];

    const filters: DataFilters = {
      filters: filterItems,
      logic: 'and' as const,
    };

    return this.service.getOrdersStock({
      filter: filters,
      skip: 0,
    });
  }
}
