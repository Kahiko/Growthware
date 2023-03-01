import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ISecurityInfo } from '@Growthware/Lib/src/lib/models';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {

  private _BaseURL: string = '';
  private _APISecurity: string = '';

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
  ) { 
    this._BaseURL = this._GWCommon.baseURL;
    this._APISecurity = this._BaseURL + 'GrowthwareAPI/';
  }

  public async getSecurityInfo(action: string): Promise<ISecurityInfo> {
    const mQueryParameter: HttpParams = new HttpParams().append('action', action);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    const mUrl = this._BaseURL + this._APISecurity + 'GetSecurityInfo';
    return new Promise<ISecurityInfo>((resolve, reject) => {
      this._HttpClient.get<ISecurityInfo>(mUrl, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'AccountService', 'getAccount');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }
}
