import { CommonModule } from '@angular/common';
import { Component, Input, SimpleChanges } from '@angular/core';
import { NGX_ECHARTS_CONFIG, NgxEchartsModule } from 'ngx-echarts';
import * as echarts from 'echarts';
import { EChartsCoreOption } from 'echarts/core';

@Component({
  selector: 'partial-order-stock-style-item',
  imports: [
    CommonModule,
    NgxEchartsModule,
  ],
  providers: [
    {
      provide: NGX_ECHARTS_CONFIG,
      useValue: { echarts },
    },
  ],
  templateUrl: './order-stock-style-item.component.html',
  styleUrl: './order-stock-style-item.component.css',
})
export class OrderStockStyleItemComponent {
  orderStockOption!: EChartsCoreOption;
  @Input() value: any[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['value'] && this.value.length > 0) {
      this.buildChart();
    }
  }
  
  private buildChart() {
    const orderCountRate: any[] = [];
    const orderQtyRate: any[] = [];
    const stockQtyRate: any[] = [];
    let orderOther = 100;
    let qtyOther = 100;
    let stockOther = 100;

    // 固定图例和型号顺序（和你最初一致）
    const fixedLegendNames = [
      'MR530V1(D)(PU)-SG',
      'MR530V1(D)(20S2-1)-EMA',
      'GR740V1(M)(25Q1-1)-BM',
      'U740V1(D)(C2C專案-41)-96I',
      'MR530V1(D)(23S2-4)-CK',
      'Others'
    ];

    // 初始化所有型号为0（和你最初一致）
    fixedLegendNames.forEach(model => {
      if (model !== 'Others') {
        orderCountRate.push({ value: 0, name: model });
        orderQtyRate.push({ value: 0, name: model });
        stockQtyRate.push({ value: 0, name: model });
      }
    });

    this.value.forEach((i: any) => {
      const styleNo = i.modelName;
      const orderCount = Number(i.orderRatio || 0);
      const orderQty = Number(i.pairRatio || 0);
      const stockQty = Number(i.stockRatio || 0);

      const orderIdx = fixedLegendNames.indexOf(styleNo);
      const qtyIdx = fixedLegendNames.indexOf(styleNo);
      const stockIdx = fixedLegendNames.indexOf(styleNo);

      if (orderIdx !== -1 && orderCount > 0) {
        orderCountRate[orderIdx].value = orderCount;
        orderOther -= orderCount;
      }
      if (qtyIdx !== -1 && orderQty > 0) {
        orderQtyRate[qtyIdx].value = orderQty;
        qtyOther -= orderQty;
      }
      if (stockIdx !== -1 && stockQty > 0) {
        stockQtyRate[stockIdx].value = stockQty;
        stockOther -= stockQty;
      }
    });

    if (orderOther > 0) orderCountRate.push({ value: orderOther, name: 'Others' });
    if (qtyOther > 0) orderQtyRate.push({ value: qtyOther, name: 'Others' });
    if (stockOther > 0) stockQtyRate.push({ value: stockOther, name: 'Others' });

    this.orderStockOption = {
      darkMode: true,
      legend: {
        top: 'bottom',
        textStyle: { color: '#d6f1ff', fontSize: 12 },
        data: fixedLegendNames
      },
      // 标题位置和你最初一致
      title: [
        { subtext: '訂單比例', left: '16.67%', top: '75%', textAlign: 'center' },
        { subtext: '雙數比例', left: '50%', top: '75%', textAlign: 'center' },
        { subtext: '庫存比例', left: '83.33%', top: '75%', textAlign: 'center' }
      ],
      color: ['#00eaff', '#0095ff', '#ffd200', '#00f6a2', '#ff7fd4', '#667380'],
      series: [
        {
          type: 'pie', radius: '45%', center: ['50%', '50%'], data: orderCountRate,
          label: { 
            position: 'outer', alignTo: 'edge', edgeDistance: 15,
            formatter: '{d}%',
            color: '#d6f1ff',
            fontSize: 11
          },
          left: 0, right: '66.6667%', top: 0, bottom: 0
        },
        {
          type: 'pie', radius: '45%', center: ['50%', '50%'], data: orderQtyRate,
          label: { 
            position: 'outer', alignTo: 'edge', edgeDistance: 15,
            formatter: '{d}%',
            color: '#d6f1ff',
            fontSize: 11
          },
          left: '33.3333%', right: '33.3333%', top: 0, bottom: 0
        },
        {
          type: 'pie', radius: '45%', center: ['50%', '50%'], data: stockQtyRate,
          label: { 
            position: 'outer', alignTo: 'edge', edgeDistance: 15,
            formatter: '{d}%',
            color: '#d6f1ff',
            fontSize: 11
          },
          left: '66.6667%', right: 0, top: 0, bottom: 0
        }
      ]
    };
  }
}