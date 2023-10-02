import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { IValidSecurityEntity } from './valid-security-entity.model';

@Injectable({
  providedIn: 'root'
})
export class SecurityEntityService {

  private _ApiName: string = 'GrowthwareSecurityEntity/';
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
