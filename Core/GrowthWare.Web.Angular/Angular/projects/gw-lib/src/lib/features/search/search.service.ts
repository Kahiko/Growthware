import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Common } from '../../common';

export class SearchCriteria {
  constructor(
    public columns: string,
    public orderByColumn: string,
    public orderByDirection: string,
    public pageSize: number,
    public selectedPage: number,
    public tableOrView: string,
    public whereClause: string
  ) {}
}

@Injectable({
  providedIn: 'root',
})
export class GWLibSearchService {
  private _HttpClient: HttpClient;
  private _SearchUrl: string = Common.baseURL + 'GrowthwareAPI/Search';

  constructor(httpClient: HttpClient) {
    this._HttpClient = httpClient;
  }

  /**
   * Handles an HttpClient error
   *
   * @private
   * @param {HttpErrorResponse} errorResponse
   * @param {string} methodName
   * @memberof SearchService
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
    console.log('SearchService.' + methodName + ': ');
    console.log(errorMessage);
  }

  /**
   * Calls GrowthWareAPI.Search
   *
   * @param {SearchCriteria} criteria
   * @return {*}  {Promise<any>}
   * @memberof SearchService
   */
  public async getResults(criteria: SearchCriteria): Promise<any> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };

    return new Promise<any>((resolve, reject) => {
      this._HttpClient
        .post<any>(this._SearchUrl, criteria, mHttpOptions)
        .subscribe({
          next: (response) => resolve(response),
          error: (errorResponse) => {
            this.errorHandler(errorResponse, 'getResults');
            reject(errorResponse);
          },
          complete: () => console.info('complete')
        });
    });
  }
}
