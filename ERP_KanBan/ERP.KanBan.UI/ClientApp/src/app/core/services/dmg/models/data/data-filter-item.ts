export interface DataFilterItem {
    operator: 'eq' | 'neq' | 'isnull' | 'isnotnull' | 'lt' | 'lte' | 'gt' | 'gte' |
    'startswith' | 'endswith' | 'contains' | 'doesnotcontain' | 'isempty' | 'isnotempty';
    field: string;
    value: any;
}
