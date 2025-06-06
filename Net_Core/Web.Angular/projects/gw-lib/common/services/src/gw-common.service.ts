import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
// Library
import { CallbackMethod, IMenuData, INavItem, ITotalRecords } from '@growthware/common/interfaces';

@Injectable({
  providedIn: 'root'
})
export class GWCommon {

  /**
   * Adds or updates an element in an array so long as the elements in the array
   * has an 'id' property.
   *
   * @param {any[]} yourArray
   * @param {*} objectWithProperty
   * @memberof GWCommon
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public addOrUpdateArray(yourArray: any[], objectWithProperty: any, propertyName: string = 'id'): void {
    if (propertyName in objectWithProperty) {
      const index = yourArray.findIndex((obj) => obj.id === objectWithProperty.id);
      if (index === -1) {
        yourArray.push(objectWithProperty);
      } else {
        yourArray[index] = objectWithProperty;
      }
    } else {
      console.warn('addOrUpdateArray: objectWithProperty does not have the "' + propertyName + '" property');
      throw new Error('objectWithProperty does not have an id property');
    }
  }

  /**
   * A function to build hierarchical INavItem[] from the flat menuData.
   *
   * @param {IMenuData[]} menuData - description of parameter
   * @return {INavItem[]} description of return value
   */
  public buildNavItems(menuData: IMenuData[]): INavItem[] {
    // console.log('menuData', menuData);
    const mRetItems: INavItem[] = [];
    const items: INavItem[] = [];
    // build hierarchical source.
    if (menuData && menuData.length) {
      for (let i = 0; i < menuData.length; i++) {
        const item = menuData[i];
        const id = item.Id;
        const label = item.Title;
        const description = item.Description;
        const action = item.URL.replace('?Action=', '').replace('&Action=', '');
        const parentId = item.ParentId;
        if (items[parentId]) {
          const item: INavItem = { action: action, description: description, items: [], label: label, parentId: parentId };
          if (!items[parentId].items) {
            items[parentId].items = [];
          }
          items[parentId].items[items[parentId].items.length] = item;
          items[id] = item;
        }
        else {
          items[id] = { action: action, description: description, items: [], label: label, parentId: parentId };
          mRetItems[id] = items[id];
        }
      }
    }
    // console.log('source', source);
    return mRetItems;
  }

  /**
   * A function that builds an unordered list within a given HTMLUListElement based on the provided INavItem array, and optionally invokes a callback method.
   *
   * @param {HTMLUListElement} parent - The parent HTMLUListElement where the unordered list will be built.
   * @param {INavItem[]} items - The array of INavItem objects used to populate the unordered list.
   * @param {CallbackMethod} [callbackMethod] - An optional callback method to be invoked.
   * @return {void} This function does not return a value.
   */
  public buildUL(parent: HTMLUListElement, items: INavItem[], callbackMethod?: CallbackMethod): void {
    items.forEach(element => {
      if (element.label) {
        // create LI element and append it to the parent element.
        const mListItem: HTMLLIElement = document.createElement('li');
        const mAnchor: HTMLAnchorElement = document.createElement('a');
        Object.assign(mAnchor, { 'title': element.description });
        const mSpan: HTMLSpanElement = document.createElement('span');
        mSpan.innerHTML = element.label;
        mAnchor.appendChild(mSpan);
        mListItem.appendChild(mAnchor);
        // if there are sub items, call the buildUL function.
        if (element.items && element.items.length > 0) {
          mListItem.setAttribute('class', 'has-sub');
          Object.assign(mAnchor, { href: '#' });
          const mHTMLUListElement: HTMLUListElement = document.createElement('ul');
          mListItem.appendChild(mHTMLUListElement);
          this.buildUL(mHTMLUListElement, element.items, callbackMethod);
        } else {
          if (callbackMethod) {
            Object.assign(mAnchor, { onclick: () => callbackMethod(element.action) });
          }
        }
        parent.appendChild(mListItem);
      }
    });
  }

