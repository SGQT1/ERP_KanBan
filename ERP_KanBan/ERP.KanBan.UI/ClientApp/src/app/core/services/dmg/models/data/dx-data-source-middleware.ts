import { DataSourceMiddleware } from './data-source-middleware';
import { DataFilterItem } from './data-filter-item';
import { DataFilters } from './data-filters';
import { DataSortItem } from './data-sort-item';
import { DataLoadOptions } from './data-load-options';
import { DataSourceOptions } from './data-source-options';
import { DataSourceTransport } from './data-source-transport';
import * as _ from 'lodash';

import DevExpress from 'devextreme/bundles/dx.all';
import CustomStore from 'devextreme/data/custom_store';
import ArrayStore from 'devextreme/data/array_store';
import DataSource from 'devextreme/data/data_source';
import { DataFieldItem } from './data-field-item';
import { QueryResult } from './query-result';

export class DxDataSourceMiddleware<T> implements DataSourceMiddleware<T> {
  private _dataSource: DataSource = new DataSource({});
  private count?: number;
  private isLocal = true;
  private _select: DataFieldItem[] = [];
  private _groupBy: DataFieldItem[] = [];
  private key?: string | string[];
  private _filter?: DataFilterItem | DataFilters;
  private _autoBind = true;
  private _hasCount: boolean = false;

  readonly dataSource: DataSource;

  constructor(options: DataSourceOptions<T>) {
    if (options.key != null) {
      this.key = options.key;
    }
    this.init(options);
    this.dataSource = this._dataSource;
  }

  init(options: DataSourceOptions<T>): void {
    const dxOptions = {} as DevExpress.data.DataSourceOptions;

    if (options.data != null && options.transport == null) {
      dxOptions.store = this.buildArrayStore(options.data);
    } else if (options.data == null && options.transport != null) {
      this.isLocal = false;
      dxOptions.store = this.buildCustomStore(options.transport);
      dxOptions.requireTotalCount = true;
    } else {
      return;
    }

    if (options.pageSize != null) {
      dxOptions.paginate = true;
      dxOptions.pageSize = options.pageSize;
    }

    if (options.sort != null) {
      dxOptions.sort = this.buildDxSort(options.sort) as any;
    }

    if (options.filter != null) {
      this._filter = options.filter;
      dxOptions.filter = this.buildDxFilter(options.filter);
    }

    if (options.select != null) {
      this.select = options.select;
    }

    if (options.groupBy != null) {
      this.groupBy = options.groupBy;
    }

    if (options.autoBind != null) {
      this.autoBind = options.autoBind;
    }

    if (options.hasCount != null) {
      this.hasCount = options.hasCount;
    }

    this._dataSource = new DataSource(dxOptions);

    if (options.page != null) {
      this._dataSource.pageIndex(options.page - 1);
    }
  }

  private buildCustomStore(transport: DataSourceTransport<T>): CustomStore {
    const options = {
      key: this.key
    } as DevExpress.data.CustomStoreOptions;

    if (transport.read != null) {
      const load = transport.read;
      // tslint:disable-next-line:no-shadowed-variable
      options.load = async (options) => {
        if (!this.autoBind) {
          this.count = 0;
          return [];
        }
        const result = await load(this.buildLoadOptions(options)) as any;
        if (_.isArray(result)) {
          this.count = result.length;
          return result;
        }
        this.count = result.Total;
        return result.Data;
      };
      // tslint:disable-next-line:no-shadowed-variable
      options.totalCount = (options) => {
        return new Promise<number>((resolve) => {
          const action = () => resolve(this.count as any);
          if (!this._dataSource.isLoaded() && !this._dataSource.isLoading()) {
            this._dataSource.load().then(() => {
              action();
            });
          } else {
            action();
          }
        });
      };
      options.byKey = (key: any) => {
        if (this.key == null) {
          throw new Error('Key is not setting.');
        }
        // tslint:disable-next-line:no-shadowed-variable
        const options = {} as DataLoadOptions;
        const filter = {
          filters: [],
          logic: 'and'
        } as DataFilters;

        if (this._filter != null) {
          filter.filters.push(this._filter);
        }

        filter.filters.push({
          field: this.key,
          operator: 'eq',
          value: key
        } as DataFilterItem);

        options.filter = filter;

        return load(options).then((e: any) => {
          if (_.isArray(e)) {
            if (e.length === 0) {
              return undefined;
            }
            return e[0];
          } else {
            if (e.Data.length === 0) {
              return undefined;
            }
            return e.Data[0];
          }
        });
      };
    }
    if (transport.create != null) {
      const create = transport.create;
      options.insert = (item) => create(item);
    }
    if (transport.update != null) {
      const update = transport.update;
      options.update = (key, value) => {
        const item = this.getByKey(key);
        for (let key of Object.keys(value)) {
          item[key] = value[key];
        }
        return update(item);
      };
    }
    if (transport.remove != null) {
      const remove = transport.remove;
      options.remove = (key) => remove(this.getByKey(key));
    }

    return new CustomStore(options);
  }

