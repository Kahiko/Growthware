import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
// Library
import { GWCommon } from '@growthware/common/services';
// This Feature
import { ISearchResultsNVP, SearchCriteria, SearchCriteriaNVP, SearchResultsNVP } from '../src/search-criteria.model';

@Injectable({
	providedIn: 'root'
})
export class SearchService {
	private _BaseUrl: string = '';
	private _SearchCriteria_NVP_Array: SearchCriteriaNVP[] = [];

	public searchCriteriaChanged$ = new Subject<SearchCriteriaNVP>();
	public searchDataChanged$ = new Subject<SearchResultsNVP>();

	constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
	) {
		this._BaseUrl = this._GWCommon.baseURL;
	}

	/**
   * Calls GrowthwareAPI.Search
   *
   * @param {SearchCriteria} criteria
   * @return {*}  {Promise<any>}
   * @memberof GWLibSearchService
   */
	public async getResults(url: string, criteria: SearchCriteriaNVP): Promise<ISearchResultsNVP> {
		const mUrl: string = this._BaseUrl + url;
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		};
		return new Promise<SearchResultsNVP>((resolve, reject) => {
			this._HttpClient
				.post<any>(mUrl, criteria.payLoad, mHttpOptions)
				.subscribe({
					next: (response: any) => {
						const mTotalRecords = this._GWCommon.getTotalRecords(response);
						const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(criteria.name, { data: response, totalRecords: mTotalRecords, searchCriteria: criteria.payLoad });
						resolve(mSearchResultsNVP);
					},
					error: (errorResponse: any) => {
						reject(errorResponse);
					},
					// complete: () => console.info('complete')
				});
		});
	}

	public getSearchCriteria(name: string): null | SearchCriteria {
		const index = this._SearchCriteria_NVP_Array.findIndex(searchCriteriaNVP => searchCriteriaNVP.name === name);
		let mRetVal = null;
		if (index > -1) {
			mRetVal = this._SearchCriteria_NVP_Array[index].payLoad;
		}
		return mRetVal;
	}

	public notifySearchDataChanged(name: string, data: unknown[], searchCriteria: SearchCriteria): void {
		const mTotalRecords = this._GWCommon.getTotalRecords(data);
		const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(name, { data: data, totalRecords: mTotalRecords, searchCriteria: searchCriteria });
		this.searchDataChanged$.next(mSearchResultsNVP);
	}

	public setSearchCriteria(name: string, searchCriteria: SearchCriteria) {
		const mChangedCriteria = new SearchCriteriaNVP(name, searchCriteria);
		this._GWCommon.addOrUpdateArray(this._SearchCriteria_NVP_Array, mChangedCriteria);
		this.searchCriteriaChanged$.next(mChangedCriteria);
	}
}
