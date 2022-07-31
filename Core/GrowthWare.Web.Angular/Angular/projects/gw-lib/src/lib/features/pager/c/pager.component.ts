import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { SearchCriteria, SearchCriteriaNVP, ISearchResultsNVP } from '@Growthware/Lib/src/lib/features/search';
import { SearchService } from '@Growthware/Lib/src/lib/features/search';
import { DataService } from '@Growthware/Lib/src/lib/services';

@Component({
  selector: 'gw-lib-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss'],
})
export class PagerComponent implements OnDestroy, OnInit {
  private _DataChangedSub: Subscription = new Subscription();
  private _SearchCriteria: SearchCriteria = new SearchCriteria([''],[''],1,'',1);

  @Input() name: string = '';

  public pages: number[] = [];
  public selectedPage: string = '1';
  public totalPages: number = 0;

  constructor(
    private _GWCommon: GWCommon,
    private _DataSvc: DataService,
    private _SearchSvc: SearchService
  ) {}

  ngOnDestroy(): void {
    this._DataChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    this._DataChangedSub = this._DataSvc.dataChanged.subscribe(
      (results: ISearchResultsNVP) => {
        if (
          this.name.trim().toLowerCase() === results.name.trim().toLowerCase()
        ) {
          this._SearchCriteria = results.payLoad.searchCriteria;
          const mFirstRow = results.payLoad.data[0];
          if (!this._GWCommon.isNullOrUndefined(mFirstRow)) {
            const mTotalRecords: number = parseInt(mFirstRow['TotalRecords']);
            if (mTotalRecords > results.payLoad.searchCriteria.pageSize) {
              const mPageSize: number = results.payLoad.searchCriteria.pageSize;
              let mCalculatedPages: number = Math.floor(mTotalRecords / mPageSize);
              if(mTotalRecords%mCalculatedPages != 0) {
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
            }
          }
        }
      }
    );
  }

  /**
   * Handles when the selectPage_{{name}} click event
   *
   * @memberof GWLibPagerComponent
   */
  onGoToPageClick(): void {
    this._SearchCriteria.selectedPage = parseInt(this.selectedPage);
    this._SearchSvc.setSearchCriteria(this.name, this._SearchCriteria);
  }

  /**
   * Handels when there is a page change event
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
          this._SearchSvc.setSearchCriteria(this.name, this._SearchCriteria);
        }
        break;
      case 'last':
        if (this._SearchCriteria.selectedPage < this.totalPages) {
          this._SearchCriteria.selectedPage = this.totalPages;
          this._SearchSvc.setSearchCriteria(this.name, this._SearchCriteria);
        }
        break;
      case 'next':
        if (this._SearchCriteria.selectedPage < this.totalPages) {
          this._SearchCriteria.selectedPage++;
          this._SearchSvc.setSearchCriteria(this.name, this._SearchCriteria);
        }
        break;
      case 'previous':
        if (this._SearchCriteria.selectedPage > 1) {
          this._SearchCriteria.selectedPage--;
          this._SearchSvc.setSearchCriteria(this.name, this._SearchCriteria);
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
