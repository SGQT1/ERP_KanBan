import { DataLoadOptions } from './data-load-options';
import { QueryResult } from './query-result';

export interface DataSourceTransport<T> {
    read?: (options: DataLoadOptions) => Promise<QueryResult<T> | T[]>;
    create?: (item: T) => Promise<T>;
    update?: (item: T) => Promise<T>;
    remove?: (item: T) => Promise<void>;
}
