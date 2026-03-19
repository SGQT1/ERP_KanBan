import { Routes } from '@angular/router';
import { OrderStockStyleComponent } from './features/order-stock/compoents/order-stock-style/order-stock-style.component';

export const routes: Routes = [
  { path: '', redirectTo: 'order-stock-style', pathMatch: 'full' },
  { path: 'order-stock-style', component: OrderStockStyleComponent }
];
