import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { SearchCriteria } from '@Growthware/Lib/src/lib/models';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private _Criteria: Map<string, SearchCriteria> = new Map<string, SearchCriteria>();
  private _SearchUrl: string = this._GWCommon.baseURL + 'GrowthWareAPI/Search';

  public searchCriteriaChanged = new Subject<string>();

  constructor(private _GWCommon: GWCommon, private _HttpClient: HttpClient) { }

  /**
   * Handles an HttpClient error
   *
   * @private
   * @param {HttpErrorResponse} errorResponse
   * @param {string} methodName
   * @memberof GWLibSearchService
   */
  private errorHandler(errorResponse: HttpErrorResponse, methodName: string) {
    let errorMessage = '';
    if (errorResponse.error instanceof ErrorEvent) {
        // Get client-side error
        errorMessage = errorResponse.error.message;
    } else {
        // Get server-side error
        errorMessage = `Error Code: ${errorResponse.status}\nMessage: ${errorResponse.message}`;
    }
    console.log(`GWLibSearchService.${methodName}:`);
    console.log(errorMessage);
  }

  /**
   * Calls GrowthWareAPI.Search
   *
   * @param {SearchCriteria} searchCriteria
   * @return {*}  {Promise<any>}
   * @memberof GWLibSearchService
   */
  public async getResults(searchCriteria: SearchCriteria): Promise<any> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };

    return new Promise<any>((resolve, reject) => {
      this._HttpClient
        .post<any>(this._SearchUrl, searchCriteria, mHttpOptions)
        .subscribe({
          next: (response) => {
            resolve(response)
          },
          error: (errorResponse) => {
            this.errorHandler(errorResponse, 'getResults');
            reject(errorResponse);
          },
          // complete: () => console.info('complete')
        });
    });
  }

  /**
   * Returns a SearchCriteria object given the name or new SearchCriteria('','','',1,1,'')
   *
   * @param {string} name
   * @return {*}  {SearchCriteria}
   * @memberof DynamicTableService
   */
  public getSearchCriteria(name: string): SearchCriteria {
    const mRetVal: SearchCriteria = this._Criteria.get(name.trim().toLowerCase()) || new SearchCriteria('','','',1,1,'1=1');
    return mRetVal;
  }

  /**
   * Returns a SearchCriteria given the name
   *
   * @param {string} name
   * @param {SearchCriteria} searchCriteria
   * @memberof SearchService
   */
  public setSearchCriteria(name: string, searchCriteria: SearchCriteria): void {
    if(!this._GWCommon.isNullorEmpty(name) && !this._GWCommon.isNullOrUndefined(searchCriteria)) {
      this._Criteria.set(name.trim().toLowerCase(), searchCriteria);
      this.searchCriteriaChanged.next(name);
    } else {
      throw('name and/or searchCriteria can not be null, undefined or empty');
    }
  }
}
