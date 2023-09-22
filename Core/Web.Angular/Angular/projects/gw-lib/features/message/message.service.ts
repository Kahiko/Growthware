import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { IMessageProfile } from './message-profile.model';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  private _Api_GetProfile: string = '';
  private _ApiName: string = 'GrowthwareMessage/';
  
  public get addModalId(): string {
    return 'addMessage'
  }

  public get editModalId(): string {
    return 'editMessage'
  }

  editRow: any = {};
  editReason: string = '';

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient, 
    private _LoggingSvc: LoggingService
  ) { 
    this._Api_GetProfile = this._GWCommon.baseURL + this._ApiName + 'GetProfile/';
  }

  public async getProfile(id: number): Promise<IMessageProfile> {
    return new Promise<IMessageProfile>((resolve, reject) => {
      const mQueryParameter: HttpParams = new HttpParams()
        .set('id', id);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.get<IMessageProfile>(this._Api_GetProfile, mHttpOptions).subscribe({
        next: (response: IMessageProfile) => {
          console.log('MessageService.getProfile.response', response);
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'MessageService', 'getMessageForEdit');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    })
  }
}