  private buildArrayStore(data: T[]): ArrayStore {
    return new ArrayStore({
      key: this.key,
      data: data
    });
  }

  private buildDxSort(sort: DataSortItem[]) {
    const result = [];
    // tslint:disable-next-line:prefer-const
    for (let item of sort) {
      result.push(
        item.dir === 'asc' ?
          item.field :
          { getter: item.field, desc: true });
    }
    return result;
  }

  private buildSort(sort: any): DataSortItem[] {
    let result = [] as DataSortItem[];
    for (let item of sort) {
      if (_.isString(item)) {
        result.push({ field: item, dir: 'asc' });
      } else {
        let field = item.getter != null ? item.getter : item.selector;
        result.push({ field: field, dir: item.desc ? 'desc' : 'asc' });
      }
    }
    return result;
  }

  private buildDxFilter(filter: DataFilterItem | DataFilters): any {
    if ((filter as DataFilterItem).field != null) {
      return this.buildDxFilterItem(filter as DataFilterItem);
    } else {
      return this.buildDxFilters(filter as DataFilters);
    }
  }

  private buildDxFilterItem(filter: DataFilterItem): any {
    return [filter.field, this.buildDxFilterOperator(filter.operator as any), filter.value];
  }

  private buildDxFilters(filter: DataFilters): any {
    let result = [];
    for (let item of filter.filters) {
      if (result.length > 0) {
        result.push(filter.logic);
      }
      result.push(this.buildDxFilter(item));
    }
    return result;
  }

  private buildFilter(filter: any[]): DataFilterItem | DataFilters {
    if (_.isString(filter[0])) {
      return this.buildFilterItem(filter);
    } else {
      return this.buildFilters(filter);
    }
  }

  private buildFilterItem(filter: any): DataFilterItem {
    return { field: filter[0], operator: this.buildFilterOperator(filter[1]), value: filter[2] };
  }

  private buildFilters(filter: any[]): DataFilters {
    let result = {
      filters: [],
      logic: filter[1] != null ? filter[1] : 'and'
    } as DataFilters;
    for (let i = 0; i < filter.length; i++) {
      if ((i + 1) % 2 == 0) {
        continue;
      }
      result.filters.push(this.buildFilter(filter[i]));
    }
    return result;
  }

  private buildSearch(searchExpr: string[] | string, searchOperation: '=' | '<>' | '<' | '<=' | '>' | '>=' |
    'startswith' | 'endswith' | 'contains' | 'notcontains', searchValue: any): DataFilterItem | DataFilters {
    let buildFilterItem = (field: any) => {
      return { field: field, operator: this.buildFilterOperator(searchOperation), value: searchValue } as DataFilterItem;
    };
    if (_.isArray(searchExpr)) {
      if (searchExpr.length > 1) {
        let filters = { filters: [], logic: 'and' } as DataFilters;
        for (let expr of searchExpr) {
          filters.filters.push(buildFilterItem(expr));
        }
        return filters;
      } else {
        return buildFilterItem(searchExpr[0]);
      }
    } else {
      return buildFilterItem(searchExpr);
    }
  }

  private buildDxFilterOperator(operator: 'eq' | 'neq' | 'lt' | 'lte' | 'gt' | 'gte' |
    'startswith' | 'endswith' | 'contains' | 'doesnotcontain') {
    switch (operator.toLowerCase()) {
      case 'eq':
        return '=';
      case 'neq':
        return '<>';
      case 'lt':
        return '<';
      case 'lte':
        return '<=';
      case 'gt':
        return '>';
      case 'gte':
        return '>=';
      case 'startswith':
        return 'startswith';
      case 'endswith':
        return 'endswith';
      case 'contains':
        return 'contains';
      case 'doesnotcontain':
        return 'notcontains';
      default:
        return;
    }

  }