  /**
   * Returns the base URL ending in a forward slash
   *
   * @readonly
   * @type {string}
   * @memberof GWCommon
   */
  public get baseURL(): string {
    const mCurrentLocation = window.location;
    let mPort = mCurrentLocation.port;
    if (mPort === '80' || mPort.length === 0) {
      mPort = '';
    } else {
      mPort = ':' + mPort;
    }
    const mURL = mCurrentLocation.protocol + '//' + mCurrentLocation.hostname + mPort + '/';
    return mURL;
  }


  /**
   * Returns the base URL without the port.
   *
   * @return {string} The base URL without the port.
   */
  public get baseURLWithoutPort(): string {
    const mCurrentLocation = window.location;
    const mURL = mCurrentLocation.protocol + '//' + mCurrentLocation.hostname + '/';
    return mURL;
  }

  public capitalizeFirstLetter(stringToCap: string): string {
    if (this.isNullOrEmpty(stringToCap)) { return stringToCap; }
    return stringToCap.charAt(0).toUpperCase() + stringToCap.slice(1);
  }

  /**
   * Compares two dates to check if they are equal.
   *
   * @param {Date} date1 - The first date to compare
   * @param {Date} date2 - The second date to compare
   * @return {boolean} true if the dates are equal, false otherwise
   */
  public datesEqual(date1: Date, date2: Date): boolean {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  }

  /**
   * Formats data
   *
   * @param {any} data - The data to be formatted
   * @param {string} format - The format to be applied to the data
   * @returns {any} The formatted data
   * @memberof DynamicTableComponent
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public formatData(data: any, format: string): any {
    if (this.isNullOrUndefined(data)) {
      return '&nbsp;';
    }
    let mFormattedData = data;
    const mFormatParts: string[] = format.split(':');
    const mFormat = mFormatParts[0];
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
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
      case 'checkBox':
        mFormattedData = data;
        break;
      case 'icon':
        mFormattedData = '';
        break;
      default: {
        const mMsg: string = '\'' + format + '\' is an unknown format';
        throw (mMsg);
        break;
      }
    }
    return mFormattedData;
  }

  /**
   * Formats a SQL date as 'dddd, MMMM Do YYYY, h:mm:ss a'
   *
   * @param {string | number | Date} sqlDate - the date to be formatted
   * @return {string | null} the formatted date, or null if the input is invalid
   */
  public formatDate(sqlDate: string | number | Date): string | null {
    const mDateTime = new Date(sqlDate);
    const mMask = 'dddd, MMMM Do YYYY, h:mm:ss a';
    const mDatePipe: DatePipe = new DatePipe('en-US');
    const mFormattedDate = mDatePipe.transform(mDateTime, mMask);
    return mFormattedDate;
  }

  /**
   * Retrieves the names of the enum values from the given enum object.
   *
   * @param {any} e - the enum object
   * @return {Array<string>} an array of enum names
   */
  public getEnumNames<T extends Record<string, number | string>>(e: T): Array<keyof T> {
    return Object.keys(e).filter(k =>
      typeof e[k] === 'number' || e[k] === k || e[e[k]]?.toString() !== k
    ) as Array<keyof T>;
  }


  /**
   * Gets the stack trace of the calling code, stripping out any irrelevant
   * stack frames. The stack trace is returned as a string with each caller
   * separated by ' => '.
   *
   * @return {string} the stack trace
   */
  public getStackTrace(): string {
    const mStackLines = new Error('').stack?.split('\n') ?? [];
    if (
      this.isNullOrUndefined(mStackLines) ||
      mStackLines.length === 0
    ) {
      return '';
    }
    const mOurCallStack = [];
    let mRetVal: string = '';
    try {
      for (let x = 0; x <= mStackLines.length; x++) {
        const mLine = mStackLines[x];
        if (mLine != 'Error' && !this.isNullOrUndefined(mLine)) {
          // Don't need the first line
          const mParts = mLine.split(' ');
          if ((mParts.length === 7)) {
            const mCaller = mParts[5];
            if (
              mCaller.indexOf('_next') === -1 &&
              mCaller.indexOf('callH') === -1
            ) {
              // we can stop b/c we have gotten all of our codes stack
              if (mCaller.indexOf('LoggingService') === -1) {
                // Don't include this class in the stack
                const mCallStackObj = { caller: mCaller, file: mParts[6] };
                mOurCallStack.push(mCallStackObj);
              }
            } else {
              break;
            }
          }
        }
      }
    } catch (error) {
      console.error('Error in LoggingService.getStackTrace:\n');
      console.error(error);
    }
    if (
      !this.isNullOrUndefined(mOurCallStack) &&
      mOurCallStack.length !== 0
    ) {
      mOurCallStack.forEach((element) => {
        mRetVal += element.caller + ' => ';
      });
    }
    return mRetVal;
  }

