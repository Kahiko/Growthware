import { Component, Input, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { SearchCriteria, GWLibSearchService } from '../../services/search.service';
import { GWLibDynamicTableService } from '../dynamic-table/dynamic-table.service';

@Component({
  selector: 'gw-lib-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class GWLibPagerComponent implements OnInit {
  private _TotalPagesSub: Subscription;

  public pages: number[] = [];

  @Input() name: string;
  @Input() totalPages: number;

  constructor(private _DynamicTableSvc: GWLibDynamicTableService, private _SearchSvc: GWLibSearchService) { }

  ngOnInit(): void {
    this._TotalPagesSub = this._DynamicTableSvc.totalPagesChanged.subscribe({
      next: (name) => {
        this.totalPages = this._DynamicTableSvc.getTotalPages(name);
        for (let index = 1; index < this.totalPages + 1; index++) {
          this.pages.push(index);
        }
      },
      error: (e) => {console.error(e)}
    });
  }

  /**
   * Handels when there is a page change event
   *
   * @param {string} direction Valid values, First, Last, Next, Previous, or # (as a string so '1')
   * @memberof GWLibDynamicTableComponent
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
  }
}
