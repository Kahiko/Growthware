import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { AccountService } from '@Growthware/features/account';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
import { SearchService } from '@Growthware/features/search';
// Feature
import { IFunctionProfile } from './function-profile.model';

@Injectable({
  providedIn: 'root'
})
export class FunctionService {

  private _FunctionSeqId: number = -1;
  private _ApiName: string = 'GrowthwareFunction/';
  private _Api_AvalibleParents: string = '';
  private _Api_CopyFunctionSecurity: string = '';
  private _Api_Delete: string = '';
  private _Api_GetFunction: string = '';
  private _Api_GetFunctionOrder: string = '';
  private _Api_FunctionTypes: string = '';
  private _Api_LinkBehaviors: string = '';
  private _Api_NavigationTypes: string = '';
  private _Api_Save: string = '';
  private _Reason: string = '';

  modalReason: string = '';
  selectedRow: any = {};

  public get functionSeqId(): number {
    return this._FunctionSeqId;
  }
  public set functionSeqId(value: number) {
    this._FunctionSeqId = value;
  }
  
  public get reason(): string {
    return this._Reason;
  }
  public set reason(value: string) {
    this._Reason = value;
  }

  readonly addEditModalId: string = 'addEditAccountModal';

  constructor(
    private _AccountSvc: AccountService,
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private _SearchSvc: SearchService,
  ) {
    this._Api_AvalibleParents = this._GWCommon.baseURL + this._ApiName + 'GetAvalibleParents';
    this._Api_CopyFunctionSecurity = this._GWCommon.baseURL + this._ApiName + 'CopyFunctionSecurity';
    this._Api_Delete = this._GWCommon.baseURL + this._ApiName + 'DeleteFunction';
    this._Api_GetFunction = this._GWCommon.baseURL + this._ApiName + 'GetFunction';
    this._Api_GetFunctionOrder = this._GWCommon.baseURL + this._ApiName + 'GetFunctionOrder';
    this._Api_FunctionTypes = this._GWCommon.baseURL + this._ApiName + 'GetFunctionTypes';
    this._Api_NavigationTypes = this._GWCommon.baseURL + this._ApiName + 'GetNavigationTypes';
    this._Api_LinkBehaviors = this._GWCommon.baseURL + this._ApiName + 'GetLinkBehaviors';
    this._Api_Save = this._GWCommon.baseURL + this._ApiName + 'Save';
  }

  public async copyFunctionSecurity(source: number, target: number): Promise<boolean> {
    const mQueryParameter: HttpParams = new HttpParams().append('source', source).append('target', target);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.post<any>(this._Api_CopyFunctionSecurity, null, mHttpOptions).subscribe({
        next: (response: boolean) => {
          this._AccountSvc.triggerMenuUpdate();
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'copyFunctionSecurity');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async deleteFunction(functionSeqId: number): Promise<boolean> {
    const mQueryParameter: HttpParams = new HttpParams().append('functionSeqId', functionSeqId);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.delete<any>(this._Api_Delete, mHttpOptions).subscribe({
        next: (response: boolean) => {
          this._AccountSvc.triggerMenuUpdate();
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'deleteFunction');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async getAvalibleParents(): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.get<any>(this._Api_AvalibleParents).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'getAvalibleParents');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });    
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

  public async getFunctionOrder(functionSeqId: number): Promise<any> {
    const mQueryParameter: HttpParams = new HttpParams().append('functionSeqId', functionSeqId);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.get<any>(this._Api_GetFunctionOrder, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFunctionOrder');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  /**
   * Retrieves the function types from the API.
   *
   * @return {Promise<any>} A promise that resolves with the response from the API.
   */
  public async getFuncitonTypes(): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.get<any>(this._Api_FunctionTypes).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFuncitonTypes');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async getLinkBehaviors(): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.get<any>(this._Api_LinkBehaviors).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFuncitonTypes');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async getNavigationTypes(): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.get<any>(this._Api_NavigationTypes).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFuncitonTypes');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async save(functionProfile: IFunctionProfile): Promise<boolean> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    return new Promise<any>((resolve, reject) => {
      this._HttpClient.post<boolean>(this._Api_Save, functionProfile, mHttpOptions).subscribe({
        next: (response: any) => {
          var mSearchCriteria = this._SearchSvc.getSearchCriteria("Functions"); // from SearchFunctionsComponent (this.configurationName)
          if(mSearchCriteria != null) {
            this._SearchSvc.setSearchCriteria("Functions", mSearchCriteria);
          }
          this._AccountSvc.triggerMenuUpdate();
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'save');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }
}
