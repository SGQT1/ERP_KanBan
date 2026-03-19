import { DataFilterItem } from './data-filter-item';

export interface DataFilters {
    logic: 'and' | 'or';
    filters: (DataFilterItem | DataFilters)[];
}
