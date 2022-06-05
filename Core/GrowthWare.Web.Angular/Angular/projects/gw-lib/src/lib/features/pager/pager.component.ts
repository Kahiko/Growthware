import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { SearchCriteria, SearchService } from '../../services/search.service';
import { DynamicTableService, IResults } from '../dynamic-table/dynamic-table.service';

import { GWCommon } from '../../services/common';

@Component({
  selector: 'gw-lib-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnDestroy, OnInit {
  private _DataChangedSub: Subscription = new Subscription();

  @Input() name: string = '';

  public pages: number[] = [];
  public selectedPage: string = "1";
  public totalPages: number = 0;

  constructor(
    private _GWCommon: GWCommon,
    private _DynamicTableSvc: DynamicTableService,
    private _SearchSvc: SearchService) { }

  ngOnDestroy(): void {
    this._DataChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    this._DataChangedSub = this._DynamicTableSvc.dataChanged.subscribe((results: IResults) => {
      const mFirstRow = results.data[0];
      if(results.name.trim().toLowerCase() == this.name.trim().toLowerCase()) {
        if(!this._GWCommon.isNullOrUndefined(mFirstRow)) {
          const mSearchCriteria: SearchCriteria = this._SearchSvc.getSearchCriteria(this.name);
          if(!this._GWCommon.isNullOrUndefined(mSearchCriteria)) {
            const mTotalRecords: number = parseInt(mFirstRow['TotalRecords']);
            const mCalculatedPages: number = Math.floor(mTotalRecords / mSearchCriteria.pageSize);
            if(this.pages.length !== mCalculatedPages) {
              this.pages.splice(this.pages.length-1, 1);
              for (let mIndex = 1; mIndex < (mCalculatedPages + 1); mIndex++) {
                this.pages.push(mIndex);
              }
              this.totalPages = mCalculatedPages;
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
    const mSearchCriteria: SearchCriteria = this._SearchSvc.getSearchCriteria(this.name);
    mSearchCriteria.selectedPage = parseInt(this.selectedPage);
    this._SearchSvc.setSearchCriteria(this.name, mSearchCriteria);
  }

  /**
   * Handels when there is a page change event
   *
   * @param {string} direction Valid values, First, Last, Next, Previous, or # (as a string so '1')
   * @memberof DynamicTableComponent
   */
  onPageChange(direction: string): void {
    const value = direction.trim().toLowerCase();
    const mSearchCriteria: SearchCriteria = this._SearchSvc.getSearchCriteria(this.name);
    switch (value) {
      case "first":
        if(mSearchCriteria.selectedPage > 1) {
          mSearchCriteria.selectedPage = 1;
          this._SearchSvc.setSearchCriteria(this.name, mSearchCriteria);
        }
        break;
      case "last":
        if(mSearchCriteria.selectedPage < this.totalPages) {
          mSearchCriteria.selectedPage = this.totalPages;
          this._SearchSvc.setSearchCriteria(this.name, mSearchCriteria);
        }
        break;
      case "next":
        if(mSearchCriteria.selectedPage < this.totalPages) {
          mSearchCriteria.selectedPage++;
          this._SearchSvc.setSearchCriteria(this.name, mSearchCriteria);
        }
        break;
      case "previous":
        if(mSearchCriteria.selectedPage > 1) {
          mSearchCriteria.selectedPage--;
          this._SearchSvc.setSearchCriteria(this.name, mSearchCriteria);
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
