import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
// Feature
import { ISecurityInfo } from './security-info.model';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {

  private _BaseURL: string = '';
  private _Api:string = '';
  private _Api_GUID: string = '';
  private _ApiSecurity: string = '';

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
  ) { 
    this._BaseURL = this._GWCommon.baseURL;
    this._Api = this._BaseURL + 'GrowthwareAPI/';
    this._Api_GUID = this._Api + 'GetGUID';
    this._ApiSecurity = this._Api + 'GetSecurityInfo';
  }

  /**
   * @description Calls the API to retrive a SecurityInfo for the given action
   *
   * @param {string} action
   * @return {*}  {Promise<ISecurityInfo>}
   * @memberof SecurityService
   */
  public async getSecurityInfo(action: string): Promise<ISecurityInfo> {
    const mQueryParameter: HttpParams = new HttpParams().append('action', action);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    // const mUrl = this._BaseURL + this._APISecurity + 'GetSecurityInfo';
    const mUrl = this._ApiSecurity;
    return new Promise<ISecurityInfo>((resolve, reject) => {
      this._HttpClient.get<ISecurityInfo>(mUrl, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'SecurityService', 'getSecurityInfo');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async getGuid(): Promise<string> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      responseType: "text" as "json",
    };
    const mUrl = this._Api_GUID;
    return new Promise<string>((resolve, reject) => {
      this._HttpClient.get<string>(mUrl, mHttpOptions).subscribe({
        next: (response: string)=>{
          resolve(response);
        },
        error: (error: any)=>{
          this._LoggingSvc.errorHandler(error, 'SecurityService', 'getGuid');
          reject('Failed to call the API');          
        },
        complete: ()=>{
          // doing nothing atm
        }
      });
    })
  }
}
