import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
// Library
import { GWCommon, LogLevel } from '@Growthware/Lib';
import { LoggingService } from '@Growthware/Lib';
import { IAppSettings } from './app-settings.model';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService implements OnInit {
  private _ApiName: string = 'Growthware/';
  private _ApiURL: string = '';

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService
  ) { }

  ngOnInit(): void {
    this._ApiURL = this._GWCommon.baseURL + this._ApiName;
  }

  public async getAppSettings(): Promise<IAppSettings> {
    const mUrl = this._ApiURL + 'GetAppSettings';
    return new Promise<IAppSettings>((resolve, reject) => {
      this._HttpClient.get<IAppSettings>(mUrl).subscribe({
        next: (response: IAppSettings) => {
          resolve(response);
        },
        error: (errorResponse: any) => {
          this.errorHandler(errorResponse, 'getAppSettings');
          reject('Could not get the application settings');
        },
        complete: () => console.info('complete')
      });
    })
  }

  /**
   * Handles an HttpClient error
   *
   * @private
   * @param {HttpErrorResponse} errorResponse
   * @param {string} methodName
   * @memberof LoggingService
   */
   private errorHandler(errorResponse: HttpErrorResponse, methodName: string) {
    let errorMessage = '';
    if (errorResponse.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = errorResponse.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${errorResponse.status}\nMessage: ${errorResponse.message}`;
    }
    this._LoggingSvc.console(`LoggingService.${methodName}:`, LogLevel.Error);
    this._LoggingSvc.console(errorMessage, LogLevel.Error);
  }
}
