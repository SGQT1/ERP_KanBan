import { DataFilterItem } from './data-filter-item';
import { DataFilters } from './data-filters';
import { DataSortItem } from './data-sort-item';
import { DataSourceTransport } from './data-source-transport';
import { DataFieldItem } from './data-field-item';

export interface DataSourceOptions<T> {
    key?: string | string[];
    filter?: DataFilterItem | DataFilters;
    sort?: DataSortItem[];
    page?: number;
    pageSize?: number;
    select?: DataFieldItem[];
    groupBy?: DataFieldItem[];
    autoBind?: boolean;
    data?: T[];
    transport?: DataSourceTransport<T>;
    hasCount?: boolean;
}
