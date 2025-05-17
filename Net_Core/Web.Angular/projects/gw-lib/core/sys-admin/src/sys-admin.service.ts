import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
// Library
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { INaturalSortResults } from '@growthware/common/interfaces';
import { LoggingService } from '@growthware/core/logging';
// Feature
import { LineCount } from './line-count.model';
import { SelectedRow } from './selected-row.model';


@Injectable({
	providedIn: 'root'
})
export class SysAdminService extends BaseService {
	private _ApiName: string = 'GrowthwareFile/';
	private _Api_CleanupSystemLogs = '';
	private _Api_CreateSystemLogs = '';
	private _Api_DownloadSystemLogs = '';
	private _Api_GetTestNaturalSort: string = '';
	private _ApiLineCount: string = '';
	private _Reason: string = '';

	modalReason: string = '';
	selectedRow: SelectedRow = new SelectedRow();

	public get reason(): string {
		return this._Reason;
	}
	public set reason(value: string) {
		this._Reason = value;
	}

	readonly addEditModalId: string = 'addEditDBLogModal';

	constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
	) {
		super();
		this._Api_CleanupSystemLogs = this._GWCommon.baseURL + 'GrowthwareAPI/CleanupSystemLogs';
		this._Api_CreateSystemLogs = this._GWCommon.baseURL + 'GrowthwareAPI/CreateSystemLogs';
		this._Api_DownloadSystemLogs = this._GWCommon.baseURL + 'GrowthwareAPI/DownloadSystemLogs';
		this._ApiLineCount = this._GWCommon.baseURL + this._ApiName + 'GetLineCount';
		this._Api_GetTestNaturalSort = this._ApiName + 'GetTestNaturalSort';
	}


	async cleanupSystemLogs(fileId: string): Promise<boolean> {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('fileId', fileId);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
			responseType: 'text' as 'json',
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.delete(this._Api_CleanupSystemLogs, mHttpOptions).subscribe({
				next: () => {
					resolve(true);
				},
				error: (errorResponse: string | HttpErrorResponse) => {
					this._LoggingSvc.errorHandler(errorResponse, 'SysAdminService', 'cleanupSystemLogs');
					reject(false);
				},
				// complete: () => console.info('complete')
			})
		});
	}

	async createSystemLogs(): Promise<string> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			responseType: 'text' as 'json',
		};
		return new Promise<string>((resolve, reject) => {
			this._HttpClient
				.get<string>(this._Api_CreateSystemLogs, mHttpOptions)
				.subscribe({
					next: (response: string) => {
						resolve(response);
					},
					error: (errorResponse) => {
						this._LoggingSvc.errorHandler(errorResponse, 'SysAdminService', 'createSystemLogs');
						reject(errorResponse);
					},
					// complete: () => console.info('complete')
				});
		});
	}

	public downloadSystemLogs(fileId: string) {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('fileId', fileId);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			responseType: 'blob' as 'json',
			params: mQueryParameter,
		};
		return new Promise<Blob>((resolve, reject) => {
			this._HttpClient
				.get<Blob>(this._Api_DownloadSystemLogs, mHttpOptions)
				.subscribe({
					next: (response: Blob) => {
						resolve(response);
					},
					error: (errorResponse) => {
						this._LoggingSvc.errorHandler(errorResponse, 'SysAdminService', 'downloadSystemLogs');
						reject(errorResponse);
					},
					// complete: () => console.info('complete')
				});
		});
	}

	getLineCount(lineCount: LineCount): Promise<string> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			responseType: 'text' as 'json',
		};
		return new Promise<string>((resolve, reject) => {
			this._HttpClient
				.post<string>(this._ApiLineCount, lineCount, mHttpOptions)
				.subscribe({
					next: (response: string) => {
						resolve(response);
					},
					error: (errorResponse) => {
						this._LoggingSvc.errorHandler(errorResponse, 'SysAdminService', 'getLineCount');
						reject(errorResponse);
					},
					// complete: () => console.info('complete')
				});
		});
	}

	public async getTestNaturalSort(sortDirection: string): Promise<INaturalSortResults> {
		return new Promise<INaturalSortResults>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams().append('sortDirection', sortDirection);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.get<INaturalSortResults>(this._Api_GetTestNaturalSort, mHttpOptions).subscribe({
				next: (response: INaturalSortResults) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getTestNaturalSort');
					reject(false);
				},
				// complete: () => {}
			});
		});
	}
}
