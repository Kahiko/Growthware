import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { IValidSecurityEntity } from './valid-security-entity.model';
import { ISecurityEntityProfile } from './security-entity-profile.model';

@Injectable({
  providedIn: 'root'
})
export class SecurityEntityService {

  private _ApiName: string = 'GrowthwareSecurityEntity/';
  private _Api_GetSecurityEntity: string = '';
  private _Api_GetValidSecurityEntities: string = '';  
  
  public get addModalId(): string {
    return 'addSecurityEntity'
  }

  public get editModalId(): string {
    return 'editSecurityEntity'
  }

  editReason: string = '';
  editRow: any = {};

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
  ) { 
    this._Api_GetValidSecurityEntities = this._GWCommon.baseURL + this._ApiName + 'GetValidSecurityEntities'
    this._Api_GetSecurityEntity = this._GWCommon.baseURL + this._ApiName + 'GetProfile';
  }

  public async getSecurityEntity(id: number): Promise<ISecurityEntityProfile> {
    const mQueryParameter: HttpParams = new HttpParams()
      .set('securityEntitySeqId', id);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<ISecurityEntityProfile>((resolve, reject) => {
      this._HttpClient.get<ISecurityEntityProfile>(this._Api_GetSecurityEntity, mHttpOptions).subscribe({
        next: (response: ISecurityEntityProfile) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'getSecurityEntity');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async getValidSecurityEntities(): Promise<IValidSecurityEntity[]> {
    return new Promise<IValidSecurityEntity[]>((resolve, reject) => {
      this._HttpClient.get<IValidSecurityEntity[]>(this._Api_GetValidSecurityEntities).subscribe({
        next: (response: IValidSecurityEntity[]) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'getValidSecurityEntities');
        },
        // complete: () => {}
      });
    });
  }
}
