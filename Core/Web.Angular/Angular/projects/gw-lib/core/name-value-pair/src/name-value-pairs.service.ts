import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
// import { SearchService } from '@growthware/core/search';
// Feature
import { INvpParentProfile } from './name-value-pair-parent-profile.model';
import { INvpChildProfile } from './name-value-pair-child-profile.model';

@Injectable({
	providedIn: 'root'
})
export class NameValuePairService {
	private _ApiName: string = 'GrowthwareNameValuePair/';
	private _Api_Get_ParentProfile: string = '';
	private _Api_Save_Parent_Name_Value_Pair: string = '';

	public addEditModalId: string = 'AddOrEditNVPParrent';
	public modalReason: string = '';
	public nvpParentRow!: INvpParentProfile;
	public nvpChildRow!: INvpChildProfile;

	constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient, 
		private _LoggingSvc: LoggingService,
		// private _SearchSvc: SearchService
	) { 
		this._Api_Get_ParentProfile = this._GWCommon.baseURL + this._ApiName + 'GetMNameValuePair';
		this._Api_Save_Parent_Name_Value_Pair = this._GWCommon.baseURL + this._ApiName + 'SaveNameValuePairParent';
	}

	getParentProfile(): Promise<INvpParentProfile> {
		return new Promise<INvpParentProfile>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('nameValuePairSeqId', this.nvpParentRow['nvpSeqId'].toString());
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

	saveNameValuePairParent(profile: INvpParentProfile): Promise<INvpParentProfile> {
		return new Promise<INvpParentProfile>((resolve, reject) => {
			// const mQueryParameter: HttpParams = new HttpParams()
			// 	.set('nameValuePairSeqId', this.nvpParentRow['nvpSeqId'].toString());
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				// params: mQueryParameter,
			};
			this._HttpClient.post<INvpParentProfile>(this._Api_Save_Parent_Name_Value_Pair, profile, mHttpOptions).subscribe({
				next: (response: INvpParentProfile) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'NameValuePairService', 'saveNameValuePairParent');
					reject(error);
				}
			});
		});
	}

	setNameValuePairParrentRow(row: INvpParentProfile): void {
		this.nvpParentRow = JSON.parse(JSON.stringify(row));
	}

	setNameValuePairDetailRow(row: INvpChildProfile): void {
		this.nvpChildRow = JSON.parse(JSON.stringify(row));
	}

}
