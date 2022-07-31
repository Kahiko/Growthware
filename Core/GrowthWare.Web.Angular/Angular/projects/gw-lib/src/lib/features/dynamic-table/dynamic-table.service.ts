import { Injectable } from '@angular/core';

import * as DefaultData from './dynamic-table.config.json';
import { ICallbackButton, IDynamicTableConfiguration } from '@Growthware/Lib/src/lib/models';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

@Injectable({
  providedIn: 'root'
})
export class DynamicTableService {
  private _TableConfigurations: IDynamicTableConfiguration[] = [];

  constructor(private _GWCommon: GWCommon) {
    // Load the default data for the growthware application
    const mDefaultData: IDynamicTableConfiguration[] = JSON.parse(JSON.stringify(DefaultData));
    for (let index = 0; index < mDefaultData.length; index++) {
      this._TableConfigurations.push(mDefaultData[index]);
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
   * @description Returns an IDynamicTableConfiguration given the configuration name
   *
   * @param {string} name
   * @return {*}  {IDynamicTableConfiguration}
   * @memberof DynamicTableService
   */
   public getTableConfiguration(name: string): IDynamicTableConfiguration {
    const mRetVal = this._TableConfigurations.filter(x => x.name.toLocaleLowerCase() == name.toLocaleLowerCase())[0];
    if(this._GWCommon.isNullOrUndefined(mRetVal)) {
      throw new Error(`Could not find the "${name}" configuration!`);
    }
    return mRetVal;
  }

  /**
   * Adds or updates the _TableConfigurations array with the tableConfiguration
   * Indented to change a tableConfiguration most likely use case is when the
   * rowClick or rowDoubleClick methods need to be overridden b/c functions
   * are not in the JSON data.
   * @memberof DynamicTableService
   */
  public set tableConfiguration(tableConfiguration: IDynamicTableConfiguration) {
    if(!this._GWCommon.isNullOrUndefined(tableConfiguration)) {
      var mExistingIds = this._TableConfigurations.map((obj) => obj.name);
      if (!mExistingIds.includes(tableConfiguration.name)) {
        this._TableConfigurations.push(tableConfiguration);
      } else {
        this._TableConfigurations.forEach((element, index) => {
          if (element.name === tableConfiguration.name) {
            this._TableConfigurations[index] = tableConfiguration;
          }
        });
      }
    }
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
