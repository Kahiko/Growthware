import { Component, effect, input, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
// Library
import { GWCommon } from '@growthware/common/services';
import { SearchService, SearchCriteria, ISearchResultsNVP } from '@growthware/core/search';

@Component({
	selector: 'gw-core-pager',
	standalone: true,
	imports: [
		FormsModule
	],
	templateUrl: './pager.component.html',
	styleUrls: ['./pager.component.scss'],
})
export class PagerComponent {
	private _SearchCriteria: SearchCriteria = new SearchCriteria([''], [''], 1, '', 1);

	name = input<string>('');

	public pages: number[] = [];
	public selectedPage: string = '1';
	public totalPages: number = 0;

	constructor(
		private _GWCommon: GWCommon,
		private _SearchSvc: SearchService
	) { 
		effect(() => {
			const mSearchDataResults = this._SearchSvc.searchDataChanged$();
			if (this.name().trim().toLowerCase() === mSearchDataResults.name.trim().toLowerCase()) {
				this._SearchCriteria = mSearchDataResults.payLoad.searchCriteria;
				if (mSearchDataResults.payLoad.data) {
					const mFirstRow = mSearchDataResults.payLoad.data[0];
					if (!this._GWCommon.isNullOrUndefined(mFirstRow)) {
						const mTotalRecords: number = parseInt(mFirstRow['TotalRecords']);
						const mPageSize: number = mSearchDataResults.payLoad.searchCriteria.pageSize;
						if (mTotalRecords > mPageSize) {
							let mCalculatedPages: number = Math.floor(mTotalRecords / mPageSize);
							const mRemainder = mTotalRecords % mPageSize;
							if (mRemainder != 0) {
								mCalculatedPages += 1;
							}
							if (this.totalPages !== mCalculatedPages) {
								this.pages.splice(0, this.pages.length);
								this.selectedPage = '1';
								for (let index = 1; index < mCalculatedPages + 1; index++) {
									this.pages.push(index);
								}
								this.totalPages = mCalculatedPages;
							}
						} else {
							this.pages.splice(0, this.pages.length);
							this.selectedPage = '1';
							this.totalPages = 0;
						}
					}
				} else {
					this.totalPages = 0;
				}
			}			
		});
	}

	/**
	 * Handles when the selectPage_{{name}} click event
	 *
	 * @memberof GWLibPagerComponent
	 */
	onGoToPageClick(): void {
		this._SearchCriteria.selectedPage = parseInt(this.selectedPage);
		this._SearchSvc.setSearchCriteria(this.name(), this._SearchCriteria);
	}

	/**
	 * Handles when there is a page change event
	 *
	 * @param {string} direction Valid values, First, Last, Next, Previous, or # (as a string so '1')
	 * @memberof DynamicTableComponent
	 */
	onPageChange(direction: string): void {
		const value = direction.trim().toLowerCase();
		switch (value) {
		case 'first':
			if (this._SearchCriteria.selectedPage > 1) {
				this._SearchCriteria.selectedPage = 1;
				this._SearchSvc.setSearchCriteria(this.name(), this._SearchCriteria);
			}
			break;
		case 'last':
			if (this._SearchCriteria.selectedPage < this.totalPages) {
				this._SearchCriteria.selectedPage = this.totalPages;
				this._SearchSvc.setSearchCriteria(this.name(), this._SearchCriteria);
			}
			break;
		case 'next':
			if (this._SearchCriteria.selectedPage < this.totalPages) {
				this._SearchCriteria.selectedPage++;
				this._SearchSvc.setSearchCriteria(this.name(), this._SearchCriteria);
			}
			break;
		case 'previous':
			if (this._SearchCriteria.selectedPage > 1) {
				this._SearchCriteria.selectedPage--;
				this._SearchSvc.setSearchCriteria(this.name(), this._SearchCriteria);
			}
			break;
		default:
			if (Number(value)) {
				console.log(value);
			} else {
				throw '"' + value + '" is not supported';
			}
			break;
		}
		this.selectedPage = this._SearchCriteria.selectedPage.toString();
	}
}
