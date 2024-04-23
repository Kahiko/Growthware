import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
// Library
import { GWCommon } from '@growthware/common/services';
import { INaturalSortResults } from '@growthware/common/interfaces';
import { LoggingService } from '@growthware/core/logging';
// Feature
import { LineCount } from './line-count.model';

@Injectable({
	providedIn: 'root'
})
export class SysAdminService {
	private _ApiName: string = 'GrowthwareFile/';
	private _Api_GetTestNaturalSort: string = '';
	private _ApiLineCount: string = '';

	constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,    
	) { 
		this._ApiLineCount = this._GWCommon.baseURL + this._ApiName + 'GetLineCount';
		this._Api_GetTestNaturalSort = this._ApiName + 'GetTestNaturalSort';
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
