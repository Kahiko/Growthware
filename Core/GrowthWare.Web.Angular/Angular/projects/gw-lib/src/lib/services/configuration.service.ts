import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Subject } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { IAppSettings } from '@Growthware/Lib/src/lib/models';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  private _ApplicationName = new Subject<string>();
  private _ApiName: string = 'GrowthwareAPI/';
  private _ApiURL: string = '';
  private _Loaded: boolean = false;
  private _LogPriority = new Subject<string>();
  private _Version = new Subject<string>();

  readonly applicationName = this._ApplicationName.asObservable();
  readonly logPriority = this._LogPriority.asObservable();
  readonly version = this._Version.asObservable();

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService
  ) {
    this._ApiURL = this._GWCommon.baseURL + this._ApiName;
    this.loadAppSettings();
  }

  public loadAppSettings(): void {
    if(this._Loaded === false) {
      const mUrl = this._ApiURL + 'GetAppSettings';
      this._HttpClient.get<IAppSettings>(mUrl).subscribe({
        next: (response: IAppSettings) => {
          if(response.name) { this._ApplicationName.next(response.name); }
          if(response.logPriority) { this._LogPriority.next(response.logPriority); }
          if(response.version) { this._Version.next(response.version); }
          this._Loaded = true;
        },
        error: (errorResponse: any) => {
          this.errorHandler(errorResponse, 'getAppSettings');
        },
        complete: () => console.info('complete')
      });
    }
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