  /**
   * Get the total number of records from the given data array.
   *
   * @param {Array<ITotalRecords>} data - the array of records
   * @return {number} the total number of records, or -1 if the data is empty or the 'TotalRecords' field is missing
   */
  public getTotalRecords(data: Array<ITotalRecords>): number {
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
    return mRetVal;
  }

  /**
   * @description Remove an object from a hierarchy of data
   *
   * @param {Array<any>} data - the tree to be searched for a record to delete
   * @param {string | number} searchValue - the value of the property to be searched for
   * @param {string} nameOfProperty - name of the property used to search for the value
   * @param {string} nameOfChildNodes - name of the property which contains the child nodes (default = 'children')
   * @param {any} replacementObject - the replacement object for the match
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public hierarchyRemoveItem(data: any[], searchValue: string | number, nameOfProperty: string, nameOfChildNodes: string = 'children'): boolean {
    let mRetVal: boolean = false;
    if (data && data.length > 0) {
      for (let i = 0; i < data.length; i++) {
        const mItem = data[i];
        const mPropValue = mItem[nameOfProperty];
        if (mPropValue === searchValue) {
          mRetVal = true;
          data.splice(i, 1);
          break;
        } else if (mItem[nameOfChildNodes]) {
          mRetVal = this.hierarchyRemoveItem(mItem[nameOfChildNodes], searchValue, nameOfProperty, nameOfChildNodes);
          if (mRetVal) {
            break;
          }
        }
      }
    }
    return mRetVal;
  }

  /**
   * @description Replaces an object in a hierarchy of data
   *
   * @param {Array<any>} data - the tree to be searched for a record to replace
   * @param {string | number} searchValue - the value of the property to be searched for
   * @param {string} nameOfProperty - name of the property used to search for the value
   * @param {string} nameOfChildNodes - name of the property which contains the child nodes (default = 'children')
   * @param {any} replacementObject - the replacement object for the matching item
   * @return {boolean} true if the data was found and replaced, false otherwise
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public hierarchyReplaceItem(data: Array<any>, searchValue: string | number, nameOfProperty: string, nameOfChildNodes: string, replacementObject: any): boolean {
    let mRetVal = false;
    for (const mItem of data) {
      if (mItem[nameOfProperty] === searchValue) {
        mRetVal = true;
        Object.assign(mItem, replacementObject);
        break;
      }
      if (mItem[nameOfChildNodes]) {
        mRetVal = this.hierarchyReplaceItem(mItem[nameOfChildNodes], searchValue, nameOfProperty, nameOfChildNodes, replacementObject);
      }
    }
    return mRetVal;
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
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public hierarchySearch(data: any, searchValue: string | number, nameOfProperty: string, nameOfChildNodes: string = 'children'): object | null {
    if (Array.isArray(data)) { // if entry object is array objects, check each object
      for (let i = 0; i < data.length; i++) {
        const mNode = this.hierarchySearch(data[i], searchValue, nameOfProperty, nameOfChildNodes);
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
    }
    return null; // node does not match and did not find it in any of the children
  }

  /**
   * Determines if the obj is a function
   *
   * @static
   * @param {unknown} obj - the input to check
   * @return {boolean} true if the input is a function, false otherwise
   * @memberof GWCommon
   */
  public isFunction(obj: unknown): boolean {
    return typeof obj === 'function';
  }

