import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// This Feature
import { IFunctionProfile } from './function-profile.model';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';

@Injectable({
  providedIn: 'root'
})
export class FunctionService {

  private _FunctionSeqId: number = -1;
  private _ApiName: string = 'GrowthwareFunction/';
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
    private _LoggingSvc: LoggingService,
  ) {
    this._Api_GetFunction = this._GWCommon.baseURL + this._ApiName + 'GetFunction';
  }


  /**
   * Gets a FunctionProfile given the functionSeqId
   *
   * @param {number} functionSeqId
   * @return {*}  {Promise<IFunctionProfile>}
   * @memberof FunctionService
   */
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
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }
}
