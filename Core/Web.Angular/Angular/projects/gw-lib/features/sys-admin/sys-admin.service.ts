import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
// Library
import { LoggingService } from '@Growthware/features/logging';
import { GWCommon } from '@Growthware/common-code';
// Feature
import { LineCount } from './line-count.model';

@Injectable({
  providedIn: 'root'
})
export class SysAdminService {
  private _ApiName: string = 'GrowthwareFile/';
  private _ApiLineCount: string = '';

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,    
  ) { 
    this._ApiLineCount = this._GWCommon.baseURL + this._ApiName + 'GetLineCount';
  }

  getLineCount(lineCount: LineCount): Promise<string> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      responseType: "text" as "json",
    };
    return new Promise<string>((resolve, reject) => {
      this._HttpClient
      .post<any>(this._ApiLineCount, lineCount, mHttpOptions)
      .subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (errorResponse: any) => {
          this._LoggingSvc.errorHandler(errorResponse, 'SysAdminService', 'getLineCount');
          reject(errorResponse);
        },
        // complete: () => console.info('complete')
      });
    });
  }
}
