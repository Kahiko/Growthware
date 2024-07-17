import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { AccountService } from '@growthware/core/account';
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { IKeyValuePair } from '@growthware/common/interfaces';
import { LoggingService } from '@growthware/core/logging';
import { SearchService } from '@growthware/core/search';
import { SelectedRow } from './selected-row.model';
// Feature
import { IRegistrationInformation } from './registration-information.model';
import { ISecurityEntityProfile } from './security-entity-profile.model';
import { IValidSecurityEntities } from './valid-security-entities.model';

@Injectable({
	providedIn: 'root'
})
export class SecurityEntityService extends BaseService {

	private _ApiName: string = 'GrowthwareSecurityEntity/';
	private _Api_GetRegistrationInformation: string = '';
	private _Api_GetSecurityEntity: string = '';
	private _Api_GetValidParents: string = '';
	private _Api_GetValidSecurityEntities: string = '';
	private _Api_SaveSecurityEntity: string = '';
  
	readonly addEditModalId: string = 'addEditAccountModal';

	modalReason: string = '';
	selectedRow: SelectedRow = new SelectedRow();

	constructor(
		private _AccountSvc: AccountService,
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
		private _SearchSvc: SearchService,
	) { 
		super();
		this._Api_GetValidSecurityEntities = this._GWCommon.baseURL + this._ApiName + 'GetValidSecurityEntities';
		this._Api_GetValidParents = this._GWCommon.baseURL + this._ApiName + 'GetValidParents';
		this._Api_GetSecurityEntity = this._GWCommon.baseURL + this._ApiName + 'GetProfile';
		this._Api_GetRegistrationInformation = this._GWCommon.baseURL + this._ApiName + 'GetRegistrationInformation';
		this._Api_SaveSecurityEntity = this._GWCommon.baseURL + this._ApiName + 'SaveProfile';
	}

	/**
	 * @description Returns a Security Entity for the given id.
	 * @param id SecurityEntitySeqId
	 * @returns Promise<ISecurityEntityProfile>
	 */
	public async getSecurityEntity(id: number): Promise<ISecurityEntityProfile> {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('securityEntitySeqId', id);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<ISecurityEntityProfile>((resolve, reject) => {
			this._HttpClient.get<ISecurityEntityProfile>(this._Api_GetSecurityEntity, mHttpOptions).subscribe({
				next: (response: ISecurityEntityProfile) => {
					// console.log('SecurityEntityService.getSecurityEntity', response);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'getSecurityEntity');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * @description Returns a Registration Information for the given id.
	 * @param id The desired SecurityEntitySeqId
	 * @returns Promise<IRegistrationInformation[]
	 */
	public async getRegistrationInformation(id: number): Promise<IRegistrationInformation[]> {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('securityEntitySeqId', id);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<IRegistrationInformation[]>((resolve, reject) => {
			this._HttpClient.get<IRegistrationInformation[]>(this._Api_GetRegistrationInformation, mHttpOptions).subscribe({
				next: (response: IRegistrationInformation[]) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'getValidParents');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * Returns a list of valid parents given a security entity id
	 * @param id SecurityEntitySeqId
	 * @returns Promise<IKeyValuePair[]>
	 */
	public async getValidParents(id: number): Promise<IKeyValuePair[]> {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('securityEntitySeqId', id);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<IKeyValuePair[]>((resolve, reject) => {
			this._HttpClient.get<IKeyValuePair[]>(this._Api_GetValidParents, mHttpOptions).subscribe({
				next: (response: IKeyValuePair[]) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'getValidParents');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * @description Returns a list of valid Security Entities
	 * @returns Promise<IValidSecurityEntities[]>
	 */
	public async getValidSecurityEntities(): Promise<IValidSecurityEntities[]> {
		return new Promise<IValidSecurityEntities[]>((resolve, reject) => {
			this._HttpClient.get<IValidSecurityEntities[]>(this._Api_GetValidSecurityEntities).subscribe({
				next: (response: IValidSecurityEntities[]) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'getValidSecurityEntities');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * @description Saves the Security Entity and the Registration Information
	 * @param securityEntity ISecurityEntityProfile
	 * @param registrationInformation IRegistrationInformation
	 * @returns Promise<boolean>
	 */
	public async save(securityEntity: ISecurityEntityProfile, registrationInformation: IRegistrationInformation): Promise<boolean> {
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<boolean>(this._Api_SaveSecurityEntity, { securityEntity, registrationInformation }).subscribe({
				next: (response: boolean) => {
					const mSearchCriteria = this._SearchSvc.getSearchCriteria('Security_Entities'); // from SearchSecurityEntitiesComponent (this.configurationName)
					if(mSearchCriteria != null) {
						this._SearchSvc.setSearchCriteria('Security_Entities', mSearchCriteria);
					}
					this._AccountSvc.triggerMenuUpdate();
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'saveSecurityEntity');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}
}
