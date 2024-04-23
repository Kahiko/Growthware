import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { DynamicTableService } from '@growthware/core/dynamic-table';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { ISearchCriteria, SearchCriteria, SearchCriteriaNVP, SearchService } from '@growthware/core/search';
// Feature
import { INvpParentProfile } from './name-value-pair-parent-profile.model';
import { INvpChildProfile } from './name-value-pair-child-profile.model';

@Injectable({
	providedIn: 'root'
})
export class NameValuePairService {
	private _ApiName: string = 'GrowthwareNameValuePair/';
	private _Api_Get_ParentProfile: string = '';
	private _Api_Get_ChildProfile: string = '';
	private _Api_Save_ChildProfile: string = '';
	private _Api_Save_Parent_Name_Value_Pair: string = '';

	public addEditModalId: string = 'AddOrEditNVPParrent';
	public modalReason: string = '';
	public nvpChildId: number = 0;
	public nvpParentId: number = 0;
	public childConfigurationName = 'SearchNVPDetails';
	public parentConfigurationName = 'SearchNameValuePairs';

	constructor(
		private _DynamicTableSvc: DynamicTableService,
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
		private _SearchSvc: SearchService
	) {
		this._Api_Get_ChildProfile = this._GWCommon.baseURL + this._ApiName + 'GetMNameValuePairDetail';
		this._Api_Get_ParentProfile = this._GWCommon.baseURL + this._ApiName + 'GetMNameValuePair';
		this._Api_Save_ChildProfile = this._GWCommon.baseURL + this._ApiName + 'SaveNameValuePairDetail';
		this._Api_Save_Parent_Name_Value_Pair = this._GWCommon.baseURL + this._ApiName + 'SaveNameValuePairParent';
	}

	getChildProfile(): Promise<INvpChildProfile> {
		return new Promise<INvpChildProfile>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('nvpSeqId', this.nvpParentId.toString())
				.set('nvpDetailSeqId', this.nvpChildId.toString());
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			// The parameters names match the property names in the UISaveEventParameters.cs model
			this._HttpClient.get<INvpChildProfile>(this._Api_Get_ChildProfile, mHttpOptions).subscribe({
				next: (response: INvpChildProfile) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'NameValuePairService', 'getChildProfile');
					reject(error);
				}
			});
		});
	}

	/**
	 * @description Gets the name value pair parent profile from the API.
	 *
	 * @param {type} paramName - description of parameter
	 * @return {type} description of return value
	 */
	getParentProfile(): Promise<INvpParentProfile> {
		return new Promise<INvpParentProfile>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('nameValuePairSeqId', this.nvpParentId.toString());
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			// The parameters names match the property names in the UISaveEventParameters.cs model
			this._HttpClient.get<INvpParentProfile>(this._Api_Get_ParentProfile, mHttpOptions).subscribe({
				next: (response: INvpParentProfile) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'NameValuePairService', 'getParentProfile');
					reject(error);
				}
			});
		});
	}

	saveNameValuePairChild(profile: INvpChildProfile): Promise<INvpChildProfile> {
		return new Promise<INvpChildProfile>((resolve, reject) => {
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
			};
			this._HttpClient.post<INvpChildProfile>(this._Api_Save_ChildProfile, profile, mHttpOptions).subscribe({
				next: (response: INvpChildProfile) => {
					this.searchChildNameValuePairs(response.nameValuePairSeqId);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'NameValuePairService', 'saveNameValuePairChild');
					reject(error);
				}
			});
		});
	}

	/**
	 * @description Saves the name value pair parent profile.
	 *
	 * @param {INvpParentProfile} profile - The parent profile to be saved
	 * @return {Promise<INvpParentProfile>} A promise that resolves with the saved parent profile
	 */
	saveNameValuePairParent(profile: INvpParentProfile): Promise<INvpParentProfile> {
		return new Promise<INvpParentProfile>((resolve, reject) => {
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
			};
			this._HttpClient.post<INvpParentProfile>(this._Api_Save_Parent_Name_Value_Pair, profile, mHttpOptions).subscribe({
				next: (response: INvpParentProfile) => {
					resolve(response);
					let mNvpParentId = this.nvpParentId;
					if (!mNvpParentId) {
						mNvpParentId = 1;
					}
					this.searchChildNameValuePairs(mNvpParentId);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'NameValuePairService', 'saveNameValuePairParent');
					reject(error);
				}
			});
		});
	}

	/**
	 * @description A function to search child name-value pairs.
	 */
	public searchChildNameValuePairs(nvpParentId: number): void {
		const mSearchNVPDetailsConfig = this._DynamicTableSvc.getTableConfiguration(this.childConfigurationName);
		const mSearchCriteriaNVP = this._DynamicTableSvc.getSearchCriteriaFromConfig(this.childConfigurationName, mSearchNVPDetailsConfig);
		mSearchCriteriaNVP.payLoad.searchText = nvpParentId.toString();
		// Set the search child criteria to initiate search criteria changed subject
		this._SearchSvc.setSearchCriteria(mSearchCriteriaNVP.name, mSearchCriteriaNVP.payLoad);		
	}

	/**
	 * Initial parent SearchCriteriaNVP
	 */
	public searchParentNameValuePairs(): void {
		// Get the initial parent SearchCriteriaNVP
		const mSearchColumns: Array<string> = ['Static_Name', 'Display', 'Description'];
		const mSortColumns: Array<string> = ['Display'];
		const mNumberOfRecords: number = 10;
		const mSearchCriteria: ISearchCriteria = new SearchCriteria(mSearchColumns, mSortColumns, mNumberOfRecords, '', 1);
		const mResults: SearchCriteriaNVP = new SearchCriteriaNVP(this.parentConfigurationName, mSearchCriteria);
		// Set the search parent criteria to initiate search criteria changed subject
		this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
	}

	/**
	 * @description Sets the name value pair parent row.
	 * 
	 * @param row {INvpParentProfile} The parent profile to be set as the 'nvpParentRow'.
	 */
	// setNameValuePairParrentRow(row: INvpParentProfile): void {
	// 	this.nvpParentRow = JSON.parse(JSON.stringify(row));
	// }

	/**
	 * @description Sets the name value pair child row.
	 * 
	 * @param row {INvpChildProfile} The child profile to be set as the 'nvpChildRow'.
	 */
	// setNameValuePairDetailRow(row: INvpChildProfile): void {
	// 	this.nvpChildRow = JSON.parse(JSON.stringify(row));
	// }

}
