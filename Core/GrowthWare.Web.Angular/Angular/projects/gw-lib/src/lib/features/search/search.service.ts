import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { IDynamicTableConfiguration } from '@Growthware/Lib/src/lib/models';
import { LoggingService } from '@Growthware/Lib/src/lib/features/logging';
// This Feature
import { SearchCriteria, ISearchCriteriaNVP, SearchCriteriaNVP, SearchResultsNVP } from './search-criteria.model';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private _BaseUrl: string = '';
  private _SearchCriteria_NVP_Array: SearchCriteriaNVP[] = [];

  public searchCriteriaChanged = new Subject<SearchCriteriaNVP>();

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
  ) {
    this._BaseUrl = this._GWCommon.baseURL;
  }

  /**
   * Calls GrowthwareAPI.Search
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
          this._LoggingSvc.errorHandler(errorResponse, 'SearchService', 'getResults');
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

  public getSearchCriteria(name: string): null | SearchCriteria {
    const index = this._SearchCriteria_NVP_Array.findIndex(searchCriteriaNVP=> searchCriteriaNVP.name === name);
    let mRetVal = null;
    if(index > -1) {
      mRetVal = this._SearchCriteria_NVP_Array[index].payLoad;
    }
    return mRetVal;
  }

  /**
   * Calls GrowthwareAPI.SearchAccounts
   *
   * @param {SearchCriteriaNVP} criteria
   * @return {*}  {Promise<any>}
   * @memberof SearchService
   */
  public async searchAccounts(criteria: SearchCriteriaNVP): Promise<any> {
    const mUrl = this._BaseUrl + 'GrowthwareAccount/SearchAccounts';
    return this.getResults(mUrl, criteria);
  }

  /**
   * Calls GrowthwareAPI.SearchFunctions
   *
   * @param {SearchCriteriaNVP} criteria
   * @return {*}  {Promise<any>}
   * @memberof SearchService
   */
   public async searchFunctions(criteria: SearchCriteriaNVP): Promise<any> {
    const mUrl = this._BaseUrl + 'GrowthwareFunction/SearchFunctions';
    return this.getResults(mUrl, criteria);
  }

  public setSearchCriteria(name: string, searchCriteria: SearchCriteria) {
    const mChangedCriteria = new SearchCriteriaNVP( name, searchCriteria );
    this._GWCommon.addOrUpdateArray(this._SearchCriteria_NVP_Array, mChangedCriteria);
    this.searchCriteriaChanged.next(mChangedCriteria);
  }
}
