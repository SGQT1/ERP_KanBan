import * as _ from 'lodash';
import { DataSortItem } from './data-sort-item';
import { DataFilters } from './data-filters';
import { DataFilterItem } from './data-filter-item';
import { DataSourceOptions } from './data-source-options';
import { DataSourceMiddleware } from './data-source-middleware';
import { DataFieldItem } from './data-field-item';

export abstract class DataSource<T> {
  protected middleware?: DataSourceMiddleware<T>;

  get middlewareDataSource(): any {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    return this.middleware.dataSource;
  }

  filter(): DataFilterItem | DataFilters | undefined;
  filter(filter: DataFilterItem): void;
  // tslint:disable-next-line:unified-signatures
  filter(filter: DataFilterItem[]): void;
  // tslint:disable-next-line:unified-signatures
  filter(filter: DataFilters): void;
  filter(filter?: DataFilterItem | DataFilterItem[] | DataFilters): DataFilterItem | DataFilterItem[] | DataFilters | undefined | void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (filter != null) {
      if (_.isArray(filter)) {
        this.middleware.filter = {
          filters: filter,
          logic: 'and'
        } as DataFilters;
      } else {
        this.middleware.filter = filter;
      }
    } else {
      return this.middleware.filter;
    }
  }

  sort(): DataSortItem[] | undefined;
  sort(sort: DataSortItem): void;
  // tslint:disable-next-line:unified-signatures
  sort(sort: DataSortItem[]): void;
  sort(sort?: DataSortItem | DataSortItem[]): DataSortItem | DataSortItem[] | (DataSortItem | DataSortItem[])[] | undefined | void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (sort != null) {
      if (_.isArray(sort)) {
        this.middleware.sort = sort;
      } else {
        this.middleware.sort = [sort];
      }
    } else {
      return this.middleware.sort;
    }
  }

  page(): number | undefined;
  page(page: number): void;
  page(page?: number): number | undefined | void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (page != null) {
      this.middleware.page = page;
    } else {
      return this.middleware.page;
    }
  }

  pageSize(): number | undefined;
  pageSize(pageSize: number): void;
  pageSize(pageSize?: number): number | undefined | void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (pageSize != null) {
      this.middleware.pageSize = pageSize;
    } else {
      return this.middleware.pageSize;
    }
  }

  select(): DataFieldItem[] | undefined;
  select(select: DataFieldItem): void;
  // tslint:disable-next-line:unified-signatures
  select(select: DataFieldItem[]): void;
  // tslint:disable-next-line:max-line-length
  select(select?: DataFieldItem | DataFieldItem[]): DataFieldItem | DataFieldItem[] | (DataFieldItem | DataFieldItem[])[] | undefined | void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (select != null) {
      if (_.isArray(select)) {
        this.middleware.select = select;
      } else {
        this.middleware.select = [select];
      }
    } else {
      return this.middleware.select;
    }
  }

  groupBy(): DataFieldItem[] | undefined;
  groupBy(groupBy: DataFieldItem): void;
  // tslint:disable-next-line:unified-signatures
  groupBy(groupBy: DataFieldItem[]): void;
  // tslint:disable-next-line:max-line-length
  groupBy(groupBy?: DataFieldItem | DataFieldItem[]): DataFieldItem | DataFieldItem[] | (DataFieldItem | DataFieldItem[])[] | undefined | void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (groupBy != null) {
      if (_.isArray(groupBy)) {
        this.middleware.groupBy = groupBy;
      } else {
        this.middleware.groupBy = [groupBy];
      }
    } else {
      return this.middleware.groupBy;
    }
  }

  total(): number | undefined {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    return this.middleware.total;
  }

  data(): T[] | undefined;
  data(data: T[]): void;
  data(data?: T[]): T[] | undefined | void{
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (data != null) {
      this.middleware.data = data;
    } else {
      return this.middleware.data;
    }
  }

  reload(): void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    this.middleware.reload();
  }

  autoBind(): boolean | undefined;
  autoBind(value: boolean): void;
  autoBind(value?: boolean): boolean | undefined | void{
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (value == null) {
      return this.middleware.autoBind;
    } else {
      this.middleware.autoBind = value;
    }
  }

  hasCount(): boolean | undefined;
  hasCount(value: boolean): void;
  hasCount(value?: boolean): boolean | undefined | void {
    if (this.middleware == null) {
      throw new Error('Middleware Not Found.');
    }
    if (value == null) {
      return this.middleware.hasCount;
    } else {
      this.middleware.hasCount = value;
    }
  }
}
