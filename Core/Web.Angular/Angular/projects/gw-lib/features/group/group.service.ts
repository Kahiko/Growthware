import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private _ApiName: string = 'GrowthwareGroup/';

  public get addModalId(): string {
    return 'addAccount'
  }

  public get editModalId(): string {
    return 'editAccount'
  }

  editAccount: string = '';
  editReason: string = '';

  constructor(private _GWCommon: GWCommon, private _HttpClient: HttpClient, private _LoggingSvc: LoggingService) { }

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
}
