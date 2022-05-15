import { DatePipe } from '@angular/common';
export class GWCommon {
  static addOrUpdateArray(yourArray: any[], objectWithId: any): void {
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

  static get baseURL(): string {
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
  static formatData(data: any, format: string): any {
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
  static formatDate(sqlDate: any): any {
    const mDateTime = new Date(sqlDate);
    const mMask = 'dddd, MMMM Do YYYY, h:mm:ss a';
    const mDatepipe: DatePipe = new DatePipe('en-US');
    const mFormattedDate = mDatepipe.transform(mDateTime, mMask);
    return mFormattedDate;
  }

  /**
   * Determines if the obj is a function
   *
   * @static
   * @param {*} obj
   * @return {*}  {boolean}
   * @memberof GWCommon
   */
  static isFunction(obj: any): boolean {
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
  static isNullorEmpty(str: string): boolean {
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
  static isNullOrUndefined(obj: any): boolean {
    if (obj == null || obj === null || typeof obj === 'undefined') {
      return true;
    }
    return false;
  }
}
