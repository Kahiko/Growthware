import { Injectable } from '@angular/core';

import { DynamicTableConfig } from './dynamic-table-configuration.model';
import * as DefaultData from './dynamic-table.config.json';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';
import { Common } from '../../common';

@Injectable({
  providedIn: 'root'
})
export class DynamicTableService {
  private _TableConfigurations: IDynamicTableConfiguration[] = [];

  public getTableConfiguration(name: string): IDynamicTableConfiguration {
    const mRetVal = this._TableConfigurations.filter(x => x.name.toLocaleLowerCase() == name.toLocaleLowerCase())[0];
    if(!Common.isNullOrUndefined(mRetVal)) {

    }
    return mRetVal;
  }

  public set TableConfigurations(configURL: string) {
    if(!Common.isNullorEmpty(configURL)){
      // Reload this._TableConfigurations using the URL
    } else {
      throw('configURL can not be null or empty!');
    }
  }

  constructor() {
    // Load the default data for the growthware application
    this._TableConfigurations = JSON.parse(JSON.stringify(DefaultData));
    console.log('Default TableConfigurations:');
    console.log(this._TableConfigurations);
  }
}
