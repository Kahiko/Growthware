import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import * as DefaultData from './dynamic-table.config.json';
import { ICallbackButton, IDynamicTableConfiguration } from '@Growthware/Lib/src/lib/models';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

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
