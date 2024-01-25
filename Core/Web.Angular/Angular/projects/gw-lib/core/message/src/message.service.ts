import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { SearchService } from '@growthware/core/search';
// Feature
import { IMessageProfile } from './message-profile.model';

@Injectable({
	providedIn: 'root'
})
export class MessageService {

	private _Api_GetProfile: string = '';
	private _ApiName: string = 'GrowthwareMessage/';
	private _Api_Save: string = '';
  
	readonly addEditModalId: string = 'addEditAccountModal';

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	selectedRow: any = {};
	modalReason: string = '';

	constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient, 
    private _LoggingSvc: LoggingService,
    private _SearchSvc: SearchService
	) { 
		this._Api_GetProfile = this._GWCommon.baseURL + this._ApiName + 'GetProfile/';
		this._Api_Save = this._GWCommon.baseURL + this._ApiName + 'Save/';
	}

	public async getProfile(id: number): Promise<IMessageProfile> {
		return new Promise<IMessageProfile>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('id', id);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.get<IMessageProfile>(this._Api_GetProfile, mHttpOptions).subscribe({
				next: (response: IMessageProfile) => {
					// console.log('MessageService.getProfile.response', response);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'MessageService', 'getMessageForEdit');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	public async save(messageProfile: IMessageProfile): Promise<IMessageProfile> {
		return new Promise<IMessageProfile>((resolve, reject) => {
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
			};
			this._HttpClient.post<IMessageProfile>(this._Api_Save, messageProfile, mHttpOptions).subscribe({
				next: (response: IMessageProfile) => {
					const mSearchCriteria = this._SearchSvc.getSearchCriteria('Messages'); // from SearchMessagesComponent (this.configurationName)
					if(mSearchCriteria != null) {
						this._SearchSvc.setSearchCriteria('Messages', mSearchCriteria);
					}
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'MessageService', 'save');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}
}
