import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { DynamicTableConfig } from './dynamic-table-configuration.model';
import * as DefaultData from './dynamic-table.config.json';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';
import { Common } from '../../common';


export interface IResults {
  id: string,
  data: any
}
@Injectable({
  providedIn: 'root'
})
export class GWLibDynamicTableService {
  private _HttpClient: HttpClient;
  private _TableConfigurations: IDynamicTableConfiguration[] = [];
  private _TableData: Map<string, any>;

  public dataChanged = new Subject<string>();

  constructor(httpClient: HttpClient) {
    this._HttpClient = httpClient;
    this._TableData = new Map<string, any>();
    // Load the default data for the growthware application
    const mDefaultData: IDynamicTableConfiguration[] = JSON.parse(JSON.stringify(DefaultData));
    for (let index = 0; index < mDefaultData.length; index++) {
      this._TableConfigurations.push(mDefaultData[index]);
    }
  }

  /**
   * @description Will call and API, put the data in the store and
   *
   * @param {string} name
   * @param {string} url
   * @memberof DynamicTableService
   */
  public getData(name: string): any[] {
    return this._TableData.get(name.toLowerCase());
  }

  /**
   * @description Returns an IDynamicTableConfiguration given the configuration name
   *
   * @param {string} name
   * @return {*}  {IDynamicTableConfiguration}
   * @memberof DynamicTableService
   */
  public getTableConfiguration(name: string): IDynamicTableConfiguration {
    const mRetVal = this._TableConfigurations.filter(x => x.name.toLocaleLowerCase() == name.toLocaleLowerCase())[0];
    if(Common.isNullOrUndefined(mRetVal)) {
      throw new Error(`Could not find the "${name}" configuration!`);
    }
    return mRetVal;
  }

  /**
   * @description Allows outside processes to change the table configurations
   *
   * @memberof DynamicTableService
   */
  public set tableConfigurations(configURL: string) {
    if(!Common.isNullorEmpty(configURL)){
      // Reload this._TableConfigurations using the URL
    } else {
      throw('configURL can not be null or empty!');
    }
  }

  /**
   * @description Allows outside processes to set data.  This can be useful
   * in such cases where the data source do not come from an API or require
   * manipulation before being "sent" to the dynamic table.
   *
   * @param {string} name
   * @param {IResults} results
   * @memberof DynamicTableService
   */
  public setData(name: string, results: any[]) {
    if(!Common.isNullorEmpty(name) && !Common.isNullOrUndefined(results)) {
      this._TableData.set(name.toLowerCase(), results);
      this.dataChanged.next(name);
    } else {
      throw('The name and or the data can not be null or undefined');
    }
  }
}