  /**
   * Determines if the input string is null, undefined, empty, or just whitespace
   *
   * @param {string} str - the input string to check
   * @return {boolean} true if the input string is null, undefined, empty, or just whitespace, false otherwise
   * @memberof GWCommon
   */
  public isNullOrEmpty(str: string): boolean {
    if (this.isNullOrUndefined(str)) {
      return true;
    }
    if (0 === str.length || !str || /^\s*$/.test(str)) {
      return true;
    }
    return false;
  }

  /**
   * Determines if the obj is null or undefined
   *
   * @static
   * @param {unknown} obj - the input to check
   * @return {boolean} true if the input is null or or typeof undefined, false otherwise
   * @memberof GWCommon
   */
  public isNullOrUndefined(obj: unknown): boolean {
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
    if (!this.isNullOrEmpty(value.toString()) &&
      !isNaN(Number(value.toString()))) {
      mRetVal = true;
    }
    return mRetVal;
  }

  /**
   * Check if the input value is a string.
   *
   * @param {unknown} value - the value to be checked
   * @return {boolean} true if the value is a string, false otherwise
   */
  public isString(value: unknown): boolean {
    let mRetVal: boolean = false;
    if (typeof value === 'string' || value instanceof String) {
      mRetVal = true;
    }
    return mRetVal;
  }

  /*
   * Natural Sort algorithm for Javascript - Version 0.6 - Released under MIT license
   * Author: Jim Palmer (based on chunking idea from Dave Koelle)
   * Contributors: Mike Grier (mgrier.com), Clint Priest, Kyle Adams, guillermo
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public static naturalSort(a: any, b: any): number {
    const re: RegExp = /(^-?[0-9]+(\.?[0-9]*)[df]?e?[0-9]?$|^0x[0-9a-f]+$|[0-9]+)/gi;
    const sre: RegExp = /(^[ ]*|[ ]*$)/g;
    // eslint-disable-next-line no-useless-escape
    const dre: RegExp = /(^([\w ]+,?[\w ]+)?[\w ]+,?[\w ]+\d+:\d+(:\d+)?[\w ]?|^\d{1,4}[\/\-]\d{1,4}[\/\-]\d{1,4}|^\w+, \w+ \d+, \d{4})/;
    const hre: RegExp = /^0x[0-9a-f]+$/i;
    const ore: RegExp = /^0/;
    // convert all to strings and trim()
    const x = a.toString().replace(sre, '') || '';
    const y = b.toString().replace(sre, '') || '';
    // chunk/tokenize
    const xN = x.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0');
    const yN = y.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0');
    // numeric, hex or date detection
    const xD = parseInt(x.match(hre)) || (xN.length != 1 && x.match(dre) && Date.parse(x));
    const yD = parseInt(y.match(hre)) || xD && y.match(dre) && Date.parse(y) || null;
    // first try and sort Hex codes or Dates
    if (yD)
      if (xD < yD) return -1;
      else if (xD > yD) return 1;
    // natural sorting through split numeric strings and default strings
    for (let cLoc = 0, numS = Math.max(xN.length, yN.length); cLoc < numS; cLoc++) {
      // find floats not starting with '0', string or 0 if not defined (Clint Priest)
      let oFxNcL = !(xN[cLoc] || '').match(ore) && parseFloat(xN[cLoc]) || xN[cLoc] || 0;
      let oFyNcL = !(yN[cLoc] || '').match(ore) && parseFloat(yN[cLoc]) || yN[cLoc] || 0;
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
   * @param ms
   * 
   * @memberof UtilityService
   */
  public async sleep(ms: number) {
    await new Promise(resolve => setTimeout(() => resolve(true), ms)).then(() => {/* do nothing */ });
  }

