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
  private _Api_Save: string = '';

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
    this._Api_Save = this._GWCommon.baseURL + this._ApiName + 'SaveRole';
  }

  /**
   * Retrieves the role profile for editing based on the given role sequence ID.
   *
   * @param {number} roleSeqId - The sequence ID of the role to retrieve.
   * @return {Promise<IRoleProfile>} A promise that resolves with the role profile.
   */
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

  /**
   * Retrieves the roles from the server.
   *
   * @return {Promise<any>} A promise that resolves with the roles data.
   */
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

  /**
   * Saves the profile information.
   *
   * @param {IRoleProfile} profile - The profile to be saved.
   * @return {Promise<any>} A promise that resolves with the result of saving the profile.
   */
  public async save(profile: IRoleProfile): Promise<any> {
    return new Promise<boolean>((resolve, reject) => {
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
      };
      this._HttpClient.post<boolean>(this._Api_Save, profile, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'RoleService', 'save');
          reject(false);
        },
        // complete: () => {}
      });
    });
  }

}
