import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { AccountService } from '@growthware/core/account';
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { IKeyDataPair, IKeyValuePair } from '@growthware/common/interfaces';
import { ISearchCriteria, SearchService } from '@growthware/core/search';
// Feature
import { IFunctionProfile } from './function-profile.model';
import { SelectedRow } from './selected-row.model';
import { IFunctionMenuOrder } from './function-menu-order.model';

@Injectable({
	providedIn: 'root'
})
export class FunctionService extends BaseService {

	private _FunctionSeqId: number = -1;
	private _ApiName: string = 'GrowthwareFunction/';
	private _Api_AvalibleParents: string = '';
	private _Api_CopyFunctionSecurity: string = '';
	private _Api_Delete: string = '';
	private _Api_GetFunction: string = '';
	private _Api_GetFunctionOrder: string = '';
	private _Api_FunctionTypes: string = '';
	private _Api_LinkBehaviors: string = '';
	private _Api_NavigationTypes: string = '';
	private _Api_Save: string = '';
	private _Reason: string = '';

	modalReason: string = '';
	selectedRow: SelectedRow = new SelectedRow();

	public get functionSeqId(): number {
		return this._FunctionSeqId;
	}
	public set functionSeqId(value: number) {
		this._FunctionSeqId = value;
	}
  
	public get reason(): string {
		return this._Reason;
	}
	public set reason(value: string) {
		this._Reason = value;
	}

	readonly addEditModalId: string = 'addEditAccountModal';

	constructor(
		private _AccountSvc: AccountService,
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
		private _SearchSvc: SearchService,
	) {
		super();
		this._Api_AvalibleParents = this._GWCommon.baseURL + this._ApiName + 'GetAvalibleParents';
		this._Api_CopyFunctionSecurity = this._GWCommon.baseURL + this._ApiName + 'CopyFunctionSecurity';
		this._Api_Delete = this._GWCommon.baseURL + this._ApiName + 'DeleteFunction';
		this._Api_GetFunction = this._GWCommon.baseURL + this._ApiName + 'GetFunction';
		this._Api_GetFunctionOrder = this._GWCommon.baseURL + this._ApiName + 'GetFunctionOrder';
		this._Api_FunctionTypes = this._GWCommon.baseURL + this._ApiName + 'GetFunctionTypes';
		this._Api_NavigationTypes = this._GWCommon.baseURL + this._ApiName + 'GetNavigationTypes';
		this._Api_LinkBehaviors = this._GWCommon.baseURL + this._ApiName + 'GetLinkBehaviors';
		this._Api_Save = this._GWCommon.baseURL + this._ApiName + 'Save';
	}

	public async copyFunctionSecurity(source: number, target: number): Promise<boolean> {
		const mQueryParameter: HttpParams = new HttpParams().append('source', source).append('target', target);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<boolean>(this._Api_CopyFunctionSecurity, null, mHttpOptions).subscribe({
				next: (response: boolean) => {
					this._AccountSvc.triggerMenuUpdates();
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'copyFunctionSecurity');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	public async deleteFunction(functionSeqId: number): Promise<boolean> {
		const mQueryParameter: HttpParams = new HttpParams().append('functionSeqId', functionSeqId);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.delete<boolean>(this._Api_Delete, mHttpOptions).subscribe({
				next: (response: boolean) => {
					this._AccountSvc.triggerMenuUpdates();
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'deleteFunction');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	public async getAvalibleParents(): Promise<Array<IKeyDataPair>> {
		return new Promise<Array<IKeyDataPair>>((resolve, reject) => {
			this._HttpClient.get<Array<IKeyDataPair>>(this._Api_AvalibleParents).subscribe({
				next: (response: Array<IKeyDataPair>) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'getAvalibleParents');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});    
	}

	/**
   * Gets a FunctionProfile given the functionSeqId
   *
   * @param {number} functionSeqId
   * @return {*}  {Promise<IFunctionProfile>}
   * @memberof FunctionService
   */
	public async getFunction(functionSeqId: number): Promise<IFunctionProfile> {
		const mQueryParameter: HttpParams = new HttpParams().append('functionSeqId', functionSeqId);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<IFunctionProfile>((resolve, reject) => {
			this._HttpClient.get<IFunctionProfile>(this._Api_GetFunction, mHttpOptions).subscribe({
				next: (response: IFunctionProfile) => {
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

	public async getFunctionOrder(functionSeqId: number): Promise<Array<IFunctionMenuOrder>> {
		const mQueryParameter: HttpParams = new HttpParams().append('functionSeqId', functionSeqId);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<Array<IFunctionMenuOrder>>((resolve, reject) => {
			this._HttpClient.get<Array<IFunctionMenuOrder>>(this._Api_GetFunctionOrder, mHttpOptions).subscribe({
				next: (response: Array<IFunctionMenuOrder>) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFunctionOrder');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
   * Retrieves the function types from the API.
   *
   * @return {Promise<any>} A promise that resolves with the response from the API.
   */
	public async getFuncitonTypes(): Promise<Array<IKeyValuePair>> {
		return new Promise<Array<IKeyValuePair>>((resolve, reject) => {
			this._HttpClient.get<Array<IKeyValuePair>>(this._Api_FunctionTypes).subscribe({
				next: (response: Array<IKeyValuePair>) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFuncitonTypes');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	public async getLinkBehaviors(): Promise<Array<IKeyValuePair>> {
		return new Promise<Array<IKeyValuePair>>((resolve, reject) => {
			this._HttpClient.get<Array<IKeyValuePair>>(this._Api_LinkBehaviors).subscribe({
				next: (response: Array<IKeyValuePair>) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFuncitonTypes');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	public async getNavigationTypes(): Promise<Array<IKeyValuePair>> {
		return new Promise<Array<IKeyValuePair>>((resolve, reject) => {
			this._HttpClient.get<Array<IKeyValuePair>>(this._Api_NavigationTypes).subscribe({
				next: (response: Array<IKeyValuePair>) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'GetFuncitonTypes');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	public async save(functionProfile: IFunctionProfile): Promise<boolean> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<ISearchCriteria>(this._Api_Save, functionProfile, mHttpOptions).subscribe({
				next: () => {
					const mSearchCriteria = this._SearchSvc.getSearchCriteria('Functions'); // from SearchFunctionsComponent (this.configurationName)
					if(mSearchCriteria != null) {
						this._SearchSvc.setSearchCriteria('Functions', mSearchCriteria);
					}
					this._AccountSvc.triggerMenuUpdates();
					resolve(true);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'save');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}
}
