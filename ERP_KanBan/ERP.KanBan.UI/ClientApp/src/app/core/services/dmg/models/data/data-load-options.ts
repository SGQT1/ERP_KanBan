import { DataSortItem } from './data-sort-item';
import { DataFilterItem } from './data-filter-item';
import { DataFilters } from './data-filters';
import { DataFieldItem } from './data-field-item';

export interface DataLoadOptions {
    sort?: DataSortItem[];
    filter?: DataFilterItem | DataFilters;
    select?: DataFieldItem[];
    groupBy?: DataFieldItem[];
    take?: number;
    skip?: number;
    hasCount?: boolean;
}
