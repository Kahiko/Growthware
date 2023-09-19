import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { IRoleProfile } from './role-profile.model';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private _ApiName: string = 'GrowthwareRole/';
  private _Api_GetRole: string = '';

  public get addModalId(): string {
    return 'addRole'
  }

  public get editModalId(): string {
    return 'editRole'
  }

  editRow: any = {};
  editReason: string = '';

  constructor(private _GWCommon: GWCommon, private _HttpClient: HttpClient, private _LoggingSvc: LoggingService) { 
    this._Api_GetRole = this._GWCommon.baseURL + this._ApiName + 'GetRoleForEdit';
  }

  public async getRoleForEdit(roleSeqId: number): Promise<IRoleProfile> {
    return new Promise<IRoleProfile>((resolve, reject) => {
      const mQueryParameter: HttpParams = new HttpParams()
        .set('roleSeqId', roleSeqId);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.get<IRoleProfile>(this._Api_GetRole, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'RoleService', 'getRole');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async getRoles(): Promise<any> {
    return new Promise<boolean>((resolve, reject) => {
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
      };
      const mUrl = this._GWCommon.baseURL + this._ApiName + 'GetRoles';
      this._HttpClient.get<any>(mUrl, mHttpOptions).subscribe({
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
