import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GWCommon {

  /**
   * Adds or updates an element in an array so long as the elements in the array
   * have an 'id' property.
   *
   * @param {any[]} yourArray
   * @param {*} objectWithId
   * @memberof GWCommon
   */
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

  /**
   * Returns the base URL ending in a forward slash
   *
   * @readonly
   * @type {string}
   * @memberof GWCommon
   */
  public get baseURL(): string {
    let mCurrentLocation = window.location;
    let mPort = mCurrentLocation.port;
    if (mPort === '80' || mPort.length === 0) {
      mPort = '';
    } else {
      mPort = ':' + mPort;
    }
    let mURL = mCurrentLocation.protocol + '//' + mCurrentLocation.hostname + mPort + '/';
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
    let mFormattedData = data;
    const mFormatParts: string[] = format.split(':');
    let mFormat = mFormatParts[0];
    switch (mFormat.toLowerCase()) {
      case 'date':
        mFormattedData = this.formatDate(data);
        break;
      case 'text':
        if (mFormatParts.length > 1 && data.length > 0) {
          const mDesiredLength = parseInt(mFormatParts[1]);
          if (data.length > mDesiredLength) {
            // console.log('eclipsing the data');
            // console.log(data);
            mFormattedData = data.toString().substring(0, mDesiredLength) + '...';
          }
        }
        break;
      default:
        const mMsg = "'" + format + "' is an unknown format";
        throw (mMsg);
        break;
    }
    return mFormattedData;
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
    const mDatePipe: DatePipe = new DatePipe('en-US');
    const mFormattedDate = mDatePipe.transform(mDateTime, mMask);
    return mFormattedDate;
  }

  public getTotalRecords(data: Array<any>): number {
    let mRetVal = -1;
    if (data && data.length > 0) {
      const mFirstRow = data[0];
      if (mFirstRow) {
        const mTotalRecords = mFirstRow['TotalRecords'];
        if (mTotalRecords) {
          mRetVal = mTotalRecords;
        }
      }
    }
    return mRetVal
  }

  /**
   * @description Searches hierarchical data for an object with specified properties value
   *
   * @param {*} data tree nodes tree with children items in nodesProp[] table, with one (object) or many (array of objects) roots
   * @param {(string | number)} searchValue value of searched node's  prop
   * @param {string} nameOfProperty name of the property used to compare against the value parameter
   * @param {string} nameOfChildNodes name of prop that holds child nodes array (default value of 'children')
   * @return {*}  {*} returns first object that match supplied arguments (propertyName: value) or null if no matching object was found
   * @return {*}  {(object | null)} returns first object that match supplied arguments (propertyName: value) or null if no matching object was found
   * @memberof GWCommon
   */
  public hierarchySearch(data: any, searchValue: string | number, nameOfProperty: string, nameOfChildNodes: string = 'children'): object | null {
    var i: number // iterator
    var mNode: any = null; // found node
    if (Array.isArray(data)) { // if entry object is array objects, check each object
      for (i = 0; i < data.length; i++) {
        mNode = this.hierarchySearch(data[i], searchValue, nameOfProperty, nameOfChildNodes);
        if (mNode) { // if found matching object, return it.
          return mNode;
        }
      }
    } else if (typeof data === 'object') { // standard tree node (one root)
      if (data[nameOfProperty] !== undefined && data[nameOfProperty] === searchValue) {
        return data; // found matching node
      }
    }
    if (data[nameOfChildNodes] !== undefined && data[nameOfChildNodes].length > 0) { // did not find the node but there more children to search
      return this.hierarchySearch(data[nameOfChildNodes], searchValue, nameOfProperty, nameOfChildNodes);
    } else {
      return null; // node does not match and did not find it in any of the children
    }
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

  /**
   * Determines if the value is a number
   *
   * @param {(string | number)} value
   * @return {*}  {boolean}
   * @memberof GWCommon
   */
  public isNumber(value: string | number): boolean {
    let mRetVal: boolean = false;
    if (!this.isNullOrUndefined(value) &&
      !this.isNullOrEmpty(value.toString()) &&
      !isNaN(Number(value.toString()))) {
      mRetVal = true;
    }
    return mRetVal;
  }

  /********** Natural Sorting *****************/
  /*
  * Natural Sort algorithm for Javascript - Version 0.6 - Released under MIT license
  * Author: Jim Palmer (based on chunking idea from Dave Koelle)
  * Contributors: Mike Grier (mgrier.com), Clint Priest, Kyle Adams, guillermo
  */
  public static naturalSort(a: any, b: any): number {
    const re = /(^-?[0-9]+(\.?[0-9]*)[df]?e?[0-9]?$|^0x[0-9a-f]+$|[0-9]+)/gi
    const sre = /(^[ ]*|[ ]*$)/g
    const dre = /(^([\w ]+,?[\w ]+)?[\w ]+,?[\w ]+\d+:\d+(:\d+)?[\w ]?|^\d{1,4}[\/\-]\d{1,4}[\/\-]\d{1,4}|^\w+, \w+ \d+, \d{4})/
    const hre = /^0x[0-9a-f]+$/i
    const ore = /^0/
    // convert all to strings and trim()
    const x = a.toString().replace(sre, '') || ''
    const y = b.toString().replace(sre, '') || ''
    // chunk/tokenize
    const xN = x.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0')
    const yN = y.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0')
    // numeric, hex or date detection
    const xD = parseInt(x.match(hre)) || (xN.length != 1 && x.match(dre) && Date.parse(x))
    const yD = parseInt(y.match(hre)) || xD && y.match(dre) && Date.parse(y) || null;
    // first try and sort Hex codes or Dates
    if (yD)
      if (xD < yD) return -1;
      else if (xD > yD) return 1;
    // natural sorting through split numeric strings and default strings
    for (var cLoc = 0, numS = Math.max(xN.length, yN.length); cLoc < numS; cLoc++) {
      // find floats not starting with '0', string or 0 if not defined (Clint Priest)
      var oFxNcL = !(xN[cLoc] || '').match(ore) && parseFloat(xN[cLoc]) || xN[cLoc] || 0;
      var oFyNcL = !(yN[cLoc] || '').match(ore) && parseFloat(yN[cLoc]) || yN[cLoc] || 0;
      // handle numeric vs string comparison - number < string - (Kyle Adams)
      if (isNaN(oFxNcL) !== isNaN(oFyNcL)) return (isNaN(oFxNcL)) ? 1 : -1;
      // rely on string comparison if different types - i.e. '02' < 2 != '02' < '2'
      else if (typeof oFxNcL !== typeof oFyNcL) {
        oFxNcL += '';
        oFyNcL += '';
      }
      if (oFxNcL < oFyNcL) return -1;
      if (oFxNcL > oFyNcL) return 1;
    }
    return 0;
  }


  /**
   * sleep for x number of milliseconds
   *
   * @param {number} ms
   * @return {*}
   * @memberof UtilityService
   */
  public async sleep(ms: number) {
    await new Promise(resolve => setTimeout(()=>resolve(true), ms)).then(()=> {/* do nothing */});
  }

  /**
   * @description sortData is a basic sort for an array.
   *
   * @requires #sortBy a basic comparer function
   *
   * @param {Array} columnName this is the array to be sorted
   * @param {String} columnName this is the name of the column or element in the array
   * @param {String} orderByDirection this is the desired direction valid options asc or desc
   *
   * @returns {Array} a sorted array of object given a object property
   *
   * @usage:
   * @usage:
   * var myArray = [
   *  {'First': '', 'Last': '', 'Middle': ''},
   *  {'First': '', 'Last': '', 'Middle': ''}
   * ]; // of course your array will have many "rows" not just two
   * myArray = svc.SortArray(myArray, 'First', 'asc'); // sort your array by the element "First"
   * myArray = svc.SortArray(myArray, 'First', 'asc'); // sort your array by the element "Last"
   *
   */
  public static sortArray(dataArray: any, columnName: string, orderByDirection: string) {
    const isArray = (Object.prototype.toString.call(dataArray) === "[object Array]");
    if (!isArray) {
      console.log('Common.sortArray: dataArray is not an "Array" exiting!');
      return;
    }
    let mReverse = false;

    if (orderByDirection.toUpperCase() === 'ASC') {
      mReverse = true;
    }
    const newArray = dataArray.slice();
    dataArray = [];
    newArray.sort(this.sortBy(columnName, mReverse));
    dataArray = newArray.slice();
    return dataArray;
  }

  /**
   * @description Returns a function which will sort an
   * array of objects by the given key.
   *
   */
  private static sortBy(key: string | number, reverse: boolean): any {
    // Move smaller items towards the front
    // or back of the array depending on if
    // we want to sort the array in reverse
    // order or not.
    const moveSmaller = reverse ? 1 : -1;

    // Move larger items towards the front
    // or back of the array depending on if
    // we want to sort the array in reverse
    // order or not.
    const moveLarger = reverse ? -1 : 1;

    /**
     * @param  {*} a
     * @param  {*} b
     * @return {Number}
     */
    return (a: any, b: any) => {
      if (a[key] < b[key]) {
        return moveSmaller;
      }
      if (a[key] > b[key]) {
        return moveLarger;
      }
      return 0;
    };
  }
}