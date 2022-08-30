import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { INavLink } from '@Growthware/Lib/src/lib/features/navigation';

import { IAccountProfile } from './account-profile.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private _Account: string = '';
  private _ApiName: string = 'GrowthwareAPI/';
  private _Api_GetAccount: string = '';
  private _Api_GetLinks: string = '';
  private _DefaultAccount: string = 'Anonymous'
  private _Reason: string = '';

  public get account(): string {
    return this._Account;
  }
  public set account(value: string) {
    this._Account = value;
  }

  public get addModalId(): string {
    return 'addAccount'
  }

  public get editModalId(): string {
    return 'editAccount'
  }

  public get loginModalId(): string {
    return 'login';
  }

  public get reason(): string {
    return this._Reason;
  }
  public set reason(value: string) {
    this._Reason = value;
  }

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService
  ) {
    this._Api_GetAccount = this._GWCommon.baseURL + this._ApiName + 'GetAccount';
    this._Api_GetLinks = this._GWCommon.baseURL + this._ApiName + 'GetLinks';
  }

  public async authenticate(account: string, password: string): Promise<boolean | string> {
    return new Promise<boolean>((resolve, reject) => {
      if(this._GWCommon.isNullOrEmpty(account)) {
        throw new Error("account can not be blank!");
      }
      if(this._GWCommon.isNullOrEmpty(password)) {
        throw new Error("password can not be blank!");
      }
      let mQueryParameter: HttpParams = new HttpParams().append('account', account);
      mQueryParameter = new HttpParams().append('account', account)
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };

      resolve(true);
    });
  }

  public async getAccount(account: string): Promise<IAccountProfile> {
    let mAccount: string = account;
    if(this._GWCommon.isNullOrEmpty(mAccount)) {
      mAccount = this._DefaultAccount;
    }
    const mQueryParameter: HttpParams = new HttpParams().append('account', mAccount);
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
   * Gets an array if NavLinks from the API
   *
   * @return {*}  {Promise<INavLink[]>}
   * @memberof AccountService
   * TODO: this really should be used to populate an observable property
   * so that the links can change when say the account changes.
   */
  public async getNavLinks(): Promise<INavLink[]> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    return new Promise<INavLink[]>((resolve, reject) => {
      this._HttpClient.get<INavLink[]>(this._Api_GetLinks, mHttpOptions).subscribe({
        next: (response) => {
          resolve(response);
        },
        error: (error) => {
          this.errorHandler(error, 'getNavLinks');
          reject('Was not able to retrieve the navigation links');
        },
        complete: () => {
          // here as example
        }
      })
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
