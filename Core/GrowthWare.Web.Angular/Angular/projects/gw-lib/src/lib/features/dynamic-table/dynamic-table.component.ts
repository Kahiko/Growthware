import { Component, Input, OnDestroy } from '@angular/core';
import { AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';

import { GWLibPagerComponent } from 'projects/gw-lib/src/lib/features/pager/pager.component';
import { GWCommon } from 'projects/gw-lib/src/lib/common'
import { GWLibDynamicTableService } from './dynamic-table.service';
import { GWLibSearchService } from 'projects/gw-lib/src/lib/services/search.service';
import { GWLibPagerService } from '../pager/pager.service';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';
import { SearchCriteria } from 'projects/gw-lib/src/lib/services/search.service';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class GWLibDynamicTableComponent implements AfterViewInit, OnInit, OnDestroy {
  private _SearchCriteria = new SearchCriteria("none, set", "[none]", "asc", 10, 1, "1=1");
  private _Subscriptions: Subscription;

  @Input() ConfigurationName: string;
  @ViewChild('pager', {static: false}) pagerComponent: GWLibPagerComponent;

  public tableConfiguration: IDynamicTableConfiguration;
  public tableData: any[];

  constructor(
    private _SearchSvc: GWLibSearchService,
    private _DynamicTableSvc: GWLibDynamicTableService,
    private _pagerSvc: GWLibPagerService) { }

  formatData(data: any, type: string) {
    return GWCommon.formatData(data, type);
  }

  /**
   * @description getData retrieves data from the GWLibSearchService
   *
   * @see GWLibSearchService.getResults
   * @memberof GWLibDynamicTableComponent
   */
  public getData(): void {
    if(this._SearchCriteria.orderByColumn.indexOf('none') > 0 && GWCommon.isNullorEmpty(this._SearchCriteria.orderByColumn)) {
      throw('The _SearchCriteria.orderByColumn has not been set for "' + this.ConfigurationName + '"');
    }
    if(this._SearchCriteria.columns.length < 1) {
      throw('this._SearchCriteria.columns must have at least 1 column defined!');
    }
    if(GWCommon.isNullorEmpty(this._SearchCriteria.tableOrView)) {
      throw('this._SearchCriteria.tableOrView must have a value!');
    }
    this._SearchSvc.getResults(this._SearchCriteria).then((results) => {
      if(!GWCommon.isNullOrUndefined(results) && results.length > 0) {
        this.tableData = results;
        const mFirstRow = results[0];
        this._pagerSvc.setTotalNumberOfPages(this.ConfigurationName, parseInt(mFirstRow['TotalRecords']), this._SearchCriteria.pageSize);
      }
    }).catch((error) => {
      console.log(error);
    });
  }

  ngAfterViewInit(): void {
    this.pagerComponent.name = this.ConfigurationName;
  }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.ConfigurationName = this.ConfigurationName.trim();
    this._Subscriptions = new Subscription();
    this._Subscriptions.add(
      // Suports when this.getData has been overridden
      this._DynamicTableSvc.dataChanged.subscribe({
        next: (name) => {
          if(this.ConfigurationName.toLowerCase() === name.trim().toLowerCase()) {
            this.tableData = this._DynamicTableSvc.getData(this.ConfigurationName);
          }
        },
        error: (e) => {console.error(e)}
      })
    );

    this._Subscriptions.add(
      // Supports when the searchCriteria has changed by an outside process
      // Example: GWLibPagerComponent
      this._SearchSvc.searchCriteriaChanged.subscribe({
        next: (name) => {
          if(name.trim().toLowerCase() === this.ConfigurationName.toLowerCase()) {
            this._SearchCriteria = this._SearchSvc.getSearchCriteria(name);
            this.getData();
          }
        }
      })
    );

    this.tableConfiguration = this._DynamicTableSvc.getTableConfiguration(this.ConfigurationName);
    if(!GWCommon.isNullOrUndefined(this.tableConfiguration)) {
      let mColumns = '';
      this.tableConfiguration.columns.forEach(column => {
        mColumns += '[' + column.name + '], ';
      });
      mColumns = mColumns.substring(0, mColumns.length -2);
      this._SearchCriteria.columns = mColumns;
      this._SearchCriteria.orderByColumn = this.tableConfiguration.orderByColumn;
      this._SearchCriteria.tableOrView = this.tableConfiguration.tableOrView;
      this._SearchSvc.setSearchCriteria(this.tableConfiguration.name, this._SearchCriteria);
    }
  }

  /**
   * Handels when there is a page change even
   *
   * @param {string} direction Valid values, First, Last, Next, Previous, or # (as a string so '1')
   * @memberof GWLibDynamicTableComponent
   */
  onPageChange(direction: string): void {
    const value = direction.trim().toLowerCase();
    switch (value) {
      case "fist":
        this._SearchCriteria.selectedPage = 1;
        this.getData();
        break;
      case "last":
        // TODO: will need to grab the "" column from the data
        break;
      case "next":
        // TODO: need to add check if selectedPage is the at the last page already
        this._SearchCriteria.selectedPage++;
        this.getData();
        break;
      case "previous":
        if(this._SearchCriteria.selectedPage > 1) {
          this._SearchCriteria.selectedPage--;
          this.getData();
        }
        break;
      default:
        if(Number(value)) {
          this._SearchCriteria.selectedPage = +value;
          this.getData();
        } else {
          throw('"' + value + '" is not supported');
        }
        break;
    }
  }
}
