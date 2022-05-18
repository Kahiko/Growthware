import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import * as DefaultData from './dynamic-table.config.json';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';
import { GWCommon } from 'projects/gw-lib/src/lib/common';
import { GWLibSearchService } from '../../services/search.service';

export interface IResults {
  id: string,
  data: any
}
@Injectable({
  providedIn: 'root'
})
export class GWLibDynamicTableService {
  private _TableConfigurations: IDynamicTableConfiguration[] = [];
  private _TableData: Map<string, any>;
  private _TotalPages: Map<string, number>;

  // Suports when GWLibDynamicTableComponent.getData has been overridden and used by GWLibDynamicTableComponent
  public dataChanged = new Subject<string>();
  // Supports the GWLibPagerComponent
  public totalPagesChanged = new Subject<string>();

  constructor(private _SearchSvc: GWLibSearchService) {
    this._TableData = new Map<string, any>();
    this._TotalPages = new Map<string, number>();
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
   * @memberof DynamicTableService
   */
  public getData(name: string): any[] {
    return this._TableData.get(name.trim().toLowerCase());
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
   * Return the total number of pages for the given compentent name
   *
   * @param {string} name
   * @return {*}  {number}
   * @memberof GWLibDynamicTableService
   */
  public getTotalPages(name: string): number {
    return this._TotalPages.get(name.trim().toLowerCase()) || -1;
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
   * @description Stores the results by name.
   *
   * @param {string} name
   * @param {IResults} results
   * @memberof DynamicTableService
   * @summary Supports when GWLibDynamicTableComponent.getData has been overritten
   */
  public setData(name: string, results: any[]) {
    if(!GWCommon.isNullorEmpty(name) && !GWCommon.isNullOrUndefined(results)) {
      if(results.length > 0) {
        const mFirstRow = results[0];
        const mSearchCriteria = this._SearchSvc.getSearchCriteria(name);
        const mTotalNumberofPages: number = Math.floor(parseInt(mFirstRow['TotalRecords']) / mSearchCriteria.pageSize);
        this._TotalPages.set(name.trim().toLowerCase(), mTotalNumberofPages);
        this._TableData.set(name.toLowerCase(), results);
        this.dataChanged.next(name);
      } else {
        console.log('results has no rows.')
      }
    } else {
      throw('The name and or the data can not be null or undefined');
    }
  }
}
