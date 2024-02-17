import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { SearchService } from '@growthware/core/search';
// Feature
import { IStateProfile } from './state-profile.model';
import { SelectedRow } from './selected-row.model';

@Injectable({
	providedIn: 'root'
})
export class StatesService extends BaseService {

	private _ApiName: string = 'GrowthwareState/';
	private _Api_GetProfile: string = '';
	private _Api_Save: string = '';

	readonly addEditModalId: string = 'addEditAccountModal';

	modalReason: string = '';
	selectedRow: SelectedRow = new SelectedRow();

	constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
		private _SearchSvc: SearchService
	) { 
		super();
		this._Api_GetProfile = this._GWCommon.baseURL + this._ApiName + 'GetProfile';
		this._Api_Save = this._GWCommon.baseURL + this._ApiName + 'Save';
	}

	public async getState(state: string): Promise<IStateProfile> {
		const mQueryParameter: HttpParams = new HttpParams().append('state', state);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};    
		return new Promise<IStateProfile>((resolve, reject) => {
			this._HttpClient.get<IStateProfile>(this._Api_GetProfile, mHttpOptions).subscribe({
				next: (response: IStateProfile) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	public async saveState(stateProfile: IStateProfile): Promise<boolean> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<boolean>(this._Api_Save, stateProfile, mHttpOptions).subscribe({
				next: (response: boolean) => {
					const mSearchCriteria = this._SearchSvc.getSearchCriteria('States'); // from SearchStatesComponent (this.configurationName)
					if(mSearchCriteria != null) {
						this._SearchSvc.setSearchCriteria('States', mSearchCriteria);
					}
					resolve(response);
				}
				, error: (error) => {
					this._LoggingSvc.errorHandler(error, 'StatesService', 'saveState');
					reject('Failed to call the API');
				}
			});
		});
	}
}
