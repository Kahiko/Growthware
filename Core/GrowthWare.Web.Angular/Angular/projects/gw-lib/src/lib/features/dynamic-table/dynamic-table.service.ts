import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import * as DefaultData from './dynamic-table.config.json';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';
import { GWCommon } from 'projects/gw-lib/src/lib/common';
import { SearchCriteria } from 'projects/gw-lib/src/lib/services/search.service';

export interface IResults {
  id: string,
  data: any
}
@Injectable({
  providedIn: 'root'
})
export class GWLibDynamicTableService {
  private _Criteria: Map<string, SearchCriteria>;
  private _TableConfigurations: IDynamicTableConfiguration[] = [];
  private _TableData: Map<string, any>;

  public dataChanged = new Subject<string>();
  public dataRequested = new Subject<string>();

  constructor() {
    this._TableData = new Map<string, any>();
    this._Criteria = new Map<string, SearchCriteria>();
    // Load the default data for the growthware application
    const mDefaultData: IDynamicTableConfiguration[] = JSON.parse(JSON.stringify(DefaultData));
    for (let index = 0; index < mDefaultData.length; index++) {
      this._TableConfigurations.push(mDefaultData[index]);
    }
  }

  /**
   * @description Will return the from the _TableData Map
   *
   * @param {string} name
   * @param {string} url
   * @memberof DynamicTableService
   */
  public getData(name: string): any[] {
    return this._TableData.get(name.toLowerCase());
  }

  /**
   * Returns a SearchCriteria object given the name or new SearchCriteria('','','',1,1,'')
   *
   * @param {string} name
   * @return {*}  {SearchCriteria}
   * @memberof GWLibDynamicTableService
   */
  public getSearchCriteria(name: string): SearchCriteria {
    return this._Criteria.get(name.toLocaleLowerCase()) || new SearchCriteria('','','',1,1,'');
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
    if(GWCommon.isNullOrUndefined(mRetVal)) {
      throw new Error(`Could not find the "${name}" configuration!`);
    }
    return mRetVal;
  }

  /**
   * @description Allows an outside process to change the default table configurations
   *
   * @memberof DynamicTableService
   */
  public set tableConfigurations(tableConfigurations: IDynamicTableConfiguration[]) {
    if(!GWCommon.isNullOrUndefined(tableConfigurations) && tableConfigurations.length > 0){
      this._TableConfigurations = tableConfigurations;
    } else {
      throw('tableConfigurations can not be null, undefined or empty!');
    }
  }

  /**
   * @description Calls "next" on the dataRequested Subject passing the component name
   *
   * @summary requestData faciliates when the GWLibDynamicTableComponent.getData
   * methods is beining overriden.  When any internal methods to the component are
   * being used (pagination controls and what not) the overriden getData
   * methods needs to be fired.
   * @param {string} componentName
   * @memberof GWLibDynamicTableService
   */
  public requestData(componentName: string): void {
    this.dataRequested.next(componentName);
  }

  /**
   * @description Allows an outside process to set data.  This can be useful
   * in such cases where the data source do not come from an API or require
   * manipulation before being "sent" to the dynamic table.
   *
   * @param {string} name
   * @param {IResults} results
   * @memberof DynamicTableService
   */
  public setData(name: string, results: any[]) {
    if(!GWCommon.isNullorEmpty(name) && !GWCommon.isNullOrUndefined(results)) {
      this._TableData.set(name.toLowerCase(), results);
      this.dataChanged.next(name);
    } else {
      throw('The name and or the data can not be null or undefined');
    }
  }
}
