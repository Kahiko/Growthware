import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
// Feature
import { IGroupProfile } from './group-profile.model';
import { SelectedRow } from './selected-row.model';

@Injectable({
	providedIn: 'root'
})
export class GroupService implements BaseService {
	private _ApiName: string = 'GrowthwareGroup/';
	private _Api_DeleteGroup = '';
	private _Api_GetGroupForEdit = '';
	private _Api_SaveGroup = '';

	readonly addEditModalId: string = 'addEditAccountModal';
  
	selectedRow: SelectedRow = new SelectedRow();
	modalReason: string = '';

	constructor(private _GWCommon: GWCommon, private _HttpClient: HttpClient, private _LoggingSvc: LoggingService) { 
		this._Api_DeleteGroup = this._GWCommon.baseURL + this._ApiName + 'DeleteGroup/';
		this._Api_GetGroupForEdit = this._GWCommon.baseURL + this._ApiName + 'GetGroupForEdit/';
		this._Api_SaveGroup = this._GWCommon.baseURL + this._ApiName + 'SaveGroup/';
	}

	public async delete(groupSeqId: number): Promise<boolean> {
		return new Promise<boolean>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('groupSeqId', groupSeqId);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.post<boolean>(this._Api_DeleteGroup, null, mHttpOptions).subscribe({
				next: (response: boolean) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'GroupService', 'delete');
					reject(false);
				},
				// complete: () => {}
			});
		});
	}

	/**
   * Retrieves the group profile for editing based on the provided group sequence ID.
   *
   * @param {number} groupSeqId - The sequence ID of the group.
   * @return {Promise<IGroupProfile>} A Promise that resolves with the group profile object.
   */
	public async getGroupForEdit(groupSeqId: number): Promise<IGroupProfile> {
		// console.log('GroupService.getGroupForEdit.groupSeqId', groupSeqId);
		return new Promise<IGroupProfile>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('groupSeqId', groupSeqId);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.get<IGroupProfile>(this._Api_GetGroupForEdit, mHttpOptions).subscribe({
				next: (response: IGroupProfile) => {
					// console.log('getGroupForEdit.GetGroupForEdit.response', response);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'GroupService', 'getGroupForEdit');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
   * Retrieves the groups from the API.
   *
   * @return {Promise<aArray<string>ny>} A promise that resolves with the groups retrieved from the API.
   */
	public async getGroups(): Promise<Array<string>> {
		return new Promise<Array<string>>((resolve, reject) => {
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
			};
			const mUrl = this._GWCommon.baseURL + this._ApiName + 'GetGroups';
			this._HttpClient.get<Array<string>>(mUrl, mHttpOptions).subscribe({
				next: (response: Array<string>) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'GroupService', 'getGroups');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
   * Saves a group profile.
   *
   * @param {IGroupProfile} profile - The group profile to be saved.
   * @return {Promise<boolean>} A promise that resolves to true if the group profile is successfully saved, or false otherwise.
   */
	public async saveGroup(profile : IGroupProfile): Promise<boolean> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<boolean>(this._Api_SaveGroup, profile, mHttpOptions).subscribe({
				next: (response: boolean) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'GroupService', 'saveGroup');
					reject(false);
				},
				// complete: () => {}
			});
		});
	}
}
