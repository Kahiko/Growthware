import { Component, Input, OnDestroy } from '@angular/core';
import { OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { GWCommon } from 'projects/gw-lib/src/lib/common'
import { GWLibDynamicTableService } from './dynamic-table.service';
import { GWLibSearchService } from 'projects/gw-lib/src/lib/services/search.service';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';
import { SearchCriteria } from 'projects/gw-lib/src/lib/services/search.service';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class GWLibDynamicTableComponent implements OnInit, OnDestroy {
  private _SearchCriteria = new SearchCriteria("none, set", "[none]", "asc", 10, 1, "1=1");
  private _Subscriptions: Subscription;

  @Input() ConfigurationName: string;

  public tableConfiguration: IDynamicTableConfiguration;
  public tableData: any[];

  constructor(
    private _SearchSvc: GWLibSearchService,
    private _DynamicTableSvc: GWLibDynamicTableService) { }

  formatData(data: any, type: string) {
    return GWCommon.formatData(data, type);
  }

  /**
   * @description getData retrieves data from the GWLibSearchService
   *
   * @see GWLibSearchService.getResults
   * @memberof GWLibDynamicTableComponent
   * @summary The method is exposed as public and is indended to be overriden when
   * your data needs are not met by using the search service.  In this case your
   * component will need to:
   * 1.) Implement AfterViewInit and ViewChild from @angular/core
   * In your component.html:
   * 2.) Add a #<yourNameofChoice> on the gw-lib-dynamic-table tag
   *      Example: <gw-lib-dynamic-table ConfigurationName="Functions" #searchFunctions></gw-lib-dynamic-table>
   * In your component.ts:
   * 3.) Import the GWLibDynamicTableComponent
   * 4.) Get an instance of the xx
   *      Example: @ViewChild('searchFunctions', {static: false}) searchFunctionsComponent: GWLibDynamicTableComponent;
   * 5.) Override the getData method in your ngAfterViewInit
   *      Example:  this.searchFunctionsComponent.getData = () => { // your code here }
   * 6.) Call the GWLibDynamicTableService.requestData method
   * 7.) Setup a subscription to GWLibDynamicTableService.dataRequested
   *      1.) Get your data
   *      2.) Call the GWLibDynamicTableService.setData method
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
      this.tableData = results;
    }).catch((error) => {
      console.log(error);
    });
  }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscriptions = new Subscription();
    this._Subscriptions.add(
      // needed to suport when this.getData has been overridden
      this._DynamicTableSvc.dataChanged.subscribe({
        next: (name) => {
          if(this.ConfigurationName.toLowerCase() === name.toLowerCase()) {
            this.tableData = this._DynamicTableSvc.getData(this.ConfigurationName);
          }
        },
        error: (e) => {console.error(e)}
      })
    );
    this._Subscriptions.add(
      // needed to suport when this.getData has been overridden will be called by
      // the overriding component after it has overriden getData
      this._DynamicTableSvc.dataRequested.subscribe({
        next: (name) => {
          if(this.ConfigurationName.toLowerCase() === name.toLowerCase()) {
            this.getData();
          };
        },
        error: (e) => {console.error(e)}
      })
    );

    this._Subscriptions.add(
      // needed to support when the searchCriteria has changed by an outside process
      // Example: GWLibPagerComponent
      this._SearchSvc.searchCriteriaChanged.subscribe({
        next: (name) => {
          if(name.trim().toLowerCase() === this.ConfigurationName.trim().toLowerCase()) {
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
      if(!this.tableConfiguration.overridingGetData) {
        this.getData();
      }
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
