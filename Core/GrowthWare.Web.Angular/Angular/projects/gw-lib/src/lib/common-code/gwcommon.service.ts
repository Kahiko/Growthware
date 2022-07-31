import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GWCommon {

  public addOrUpdateArray(yourArray: any[], objectWithId: any): void {
    var mExistingIds = yourArray.map((obj) => obj.id);

    if (!mExistingIds.includes(objectWithId.id)) {
      yourArray.push(objectWithId);
    } else {
      yourArray.forEach((element, index) => {
        if (element.id === objectWithId.id) {
          yourArray[index] = objectWithId;
        }
      });
    }
  }

  public get baseURL(): string {
    let mCurrentLocation = window.location;
    let mPort = mCurrentLocation.port;
    const mCurrentPath = window.location.pathname;
    if (mPort === '80' || mPort.length === 0) {
      mPort = '';
    } else {
      mPort = ':' + mPort;
    }
    let mURL =
      mCurrentLocation.protocol + '//' + mCurrentLocation.hostname + mPort;
    const mLastSlashPos = mCurrentPath.lastIndexOf('/');
    if (mLastSlashPos !== 0) {
      mURL = mURL + '/' + mCurrentPath;
    } else {
      mURL = mURL + '/';
    }
    return mURL;
  }

  /**
   * Formats data
   *
   * @param {*} data
   * @param {string} format
   * @returns
   * @memberof DynamicTableComponent
   */
  public formatData(data: any, format: string): any {
    if (this.isNullOrUndefined(data)) {
      return '&nbsp;';
    }
    switch (format.toLowerCase()) {
      case 'date':
        return this.formatDate(data);
      default:
        const mMsg = "'" + format + "' is an unknown format";
        throw(mMsg);
        break;
    }
  }

  /**
   * Formats a SQL date as 'dddd, MMMM Do YYYY, h:mm:ss a'
   *
   * @static
   * @param {*} sqlDate
   * @return {*}  {*}
   * @memberof GWCommon
   */
  public formatDate(sqlDate: any): any {
    const mDateTime = new Date(sqlDate);
    const mMask = 'dddd, MMMM Do YYYY, h:mm:ss a';
    const mDatepipe: DatePipe = new DatePipe('en-US');
    const mFormattedDate = mDatepipe.transform(mDateTime, mMask);
    return mFormattedDate;
  }

  public getTotalRecords(data: Array<any>): number {
    let mRetVal = -1;
    if(data && data.length > 0) {
      const mFirstRow = data[0];
      if(mFirstRow) {
        const mTotalRecords = mFirstRow['TotalRecords'];
        if(mTotalRecords) {
          mRetVal = mTotalRecords;
        }
      }
    }
    return mRetVal
  }

  /**
   * Determines if the obj is a function
   *
   * @static
   * @param {*} obj
   * @return {*}  {boolean}
   * @memberof GWCommon
   */
  public isFunction(obj: any): boolean {
    return typeof obj === 'function';
  }

  /**
   * Determins if the str is null or empty (ie length === 0)
   *
   * @static
   * @param {string} str
   * @return {*}
   * @memberof GWCommon
   */
  public isNullOrEmpty(str: string): boolean {
    if (!str || 0 === str.length || !str || /^\s*$/.test(str)) {
      return true;
    }
    return false;
  }

  /**
   * Determines if the obj is null or undefined
   *
   * @static
   * @param {*} obj
   * @return {*}
   * @memberof GWCommon
   */
  public isNullOrUndefined(obj: any): boolean {
    if (obj == null || obj === null || typeof obj === 'undefined') {
      return true;
    }
    return false;
  }

  public isNumber(value: string | number): boolean
  {
    let mRetVal: boolean = false;
    if(!this.isNullOrUndefined(value) &&
      !this.isNullOrEmpty(value.toString()) &&
      !isNaN(Number(value.toString()))) {
        mRetVal = true;
    }
    return mRetVal;
  }
}

