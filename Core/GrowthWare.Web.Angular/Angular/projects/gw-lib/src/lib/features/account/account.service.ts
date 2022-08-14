import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

import { IAccountProfile } from './account-profile.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private _AccountId: number = -1;
  private _ApiName: string = 'GrowthwareAPI/';
  private _Api_GetAccountById: string = '';

  public get accountId(): number {
    return this._AccountId;
  }

  public set accountId(value: number) {
    this._AccountId = value;
  }

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService
    ) {
      this._Api_GetAccountById = this._GWCommon.baseURL + this._ApiName + 'GetAccountById';
    }

  public async getAccountById(accountSeqId: number): Promise<IAccountProfile> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return new Promise<IAccountProfile>((resolve, reject) => {
      this._HttpClient.post<IAccountProfile>(this._Api_GetAccountById, accountSeqId, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this.errorHandler(error, 'getAccountById');
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
