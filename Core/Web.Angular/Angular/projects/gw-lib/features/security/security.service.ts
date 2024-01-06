import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { ISecurityInfo } from './security-info.model';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {

  private _BaseURL: string = '';
  private _Api:string = '';
  private _Api_GUID: string = '';
  private _Api_RandomNumbers: string = '';
  private _Api_Security: string = '';

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
  ) { 
    this._BaseURL = this._GWCommon.baseURL;
    this._Api = this._BaseURL + 'GrowthwareAPI/';
    this._Api_GUID = this._Api + 'GetGUID';
    this._Api_RandomNumbers = this._Api + 'GetRandomNumbers';
    this._Api_Security = this._Api + 'GetSecurityInfo';
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
    const mUrl = this._Api_Security;
    return new Promise<ISecurityInfo>((resolve, reject) => {
      this._HttpClient.get<ISecurityInfo>(mUrl, mHttpOptions).subscribe({
        next: (response: ISecurityInfo) => {
          resolve(response);
        },
        error: (error) => {
          this._LoggingSvc.errorHandler(error, 'SecurityService', 'getSecurityInfo');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  /**
   * @description Returns a GUID
   *
   * @return {*}  {Promise<string>}
   * @memberof SecurityService
   */
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
        error: (error)=>{
          this._LoggingSvc.errorHandler(error, 'SecurityService', 'getGuid');
          reject('Failed to call the API');          
        },
        complete: ()=>{
          // doing nothing atm
        }
      });
    })
  }

  public async getRandomNumbers(amountOfNumbers: number, maxNumber: number, minNumber: number): Promise<number[]> {
    const mQueryParameter: HttpParams = new HttpParams()
      .set('amountOfNumbers', amountOfNumbers)
      .set('maxNumber', maxNumber)
      .set('minNumber', minNumber);

    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    
    return new Promise<number[]>((resolve, reject) => {
      this._HttpClient.get<number[]>(this._Api_RandomNumbers, mHttpOptions).subscribe({
        next: (response: number[])=>{
          resolve(response);
        },
        error: (error)=>{
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
