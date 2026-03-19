import { DecimalPipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'partial-order-stock-style-head',
  imports: [DecimalPipe, TableModule],
  templateUrl: './order-stock-style-head.component.html',
  styleUrl: './order-stock-style-head.component.css',
})
export class OrderStockStyleHeadComponent {
  _data: any[] = [];
  
  @Input() set value(value: any) {
    this._data = Array.isArray(value) ? value.slice(0, 5) : [];
  }
}