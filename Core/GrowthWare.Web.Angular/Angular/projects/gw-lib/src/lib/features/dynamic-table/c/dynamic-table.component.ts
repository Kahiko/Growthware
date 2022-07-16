import { Component, Input, ViewChild } from '@angular/core';
import { AfterViewInit, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { PagerComponent } from '@Growthware/Lib/src/lib/features/pager';
import { IDynamicTableColumn, IDynamicTableConfiguration, ISearchResultsNVP } from '@Growthware/Lib/src/lib/models';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { DataService, DynamicTableService, SearchService } from '@Growthware/Lib/src/lib/services';
import { DynamicTableBtnMethods, SearchCriteria, SearchCriteriaNVP } from '@Growthware/Lib/src/lib/models';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

interface ISortInfo {
  "columnName": string, "direction": string
}

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class DynamicTableComponent implements AfterViewInit, OnDestroy, OnInit {
  private _SortColumns: ISortInfo[] = []
  private _Subscriptions: Subscription = new Subscription();

  @Input() configurationName: string = '';
  @ViewChild('pager', { static: false }) pagerComponent!: PagerComponent;

  public activeRow: number = -1;
  public recordsPerPageSubject: Subject<number> = new Subject<number>();
  public recordsPerPageMsg: string = '';
  public searchCriteria!: SearchCriteria;
  public tableConfiguration!: IDynamicTableConfiguration;
  public tableData: any[] = [];
  public tableWidth: number = 200;
  public tableHeight: number = 206;
  public totalRecords: number = -1;
  public txtRecordsPerPage: number = 0;

  public closeCallBackMethod!: (arg?: any) => void;

  constructor(
    private _GWCommon: GWCommon,
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
    private _LoggingSvc: LoggingService,
    private _SearchSvc: SearchService,
  ) { }

  public changeSort(columnName: string): void {
    const mSortColumn = this._SortColumns.filter(x => x.columnName.toLocaleLowerCase() == columnName.toLocaleLowerCase())[0];
    if(!this._GWCommon.isNullOrUndefined(mSortColumn)) {
      mSortColumn.direction = ((mSortColumn.direction === 'asc') ? 'desc' : 'asc');
      this._GWCommon.addOrUpdateArray(this._SortColumns, mSortColumn);
    }
  }

  /**
   * Formats the data
   *
   * @see this._GWCommon.formatData
   * @param {*} data
   * @param {string} type
   * @return {*}
   * @memberof DynamicTableComponent
   */
   formatData(data: any, type: string): void {
    return this._GWCommon.formatData(data, type);
  }

  ngAfterViewInit(): void {
    if (this.pagerComponent) {
      this.pagerComponent.name = this.configurationName;
    }
  }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.configurationName = this.configurationName.trim();
    if (
      !this._GWCommon.isNullOrUndefined(this.configurationName) &&
      !this._GWCommon.isNullOrEmpty(this.configurationName)
    ) {
      this.tableConfiguration = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
      if (!this._GWCommon.isNullOrUndefined(this.tableConfiguration)) {
        this.txtRecordsPerPage = this.tableConfiguration.numberOfRows;
        let mWidth: number = 0;
        this.tableConfiguration.columns.forEach((column: IDynamicTableColumn) => {
          if(!this._GWCommon.isNullOrEmpty(this.tableConfiguration.orderByColumn) && this.tableConfiguration.orderByColumn.toLocaleLowerCase() === column.name.toLocaleLowerCase()) {
            const mSortColum: ISortInfo = { columnName: this.tableConfiguration.orderByColumn.trim() , direction: 'asc' };
            this._SortColumns.push(mSortColum);
          }
          mWidth += +column.width;
        });
        this.tableWidth = mWidth;
        this.tableHeight = this.tableConfiguration.tableHeight;
      }
      // subscribe to the data change event and update the local data
      this._Subscriptions.add(
        this._DataSvc.dataChanged.subscribe((results: ISearchResultsNVP) => {
          if(this.configurationName.trim().toLowerCase() === results.name.trim().toLowerCase()) {
            this.searchCriteria = results.payLoad.searchCriteria;
            this.tableData = results.payLoad.data;
            const mFirstRow = this.tableData[0];
            if(!this._GWCommon.isNullOrUndefined(mFirstRow)) {
              this.totalRecords = parseInt(mFirstRow['TotalRecords']);
            }
            this.activeRow = -1;
          }
        })
      );
    } else {
      console.error(
        'DynamicTableComponent.ngOnInit: configurationName is blank'
      );
    }
    // subscribe to the recordsPerPage change event and update the search criteria
    this._Subscriptions.add(
      this.recordsPerPageSubject
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe((newText) => {
        if (this._GWCommon.isNumber(newText) && +newText > 0) {
          this.recordsPerPageMsg = '';
          this.searchCriteria.pageSize = +newText;
          this.searchCriteria.selectedPage = 1;
          const mNewCriteria: SearchCriteriaNVP = new SearchCriteriaNVP(this.configurationName, this.searchCriteria);
          this._SearchSvc.setSearchCriteria(mNewCriteria);
        } else {
          this.recordsPerPageMsg = 'Value must be numeric and greater than zero!';
        }
      })
    )
  }

  /**
   * Handles the table > tbody > tr click event
   * Sets the active row
   *
   * @param {number} rowNumber
   * @memberof DynamicTableComponent
   */
  public onRowClick(rowNumber: number) {
    if (this.activeRow !== rowNumber) {
      this.activeRow = rowNumber;
    } else {
      this.activeRow = -1;
    }
  }

  /**
   * Sets the onTopLeft, onTopRight, onBottomLeft and onBottomRight methods with
   * the methods supplied.
   *
   * If the methods does not get set the default method behavior is to
   * alert indicating the method has not been set.
   *
   * @param {DynamicTableBtnMethods} dynamicTableBtnMethods
   * @memberof DynamicTableComponent
   */
  setButtonMethods(dynamicTableBtnMethods: DynamicTableBtnMethods) {
    if(this._GWCommon.isFunction(dynamicTableBtnMethods.btnTopLeftCallBackMethod)) {
      this.onTopLeft = dynamicTableBtnMethods.btnTopLeftCallBackMethod;
    }
    if(this._GWCommon.isFunction(dynamicTableBtnMethods.btnTopRightCallBackMethod)) {
      this.onTopRight = dynamicTableBtnMethods.btnTopRightCallBackMethod;
    }
    if(this._GWCommon.isFunction(dynamicTableBtnMethods.btnBottomLeftCallBackMethod)) {
      this.onBottomLeft = dynamicTableBtnMethods.btnBottomLeftCallBackMethod;
    }
    if(this._GWCommon.isFunction(dynamicTableBtnMethods.btnBottomRightCallBackMethod)) {
      this.onBottomRight = dynamicTableBtnMethods.btnBottomRightCallBackMethod;
    }
  }

  public showSort(columnName: string, direction: 'asc' | 'desc'): boolean {
    let mRetVal: boolean = false;
    const mSortColumn = this._SortColumns.filter(x => x.columnName.toLocaleLowerCase() == columnName.toLocaleLowerCase())[0];
    if(!this._GWCommon.isNullOrUndefined(mSortColumn)) {
      if(mSortColumn.direction.toLocaleLowerCase() === direction.toLocaleLowerCase()) {
        mRetVal = true;
      }
    }
    return mRetVal;
  }

  /**
   * Handles the top left button click.
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * @memberof DynamicTableComponent
   */
  public onTopLeft(): void {      // 0
    this._LoggingSvc.toast('You have not set the onTopLeft call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  /**
   * Handles the top right button click
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * @memberof DynamicTableComponent
   */
  public onTopRight(): void {     // 1
    this._LoggingSvc.toast('You have not set the onTopRight call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  /**
   * Handles the button left button click
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * @memberof DynamicTableComponent
   */
  public onBottomLeft(): void {   // 2
    this._LoggingSvc.toast('You have not set the onBottomLeft call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  /**
   * Handles the bottom right button click
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * @memberof DynamicTableComponent
   */
  public onBottomRight(): void {  // 3
    this._LoggingSvc.toast('You have not set the onBottomRight call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }
}
