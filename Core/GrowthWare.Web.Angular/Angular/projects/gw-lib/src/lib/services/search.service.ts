import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { IDynamicTableColumn, SearchCriteria, SearchCriteriaNVP, SearchResultsNVP } from '@Growthware/Lib/src/lib/models';
import { DynamicTableService } from './dynamic-table.service';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private _SearchUrl: string = this._GWCommon.baseURL + 'GrowthWareAPI/Search';

  public searchCriteriaChanged = new Subject<SearchCriteriaNVP>();

  constructor(
    private _DynamicTableSvc: DynamicTableService,
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
  public async getResults(criteria: SearchCriteriaNVP): Promise<any> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return new Promise<SearchResultsNVP>((resolve, reject) => {
      this._HttpClient
      .post<any>(this._SearchUrl, criteria.payLoad, mHttpOptions)
      .subscribe({
        next: (response) => {
          const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(criteria.name, { searchCriteria: criteria.payLoad, data: response });
          resolve(mSearchResultsNVP);
        },
        error: (errorResponse) => {
          this.errorHandler(errorResponse, 'getResults');
          reject(errorResponse);
        },
        // complete: () => console.info('complete')
      });
    });
  }

  public getSearchCriteriaFromConfig(name: string): SearchCriteriaNVP {
    const mSearchCriteria: SearchCriteria = new SearchCriteria('','','',1,1,'1=1');
    const mTableConfiguration = this._DynamicTableSvc.getTableConfiguration(name);
    const mRetVal: SearchCriteriaNVP = new SearchCriteriaNVP(name, mSearchCriteria);
    if(this._GWCommon.isNullOrUndefined(mTableConfiguration)) {
      throw new Error(`Could not find the "${name}" configuration!`);
    }
    let mColumns: string = '';
    mTableConfiguration.columns.forEach((column: IDynamicTableColumn) => {
      mColumns += column.name + ', ';
    });
    mColumns = mColumns.substring(0, mColumns.length -2);
    mSearchCriteria.columns = mColumns;
    mSearchCriteria.orderByColumn = mTableConfiguration.orderByColumn;
    mSearchCriteria.orderByDirection = 'asc'
    mSearchCriteria.pageSize = mTableConfiguration.numberOfRows;
    mSearchCriteria.selectedPage = 1;
    mSearchCriteria.tableOrView = mTableConfiguration.tableOrView;
    mRetVal.payLoad = mSearchCriteria;
    return mRetVal;
  }

  public setSearchCriteria(newCriteria: SearchCriteriaNVP) {
    this.searchCriteriaChanged.next(newCriteria);
  }
}
