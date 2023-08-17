import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// This Feature
import { SearchCriteria, SearchCriteriaNVP, SearchResultsNVP } from './search-criteria.model';

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
