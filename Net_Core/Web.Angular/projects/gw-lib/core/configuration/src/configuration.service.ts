import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
// Feature
import { IAppSettings } from './app-settings.model';
import { IDBInformation, DBInformation } from './db-information.model';

@Injectable({
	providedIn: 'root'
})
export class ConfigurationService {
	private _ApplicationName = new BehaviorSubject('');
	private _ApiName: string = 'GrowthwareAPI/';
	private _ApiAppSettingsURL: string = '';
	private _ApiGetDBInformationURL: string = '';
	private _ApiSetDBInformationURL: string = '';
	private _Environment = new BehaviorSubject('not set');
	private _Loaded: boolean = false;
	private _LogPriority = new BehaviorSubject('Debug');
	private _SecurityEntityTranslation = new BehaviorSubject('Security Entity');
	private _Version = new BehaviorSubject('0.0.0.0');

	readonly applicationName$ = this._ApplicationName.asObservable();
	readonly environment$ = this._Environment.asObservable();
	readonly logPriority$ = this._LogPriority.asObservable();
	readonly securityEntityTranslation$ = this._SecurityEntityTranslation.asObservable();
	readonly version$ = this._Version.asObservable();

	constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService
	) {
		this._ApiAppSettingsURL = this._GWCommon.baseURL + this._ApiName + 'GetAppSettings';
		this._ApiGetDBInformationURL = this._GWCommon.baseURL + this._ApiName + 'GetDBInformation';
		this._ApiSetDBInformationURL = this._GWCommon.baseURL + this._ApiName + 'UpdateProfile';
		this.loadAppSettings();
	}

	public async getDBInformation(): Promise<IDBInformation> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		};
		return new Promise<DBInformation>((resolve, reject) => {
			// console.log('getDBInformation', this._ApiDBInformationURL);
			this._HttpClient.get<IDBInformation>(this._ApiGetDBInformationURL, mHttpOptions).subscribe({
				next: (response: IDBInformation) => {
					resolve(response);
				},
				// eslint-disable-next-line @typescript-eslint/no-explicit-any
				error: (errorResponse: any) => {
					this._LoggingSvc.errorHandler(errorResponse, 'ConfigurationService', 'getDBInformation');
					reject(errorResponse);
				},
				// complete: () => console.info('complete')
			});
		});
	}

	public loadAppSettings(): void {
		if(this._Loaded === false) {
			const mUrl = this._ApiAppSettingsURL;
			this._HttpClient.get<IAppSettings>(mUrl).subscribe({
				next: (response: IAppSettings) => {
					if(response.name) { this._ApplicationName.next(response.name); }
					if(response.environment) { this._Environment.next(response.environment); }
					if(response.logPriority) { this._LogPriority.next(response.logPriority); }
					if(response.version) { this._Version.next(response.version); }
					if(response.securityEntityTranslation) { this._SecurityEntityTranslation.next(response.securityEntityTranslation); }
					this._Loaded = true;
				},
				// eslint-disable-next-line @typescript-eslint/no-explicit-any
				error: (errorResponse: any) => {
					this._LoggingSvc.errorHandler(errorResponse, 'ConfigurationService', 'getAppSettings');
				},
				complete: () => console.info('complete')
			});
		}
	}

	public async updateProfile(enableInheritance: number): Promise<boolean> {
		if(this._GWCommon.isNullOrUndefined(enableInheritance)) {
			throw new Error('enableInheritance can not be blank!');
		}
		const mQueryParameter: HttpParams = new HttpParams()
			.set('enableInheritance', enableInheritance);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<boolean>((resolve) => {
			this._HttpClient.post<boolean>(this._ApiSetDBInformationURL, mQueryParameter, mHttpOptions).subscribe({
				// eslint-disable-next-line @typescript-eslint/no-explicit-any
				next: (response: any) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'ConfigurationService', 'updateProfile');
				},
				// complete: () => {}
			});
		});
	}
}