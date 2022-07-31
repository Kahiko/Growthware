import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { IDynamicTableConfiguration } from '@Growthware/Lib/src/lib/models';
import { SearchCriteria, SearchCriteriaNVP, SearchResultsNVP } from './search-criteria.model';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private _SearchUrl: string = this._GWCommon.baseURL + 'GrowthWareAPI/Search';

  public searchCriteriaChanged = new Subject<SearchCriteriaNVP>();

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient
  ) { }

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
   * @param {SearchCriteria} criteria
   * @return {*}  {Promise<any>}
   * @memberof GWLibSearchService
   */
  private async getResults(url: string, criteria: SearchCriteriaNVP): Promise<any> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return new Promise<SearchResultsNVP>((resolve, reject) => {
      this._HttpClient
      .post<any>(url, criteria.payLoad, mHttpOptions)
      .subscribe({
        next: (response: any) => {
          const mTotalRecords = this._GWCommon.getTotalRecords(response);
          const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(criteria.name, { data: response, totalRecords: mTotalRecords, searchCriteria: criteria.payLoad });
          resolve(mSearchResultsNVP);
        },
        error: (errorResponse: any) => {
          this.errorHandler(errorResponse, 'getResults');
          reject(errorResponse);
        },
        // complete: () => console.info('complete')
      });
    });
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
   * Calls GrowthWareAPI.SearchAccounts
   *
   * @param {SearchCriteriaNVP} criteria
   * @return {*}  {Promise<any>}
   * @memberof SearchService
   */
  public async searchAccounts(criteria: SearchCriteriaNVP): Promise<any> {
    const mUrl = this._GWCommon.baseURL + 'GrowthWareAPI/SearchAccounts';
    return this.getResults(mUrl, criteria);
  }

  /**
   * Calls GrowthWareAPI.SearchFunctions
   *
   * @param {SearchCriteriaNVP} criteria
   * @return {*}  {Promise<any>}
   * @memberof SearchService
   */
   public async searchFunctions(criteria: SearchCriteriaNVP): Promise<any> {
    const mUrl = this._GWCommon.baseURL + 'GrowthWareAPI/SearchFunctions';
    return this.getResults(mUrl, criteria);
  }

  public setSearchCriteria(name: string, searchCriteria: SearchCriteria) {
    const mChangedCriteria = new SearchCriteriaNVP( name, searchCriteria );
    this.searchCriteriaChanged.next(mChangedCriteria);
  }
}
