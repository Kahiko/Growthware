import { Injectable } from '@angular/core';

import * as DefaultData from './dynamic-table.config.json';
import { IDynamicTableConfiguration } from '@Growthware/Lib/src/lib/models';
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
   * @description Allows an outside process to change the default table configurations
   *
   * @memberof DynamicTableService
   */
   public set tableConfigurations(tableConfigurations: IDynamicTableConfiguration[]) {
    if(!this._GWCommon.isNullOrUndefined(tableConfigurations) && tableConfigurations.length > 0){
      this._TableConfigurations = tableConfigurations;
    } else {
      throw('tableConfigurations can not be null, undefined or empty!');
    }
  }
}
