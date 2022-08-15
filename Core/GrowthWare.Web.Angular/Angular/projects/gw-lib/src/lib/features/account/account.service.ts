import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

import { IAccountProfile } from './account-profile.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private _Account: string = '';
  private _ApiName: string = 'GrowthwareAPI/';
  private _Api_GetAccount: string = '';

  public get account(): string {
    return this._Account;
  }

  public set account(value: string) {
    this._Account = value;
  }

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService
    ) {
      this._Api_GetAccount = this._GWCommon.baseURL + this._ApiName + 'GetAccount';
    }

  public async getAccount(account: string): Promise<IAccountProfile> {
    const mQueryParameter: HttpParams = new HttpParams().append('account', account);
    console.log(mQueryParameter);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<IAccountProfile>((resolve, reject) => {
      this._HttpClient.get<IAccountProfile>(this._Api_GetAccount, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this.errorHandler(error, 'getAccount');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
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
    this._LoggingSvc.console(`AccountService.${methodName}:`, LogLevel.Error);
    this._LoggingSvc.console(errorMessage, LogLevel.Error);
  }
}
