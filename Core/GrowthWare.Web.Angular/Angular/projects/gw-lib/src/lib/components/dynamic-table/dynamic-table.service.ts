import { DynamicTableConfig } from './dynamic-table-config.model';
import * as DefaultData from './dynamic-table.config.json';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';
import { Common } from '../../common';

export class DynamicTableService {
  private _TableConfigurations: IDynamicTableConfiguration[] = [];

  public getTableConfiguration(name: string): IDynamicTableConfiguration {
    const mRetVal = this._TableConfigurations.filter(x => x.name.toLocaleLowerCase() == name.toLocaleLowerCase())[0];
    if(!Common.isNullOrUndefined(mRetVal)) {

    }
    return mRetVal;
  }

  public set TableConfigurations(config: IDynamicTableConfiguration[]) {
    if(config != null && config.length > 0) {
      this._TableConfigurations = config;
    }
  }

  constructor() {
    console.log(DefaultData);
    this._TableConfigurations = JSON.parse(JSON.stringify(DefaultData));
    console.log(this._TableConfigurations);
  }
}
