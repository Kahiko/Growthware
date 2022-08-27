import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

import { IFunctionProfile } from './function-profile.model';

@Injectable({
  providedIn: 'root'
})
export class FunctionService {

  private _FunctionSeqId: number = -1;
  private _ApiName: string = 'GrowthwareAPI/';
  private _Api_GetFunction: string = '';
  private _Reason: string = '';

  public get functionSeqId(): number {
    return this._FunctionSeqId;
  }
  public set functionSeqId(value: number) {
    this._FunctionSeqId = value;
  }

  public get addModalId(): string {
    return 'addFunction'
  }

  public get editModalId(): string {
    return 'editFunction'
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
    this._Api_GetFunction = this._GWCommon.baseURL + this._ApiName + 'GetFunction';
  }


  public async getFunction(functionSeqId: number): Promise<IFunctionProfile> {
    const mQueryParameter: HttpParams = new HttpParams().append('functionSeqId', functionSeqId);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<IFunctionProfile>((resolve, reject) => {
      this._HttpClient.get<IFunctionProfile>(this._Api_GetFunction, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this.errorHandler(error, 'getFunction');
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
    this._LoggingSvc.console(`FunctionService.${methodName}:`, LogLevel.Error);
    this._LoggingSvc.console(errorMessage, LogLevel.Error);
  }
}
