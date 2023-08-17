import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Subject } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { IAppSettings } from './app-settings.model';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  private _ApplicationName = new BehaviorSubject('');
  private _ApiName: string = 'GrowthwareAPI/';
  private _ApiURL: string = '';
  private _Loaded: boolean = false;
  private _LogPriority = new Subject<string>();
  private _Version = new Subject<string>();

  readonly applicationName$ = this._ApplicationName.asObservable();
  readonly logPriority$ = this._LogPriority.asObservable();
  readonly version$ = this._Version.asObservable();

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
          this._LoggingSvc.errorHandler(errorResponse, 'ConfigurationService', 'getAppSettings');
        },
        complete: () => console.info('complete')
      });
    }
  }
}
