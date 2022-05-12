import { DatePipe } from '@angular/common';
export class Common {
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

  static formatDate(sqlDate: any) {
    const mDateTime = new Date(sqlDate);
    const mMask = 'dddd, MMMM Do YYYY, h:mm:ss a';
    const mDatepipe: DatePipe = new DatePipe('en-US');
    const mFormattedDate = mDatepipe.transform(mDateTime, mMask);
    return mFormattedDate;
  }

  static isFunction(obj: any): boolean {
    return typeof obj === 'function';
  }

  static isNullorEmpty(str: string) {
    if (!str || 0 === str.length || !str || /^\s*$/.test(str)) {
      return true;
    }
    return false;
  }

  static isNullOrUndefined(obj: any) {
    if (obj == null || obj === null || typeof obj === 'undefined') {
      return true;
    }
    return false;
  }
}
