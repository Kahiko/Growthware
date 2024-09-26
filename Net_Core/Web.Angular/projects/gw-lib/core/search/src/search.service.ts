import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
// Library
import { GWCommon } from '@growthware/common/services';
import { ITotalRecords } from '@growthware/common/interfaces';
// This Feature
import { ISearchResultsNVP, SearchCriteria, SearchCriteriaNVP, SearchResultsNVP } from '../src/search-criteria.model';

@Injectable({
	providedIn: 'root'
})
export class SearchService {
	private _BaseUrl: string = '';
	private _SearchCriteria_NVP_Array: SearchCriteriaNVP[] = [];

	public searchCriteriaChanged$ = signal<SearchCriteriaNVP>(new SearchCriteriaNVP('', new SearchCriteria([], [], 0, '', 0)));
	public searchDataChanged$ = signal<SearchResultsNVP>(new SearchResultsNVP('', { data: [], totalRecords: 0, searchCriteria: new SearchCriteria([], [], 0, '', 0) }));

	constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
	) {
		this._BaseUrl = this._GWCommon.baseURL;
	}

	/**
	 * Get search results from the specified URL using the provided search criteria.
	 *
	 * @param {string} url - the URL for the search request
	 * @param {SearchCriteriaNVP} criteria - the search criteria object
	 * @return {Promise<ISearchResultsNVP>} a promise that resolves with the search results
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
				.post<Array<ITotalRecords>>(mUrl, criteria.payLoad, mHttpOptions)
				.subscribe({
					next: (response: Array<ITotalRecords>) => {
						const mTotalRecords = this._GWCommon.getTotalRecords(response);
						const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(criteria.name, { data: response, totalRecords: mTotalRecords, searchCriteria: criteria.payLoad });
						resolve(mSearchResultsNVP);
					},
					error: (errorResponse: unknown) => {
						reject(errorResponse);
					},
					// complete: () => console.info('complete')
				});
		});
	}

	/**
	 * Returns a locally stored search criteria or null given the name of the SearchCriteria.
	 *
	 * @param {string} name - description of parameter
	 * @return {null | SearchCriteria} description of return value
	 */
	public getSearchCriteria(name: string): null | SearchCriteria {
		const index = this._SearchCriteria_NVP_Array.findIndex(searchCriteriaNVP => searchCriteriaNVP.name === name);
		let mRetVal = null;
		if (index > -1) {
			mRetVal = this._SearchCriteria_NVP_Array[index].payLoad;
		}
		return mRetVal;
	}

	/**
	 * Notify when search data has changed by calling searchDataChanged$.next
	 *
	 * @param {string} name - the name of the search data
	 * @param {Array<ITotalRecords>} data - the array of total records
	 * @param {SearchCriteria} searchCriteria - the search criteria
	 * @return {void} 
	 */
	public notifySearchDataChanged(name: string, data: Array<ITotalRecords>, searchCriteria: SearchCriteria): void {
		const mTotalRecords = this._GWCommon.getTotalRecords(data);
		const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(name, { data: data, totalRecords: mTotalRecords, searchCriteria: searchCriteria });
		this.searchDataChanged$.update(() => mSearchResultsNVP);
	}

	/**
	 * Adds or updates thel ocally stored search criteria for a given name.
	 *
	 * @param {string} name - the name of the search criteria
	 * @param {SearchCriteria} searchCriteria - the criteria to be set
	 */
	public setSearchCriteria(name: string, searchCriteria: SearchCriteria) {
		const mChangedCriteria = new SearchCriteriaNVP(name, searchCriteria);
		this._GWCommon.addOrUpdateArray(this._SearchCriteria_NVP_Array, mChangedCriteria, 'name');
		this.searchCriteriaChanged$.update(() => mChangedCriteria);
	}
}
