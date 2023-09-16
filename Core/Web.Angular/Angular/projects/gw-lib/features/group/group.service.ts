import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { IGroupProfile } from './group-profile.model';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private _ApiName: string = 'GrowthwareGroup/';
  private _Api_GetGroupForEdit = '';
  private _Api_SaveGroup = '';

  public get addModalId(): string {
    return 'addAccount'
  }

  public get editModalId(): string {
    return 'editAccount'
  }

  editRow: any = {};
  editReason: string = '';

  constructor(private _GWCommon: GWCommon, private _HttpClient: HttpClient, private _LoggingSvc: LoggingService) { 
    this._Api_GetGroupForEdit = this._GWCommon.baseURL + this._ApiName + 'GetGroupForEdit/';
    this._Api_SaveGroup = this._GWCommon.baseURL + this._ApiName + 'SaveGroup/';
  }

  public async getGroupForEdit(groupSeqId: number): Promise<IGroupProfile> {
    console.log('groupSeqId', groupSeqId);
    return new Promise<IGroupProfile>((resolve, reject) => {
      const mQueryParameter: HttpParams = new HttpParams()
        .set('groupSeqId', groupSeqId);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.get<IGroupProfile>(this._Api_GetGroupForEdit, mHttpOptions).subscribe({
        next: (response: IGroupProfile) => {
          console.log('getGroupForEdit.GetGroupForEdit.response', response);
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'GroupService', 'getGroupForEdit');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    })
  }

  public async getGroups(): Promise<any> {
    return new Promise<boolean>((resolve, reject) => {
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
      };
      const mUrl = this._GWCommon.baseURL + this._ApiName + 'GetGroups';
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

  public async saveGroup(profile : IGroupProfile): Promise<boolean> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    return new Promise<boolean>((resolve, reject) => {
      this._HttpClient.post<boolean>(this._Api_SaveGroup, profile, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'GroupService', 'saveGroup');
          reject(false);
        },
        // complete: () => {}
      });
    })
  }
}
