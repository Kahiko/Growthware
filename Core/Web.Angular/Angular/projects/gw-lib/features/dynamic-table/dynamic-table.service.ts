import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
// Library
import { ICallbackButton } from '@Growthware/shared/models';
import { GWCommon } from '@Growthware/common-code';
import { SearchCriteria, SearchCriteriaNVP } from '@Growthware/features/search';
// Feature
import * as DefaultData from './dynamic-table.config.json';
import { IDynamicTableConfiguration } from './dynamic-table-configuration.model';

@Injectable({
  providedIn: 'root'
})
export class DynamicTableService {
  private _TableConfigurations: IDynamicTableConfiguration[] = [];

  public tableConfigurationsChanged = new Subject<boolean>();

  constructor(private _GWCommon: GWCommon) {
    // Load the default data for the growthware application
    if(!this._GWCommon.isNullOrUndefined(DefaultData)) {
      this.setTableConfiguration(DefaultData);
    }
  }

  /**
   * Returns an array of objects that adhere to ICallbackButton interface
   *
   * @param {string} name
   * @return {*}  {ICallbackButton[]}
   * @memberof DynamicTableService
   */
  public getButtons(name: string): ICallbackButton[] {
    const mRetVal: ICallbackButton[] = new Array<ICallbackButton>();
    const mTableConfiguration: IDynamicTableConfiguration = this._TableConfigurations.filter(x => x.name.toLocaleLowerCase() == name.toLocaleLowerCase())[0];
    if(!this._GWCommon.isNullOrUndefined(mTableConfiguration)) {
      mTableConfiguration.buttons.forEach((element: ICallbackButton) => {
        mRetVal.push(element);
      });
    } else {
      console.log(`Could not find the "${name}" configuration!`);
    }
    return mRetVal;
  }

  /**
   * Intended to get an initial SearchCriteria Name Value Pare object
   * using the DynamicTableService.
   *
   * @param {string} name
   * @return {*}  {SearchCriteriaNVP}
   * @memberof SearchService
   */
  public getSearchCriteriaFromConfig(name: string, tableConfiguration: IDynamicTableConfiguration): SearchCriteriaNVP {
    const mSearchCriteria: SearchCriteria = new SearchCriteria([''],[''],1,'',1);
    const mRetVal: SearchCriteriaNVP = new SearchCriteriaNVP(name, mSearchCriteria);
    if(this._GWCommon.isNullOrUndefined(tableConfiguration)) {
      throw new Error(`Could not find the "${name}" configuration!`);
    }
    const mSortColumnInfoArray: Array<string> = [];
    const mSearchColumnArray: Array<string> = [];
    tableConfiguration.columns.forEach((item) => {
      if (item.sortSelected) {
        const mSortColumnInfo: any = item.name + '=' + item.direction;
        mSortColumnInfoArray.push(mSortColumnInfo);
      }
    });
    tableConfiguration.columns.forEach((item) => {
      if (item.searchSelected) {
        const mSearchColumn: any = item.name;
        mSearchColumnArray.push(mSearchColumn);
      }
    });
    mSearchCriteria.searchColumns = mSearchColumnArray;
    mSearchCriteria.sortColumns = mSortColumnInfoArray;
    mSearchCriteria.pageSize = tableConfiguration.numberOfRows;
    mSearchCriteria.searchText = '';
    mSearchCriteria.selectedPage = 1;
    mRetVal.payLoad = mSearchCriteria;
    return mRetVal;
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
    if(this._GWCommon.isNullOrUndefined(mRetVal)) {
      if(this._TableConfigurations.length != 0) {
        throw new Error(`Could not find the "${name}" configuration!`);
      } else {
        throw new Error('TableConfigurations have not been loaded yet!');
      }
    }
    return mRetVal;
  }

  /**
   * Accepts json data as a string and converts it to an array of IDynamicTableConfiguration
   *
   * @param {*} data
   * @memberof DynamicTableService
   */
  public setTableConfiguration(data: any): void {
    const mDefaultData: IDynamicTableConfiguration[] = JSON.parse(JSON.stringify(data));
    for (let index = 0; index < mDefaultData.length; index++) {
      this._TableConfigurations.push(mDefaultData[index]);
    }
    this.tableConfigurationsChanged.next(true);
  }

  /**
   * @description Allows an outside process to change the default table configurations
   *
   * @memberof DynamicTableService
   */
  public set tableConfigurations(tableConfigurations: IDynamicTableConfiguration[]) {
    if(!this._GWCommon.isNullOrUndefined(tableConfigurations) && tableConfigurations.length > 0) {
      this._TableConfigurations = tableConfigurations;
    } else {
      throw('tableConfigurations can not be null, undefined or empty!');
    }
  }
}