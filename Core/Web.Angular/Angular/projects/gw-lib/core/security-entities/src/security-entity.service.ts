import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { AccountService } from '@growthware/core/account';
import { GWCommon } from '@growthware/common/services';
import { IKeyValuePair } from '@growthware/common/interfaces';
import { LoggingService } from '@growthware/core/logging';
import { SearchService } from '@growthware/core/search';
// Feature
import { IValidSecurityEntities } from './valid-security-entities.model';
import { ISecurityEntityProfile } from './security-entity-profile.model';

@Injectable({
	providedIn: 'root'
})
export class SecurityEntityService {

	private _ApiName: string = 'GrowthwareSecurityEntity/';
	private _Api_GetSecurityEntity: string = '';
	private _Api_GetValidParents: string = '';
	private _Api_GetValidSecurityEntities: string = '';
	private _Api_SaveSecurityEntity: string = '';
  
	readonly addEditModalId: string = 'addEditAccountModal';

	modalReason: string = '';
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	selectedRow: any = {};

	constructor(
    private _AccountSvc: AccountService,
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private _SearchSvc: SearchService,
	) { 
		this._Api_GetValidSecurityEntities = this._GWCommon.baseURL + this._ApiName + 'GetValidSecurityEntities';
		this._Api_GetValidParents = this._GWCommon.baseURL + this._ApiName + 'GetValidParents';
		this._Api_GetSecurityEntity = this._GWCommon.baseURL + this._ApiName + 'GetProfile';
		this._Api_SaveSecurityEntity = this._GWCommon.baseURL + this._ApiName + 'SaveProfile';
	}

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

	public async getValidSecurityEntities(): Promise<IValidSecurityEntities[]> {
		return new Promise<IValidSecurityEntities[]>((resolve, reject) => {
			this._HttpClient.get<IValidSecurityEntities[]>(this._Api_GetValidSecurityEntities).subscribe({
				next: (response: IValidSecurityEntities[]) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'SecurityEntityService', 'getValidSecurityEntities');
				},
				// complete: () => {}
			});
		});
	}

	public async save(securityEntity: ISecurityEntityProfile): Promise<boolean> {
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<boolean>(this._Api_SaveSecurityEntity, securityEntity).subscribe({
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
