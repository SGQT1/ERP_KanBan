import { DataFilterItem } from './data-filter-item';
import { DataFilters } from './data-filters';
import { DataSortItem } from './data-sort-item';
import { DataSourceOptions } from './data-source-options';
import { DataFieldItem } from './data-field-item';

export interface DataSourceMiddleware<T> {
    filter?: DataFilterItem | DataFilters | DataFilterItem[];
    sort?: DataSortItem | DataSortItem[] | (DataSortItem | DataSortItem[])[];
    pageSize?: number;
    page?: number;
    select?: DataFieldItem | DataFieldItem[] | (DataFieldItem | DataFieldItem[])[];
    groupBy?: DataFieldItem | DataFieldItem[] | (DataFieldItem | DataFieldItem[])[];
    data?: T[];
    autoBind?: boolean;
    hasCount?: boolean;
    readonly total?: number;
    readonly dataSource: any;

    init(options: DataSourceOptions<T>): void;
    reload(): void;
}