  /**
   * @description sortData is a basic sort for an array.
   *
   * @requires #sortBy a basic comparer function
   *
   * @param dataArray this is the array to be sorted
   * @param columnName this is the name of the column or element in the array
   * @param orderByDirection this is the desired direction valid options asc or desc
   *
   * @returns a sorted array of object given a object property
   *
   * @usage:
   * var myArray = [
   *  {'First': '', 'Last': '', 'Middle': ''},
   *  {'First': '', 'Last': '', 'Middle': ''}
   * ]; // of course your array will have many "rows" not just two
   * myArray = svc.SortArray(myArray, 'First', 'asc'); // sort your array by the element "First"
   * myArray = svc.SortArray(myArray, 'Last', 'asc'); // sort your array by the element "Last"
   *
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public static sortArray(dataArray: any, columnName: string, orderByDirection: string) {
    const isArray = (Object.prototype.toString.call(dataArray) === '[object Array]');
    if (!isArray) {
      console.log('Common.sortArray: dataArray is not an "Array"!');
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
  private static sortBy(key: string | number, reverse: boolean): (a: Record<string, unknown>, b: Record<string, unknown>) => number {
    // Move smaller items towards the front
    // or back of the array depending on if
    // we want to sort the array in reverse
    // order or not.
    const moveSmaller: number = reverse ? 1 : -1;

    // Move larger items towards the front
    // or back of the array depending on if
    // we want to sort the array in reverse
    // order or not.
    const moveLarger: number = reverse ? -1 : 1;

    /**
     * @param  {*} a
     * @param  {*} b
     * @return {Number}
     */
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    return (a: Record<string, any>, b: Record<string, any>): number => {
      if (a[key] < b[key]) {
        return moveSmaller;
      }
      if (a[key] > b[key]) {
        return moveLarger;
      }
      return 0;
    };
  }

  /**
   * Splits an array into multiple arrays of a specified size.
   *
   * @param {T[]} arrayToSplit - The array to be split.
   * @param {number} numberInSubArray - The number of elements in each subarray.
   * @return {T[][]} The resulting array of subarrays.
   * 
   * @example: var myArrray = splitArray([1,2,3,4,5,6,7,8], 3);
   *          Outputs  [ [1,2,3] , [4,5,6] ,[7,8] ]
   */
  public splitArray<T>(arrayToSplit: T[], numberInSubArray: number): T[][] {
    if (numberInSubArray <= 0) {
      throw new Error('numberInSubArray must be greater than 0');
    }

    const result: T[][] = [];
    for (let i = 0; i < arrayToSplit.length; i += numberInSubArray) {
      result.push(arrayToSplit.slice(i, i + numberInSubArray));
    }

    return result;
  }

  /**
   * Formats the UTC offset as ±HH:MM.  For example, if the new Date().getTimezoneOffset() is 240, this method returns "-04:00".
   *
   * @param {number} timezoneOffset - description of parameter
   * @return {string} description of return value
   */
  public timezoneOffset_UtcOffset(timezoneOffset: number): string {
    const hours = Math.abs(Math.floor(timezoneOffset / 60));
    const minutes = Math.abs(timezoneOffset % 60);
    const sign = timezoneOffset < 0 ? '+' : '-';

    // Format the UTC offset as ±HH:MM
    return `${sign}${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
  }


  /**
   * Waits for a download to complete by attaching a click event listener to the specified link and resolving the promise when the download is detected as complete.
   *
   * @param {HTMLAnchorElement} link - The anchor element that initiates the download.
   * @param {number} [delay=500] - The delay in milliseconds for checking the download status and timeout duration.
   * @return {Promise<void>} A promise that resolves when the download is detected as complete or after a timeout.
   */
  public waitForDownload(link: HTMLAnchorElement, delay: number = 500): Promise<void> {
    return new Promise((resolve) => {
      const mDelay = delay;
      // Create a timeout to prevent infinite waiting
      const mTimeout = setTimeout(() => {
        resolve();
      }, mDelay); // 5 seconds

      // Listen for click event to detect download start
      link.addEventListener('click', () => {
        // Create a timer to check if download is complete
        const checkDownload = setInterval(() => {
          // Check if download is complete
          if (('webkitHidden' in document && document.webkitHidden) || ('msHidden' in document && document.msHidden) || document.hidden) {
            // Download is complete
            clearTimeout(mTimeout);
            clearInterval(checkDownload);
            resolve();
          }
        }, mDelay); // Check every second
      });
    });
  }
}
