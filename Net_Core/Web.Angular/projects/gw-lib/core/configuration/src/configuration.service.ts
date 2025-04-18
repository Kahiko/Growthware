import { computed, Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
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
	private _ApiName: string = 'GrowthwareAPI/';
	private _ApiAppSettingsURL: string = '';
	private _ApiGetDBInformationURL: string = '';
	private _ApiSetDBInformationURL: string = '';
	private _ApplicationName = signal<string>('');
	private _ChunkSize = signal<number>(0); // The default value is 30MB in Kestrel so this is a bit smaller
	private _Environment = signal<string>('');
	private _Loaded: boolean = false;
	private _LogPriority = signal<string>('');
	private _SecurityEntityTranslation = signal<string>('');
	private _Version = signal<string>('');

	readonly applicationName = computed(() => this._ApplicationName());
	readonly chunkSize = computed(() => this._ChunkSize());
	readonly environment = computed(() => this._Environment());
	readonly logPriority = computed(() => this._LogPriority());
	readonly securityEntityTranslation = computed(() => this._SecurityEntityTranslation());
	readonly version = computed(() => this._Version());

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
		if (this._Loaded === false) {
			const mUrl = this._ApiAppSettingsURL;
			this._HttpClient.get<IAppSettings>(mUrl).subscribe({
				next: (response: IAppSettings) => {
					if (response.name) { this._ApplicationName.set(response.name); }
					if (response.chunkSize) { this._ChunkSize.set(response.chunkSize); }
					if (response.environment) { this._Environment.set(response.environment); }
					if (response.logPriority) { this._LogPriority.set(response.logPriority); }
					if (response.securityEntityTranslation) { this._SecurityEntityTranslation.set(response.securityEntityTranslation); }
					if (response.version) { this._Version.set(response.version); }
					this._Loaded = true;
				},
				// eslint-disable-next-line @typescript-eslint/no-explicit-any
				error: (errorResponse: any) => {
					this._LoggingSvc.errorHandler(errorResponse, 'ConfigurationService', 'getAppSettings');
				},
				// complete: () => console.info('complete')
			});
		}
	}

	public async save(enableInheritance: number): Promise<boolean> {
		console.log('ConfigurationService.save', enableInheritance);
		if (this._GWCommon.isNullOrUndefined(enableInheritance)) {
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