  private buildFilterOperator(operator: '=' | '<>' | '<' | '<=' | '>' | '>=' |
    'startswith' | 'endswith' | 'contains' | 'notcontains') {
    switch (operator.toLowerCase()) {
      case '=':
        return 'eq';
      case '<>':
        return 'neq';
      case '<':
        return 'lt';
      case '<=':
        return 'lte';
      case '>':
        return 'gt';
      case '>=':
        return 'gte';
      case 'startswith':
        return 'startswith';
      case 'endswith':
        return 'endswith';
      case 'contains':
        return 'contains';
      case 'notcontains':
        return 'doesnotcontain';
      default:
        throw new Error('Error by operator');
    }
  }

  private buildLoadOptions(options: DevExpress.data.LoadOptions): DataLoadOptions {
    let result = {} as DataLoadOptions;
    result.hasCount = this.hasCount;
    result.select = this.select;
    result.groupBy = this.groupBy;
    if (options.take != null) {
      result.take = options.take;
    }
    if (options.skip != null) {
      result.skip = options.skip;
    }
    if (options.sort != null) {
      result.sort = this.buildSort(options.sort);
    }
    let filters = [] as (DataFilterItem | DataFilters)[];
    if (options.filter != null) {
      filters.push(this.buildFilter(options.filter));
    }
    if (options.searchValue != null) {
      filters.push(this.buildSearch(options.searchExpr as any, options.searchOperation as any, options.searchValue));
    }
    if (filters.length > 0) {
      result.filter = filters.length > 1 ? { logic: 'and', filters: filters } as DataFilters : filters[0];
    }
    return result;
  }

  private getByKey(key: any): any {
    let items = this._dataSource.items() as any[];
    if (items[0].data != null && items[0].data[this.key as any] != null) {
      return items.find(e => e.key == key).data;
    }
    return (items.find(e => e[this.key as any] == key));
  }

  get filter(): DataFilterItem | DataFilters | undefined {
    let filter = this._dataSource.filter();
    if (filter != null) {
      return this.buildFilter(this._dataSource.filter());
    } else {
      return undefined;
    }
  }
  set filter(filter: DataFilterItem | DataFilters | undefined) {
    if (filter != null) {
      this._filter = filter;
      this._dataSource.filter(this.buildDxFilter(filter));
      this._dataSource.reload();
    }
  }

  get sort(): DataSortItem[] | undefined {
    let sort = this._dataSource.sort();
    if (sort != null) {
      return this.buildSort(this._dataSource.sort());
    } else {
      return undefined;
    }
  }
  set sort(sort: DataSortItem[] | undefined) {
    if (sort != null) {
      this._dataSource.sort(this.buildDxSort(sort) as any);
      this._dataSource.reload();
    }
  }
  get pageSize(): number | undefined {
    let pageSize = this._dataSource.pageSize();
    if (pageSize != null) {
      return pageSize;
    } else {
      return undefined;
    }
  }
  set pageSize(pageSize: number | undefined) {
    if (pageSize != null) {
      this._dataSource.pageSize(pageSize);
      this._dataSource.reload();
    }
  }

  get page(): number | undefined {
    let page = this._dataSource.pageIndex();
    if (page != null) {
      return page + 1;
    } else {
      return undefined;
    }
  }
  set page(page: number | undefined) {
    if (page != null) {
      this._dataSource.pageIndex(page - 1);
      this._dataSource.reload();
    }
  }

  get data(): T[] | undefined {
    return this._dataSource.items();
  }

  set data(data: T[] | undefined) {
    if (data != null && this.isLocal) {
      let store = this._dataSource.store() as ArrayStore;
      store.clear();
      for (let item of data) {
        store.insert(item);
      }
    }
  }

  get select(): DataFieldItem[] {
    return this._select;
  }

  set select(select: DataFieldItem[]) {
    this._select = select;
  }

  get groupBy(): DataFieldItem[] {
    return this._groupBy;
  }

  set groupBy(groupBy: DataFieldItem[]) {
    this._groupBy = groupBy;
  }

  get total(): number | undefined {
    return this.count;
  }

  get autoBind(): boolean | undefined {
    return this._autoBind;
  }

  set autoBind(value: boolean) {
    this._autoBind = value;
  }
  get hasCount(): boolean | undefined {
    return this._hasCount;
  }
  set hasCount(hasCount: boolean) {
    this._hasCount = hasCount;
  }
  reload(): void {
    this.autoBind = true;
    this._dataSource.reload();
  }
}
