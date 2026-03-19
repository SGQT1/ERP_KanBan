import { DataSourceOptions } from './data-source-options';
import { DataSource } from './data-source';

import { DxDataSourceMiddleware } from './dx-data-source-middleware';

export class DataSourceFactory {
    static DxDataSource<T>(options: DataSourceOptions<T>): DataSource<T> {
        return new DxDataSource<T>(options);
    }
}

class DxDataSource<T> extends DataSource<T> {
    constructor(options: DataSourceOptions<T>) {
        super();
        this.middleware = new DxDataSourceMiddleware<T>(options);
    }
}
