import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { SearchCriteria, ISearchResultsNVP, SearchCriteriaNVP } from '@Growthware/Lib/src/lib/models';
import { DataService, DynamicTableService, SearchService } from '@Growthware/Lib/src/lib/services';

@Component({
  selector: 'gw-lib-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnDestroy, OnInit {
  private _DataChangedSub: Subscription = new Subscription();
  private _SearchCriteria: SearchCriteria;

  @Input() name: string = '';

  public pages: number[] = [];
  public selectedPage: string = "1";
  public totalPages: number = 0;

  constructor(
    private _GWCommon: GWCommon,
    private _DataSvc: DataService,
    private _SearchSvc: SearchService
  ) { }

  ngOnDestroy(): void {
    this._DataChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    this._DataChangedSub = this._DataSvc.dataChanged.subscribe((results: ISearchResultsNVP) => {
      if(this.name.trim().toLowerCase() === results.name.trim().toLowerCase()) {
        this._SearchCriteria = results.payLoad.searchCriteria;
        const mFirstRow = results.payLoad.data[0];
        if(!this._GWCommon.isNullOrUndefined(mFirstRow)) {
          const mTotalRecords: number = parseInt(mFirstRow['TotalRecords']);
          if(mTotalRecords > results.payLoad.searchCriteria.pageSize) {
            const mPageSize: number = results.payLoad.searchCriteria.pageSize;
            const mCalculatedPages: number = Math.floor(mTotalRecords / mPageSize);
            if(this.totalPages !== mCalculatedPages) {
              this.pages.splice(this.pages.length-1, 1);
              for (let index = 1; index < mCalculatedPages + 1; index++) {
                this.pages.push(index);
              }
              this.totalPages = mCalculatedPages
            }
          }
        }
      }
    });
  }

  /**
   * Handels when the selectPage_{{name}} click event
   *
   * @memberof GWLibPagerComponent
   */
  onGoToPageClick(): void {
    this._SearchCriteria.selectedPage = parseInt(this.selectedPage);
    const mChangedCriteria = new SearchCriteriaNVP(this.name, this._SearchCriteria);
    this._SearchSvc.setSearchCriteria(mChangedCriteria);
  }

  /**
   * Handels when there is a page change event
   *
   * @param {string} direction Valid values, First, Last, Next, Previous, or # (as a string so '1')
   * @memberof DynamicTableComponent
   */
  onPageChange(direction: string): void {
    const value = direction.trim().toLowerCase();
    const mSearchCriteria: SearchCriteria = this._SearchCriteria;
    const mChangedCriteria = new SearchCriteriaNVP(this.name, mSearchCriteria);
    switch (value) {
      case "first":
        if(mSearchCriteria.selectedPage > 1) {
          mSearchCriteria.selectedPage = 1;
          mChangedCriteria.payLoad = mSearchCriteria;
          this._SearchSvc.setSearchCriteria(mChangedCriteria);
        }
        break;
      case "last":
        if(mSearchCriteria.selectedPage < this.totalPages) {
          mSearchCriteria.selectedPage = this.totalPages;
          mChangedCriteria.payLoad = mSearchCriteria;
          this._SearchSvc.setSearchCriteria(mChangedCriteria);
        }
        break;
      case "next":
        if(mSearchCriteria.selectedPage < this.totalPages) {
          mSearchCriteria.selectedPage++;
          mChangedCriteria.payLoad = mSearchCriteria;
          this._SearchSvc.setSearchCriteria(mChangedCriteria);
        }
        break;
      case "previous":
        if(mSearchCriteria.selectedPage > 1) {
          mSearchCriteria.selectedPage--;
          mChangedCriteria.payLoad = mSearchCriteria;
          this._SearchSvc.setSearchCriteria(mChangedCriteria);
        }
        break;
      default:
        if(Number(value)) {
          console.log(value);
        } else {
          throw('"' + value + '" is not supported');
        }
        break;
    }
    this.selectedPage = mSearchCriteria.selectedPage.toString();
  }

}
