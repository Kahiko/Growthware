import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { GWCommon } from '../common';

export class SearchCriteria {
  public tableOrView: string

  constructor(
    public columns: string,
    public orderByColumn: string,
    public orderByDirection: string,
    public pageSize: number,
    public selectedPage: number,
    public whereClause: string
  ) {}
}

@Injectable({
  providedIn: 'root',
})
export class GWLibSearchService {
  private _HttpClient: HttpClient;
  private _SearchUrl: string = GWCommon.baseURL + 'GrowthWareAPI/Search';

  constructor(httpClient: HttpClient) {
    this._HttpClient = httpClient;
  }

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
}
