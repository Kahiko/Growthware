import { Component, Input, ViewChild } from '@angular/core';
import { AfterViewInit, OnDestroy, OnInit } from '@angular/core';
import { TemplateRef } from '@angular/core';
import { BehaviorSubject, Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

// Library Imports
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
// Interfaces/Models
import { IDynamicTableColumn, IDynamicTableConfiguration, DynamicTableBtnMethods } from '@Growthware/Lib/src/lib/models';
import { CallbackMethod } from '@Growthware/Lib/src/lib/models';

// Features (Components/Interfaces/Models/Services)
import { PagerComponent } from '@Growthware/Lib/src/lib/features/pager';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { SearchService, ISearchResultsNVP, SearchCriteria } from '@Growthware/Lib/src/lib/features/search';
import { ISearchCriteria } from '@Growthware/Lib/src/lib/features/search';
import { DynamicTableService } from '../dynamic-table.service';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss'],
})
export class DynamicTableComponent implements AfterViewInit, OnDestroy, OnInit {
  private _RowClickCount = 0;
  private _OnRowClickCallBackMethod?: CallbackMethod;
  private _OnRowDoubleClickCallbackMethod?: CallbackMethod;
  private _SearchCriteria!: SearchCriteria;
  private _Subscriptions: Subscription = new Subscription();
  private _TableDataSubject = new BehaviorSubject<any[]>([]);
  private _TableData: any[] = [];

  @Input() configurationName: string = '';
  @ViewChild('pager', { static: false }) pagerComponent!: PagerComponent;
  @ViewChild('helpTemplate', { read: TemplateRef }) helpTemplate!:TemplateRef<any>;

  activeRow: number = -1;
  maxHeadHeight: number = 32;
  recordsPerPageSubject: Subject<number> = new Subject<number>();
  recordsPerPageMsg: string = '';
  rowHeight: number = 10;
  searchTextSubject: Subject<string> = new Subject<string>();
  searchText: string = '';
  showHelp: boolean = true;
  tableConfiguration!: IDynamicTableConfiguration;
  readonly tableData = this._TableDataSubject.asObservable();
  tableWidth: number = 200;
  tableHeight: number = 206;
  totalRecords: number = -1;
  txtRecordsPerPage: number = 0;

  public getRowData(rowNumber: number) {
    return this._TableData[rowNumber];
  }

  /**
   * Use to set your method for the table row click event.
   *
   * @memberof DynamicTableComponent
   */
  public set rowClickBackMethod(callbackMethod: CallbackMethod) {
    if(callbackMethod && this._GWCommon.isFunction(callbackMethod)) {
      this._OnRowClickCallBackMethod = callbackMethod;
    }
  }

  /**
   * Use to set your method for the table row double click event.
   *
   * @memberof DynamicTableComponent
   */
  public set rowDoubleClickBackMethod(callbackMethod: CallbackMethod) {
    if(callbackMethod && this._GWCommon.isFunction(callbackMethod)) {
      this._OnRowDoubleClickCallbackMethod = callbackMethod;
    }
  }

  constructor(
    private _GWCommon: GWCommon,
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _SearchSvc: SearchService
  ) {}

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
   public setButtonMethods(dynamicTableBtnMethods: DynamicTableBtnMethods) {
    if (this._GWCommon.isFunction(dynamicTableBtnMethods.btnTopLeftCallBackMethod)) {
      this.onTopLeft = dynamicTableBtnMethods.btnTopLeftCallBackMethod;
    }
    if (this._GWCommon.isFunction(dynamicTableBtnMethods.btnTopRightCallBackMethod)) {
      this.onTopRight = dynamicTableBtnMethods.btnTopRightCallBackMethod;
    }
    if (this._GWCommon.isFunction(dynamicTableBtnMethods.btnBottomLeftCallBackMethod)) {
      this.onBottomLeft = dynamicTableBtnMethods.btnBottomLeftCallBackMethod;
    }
    if (this._GWCommon.isFunction(dynamicTableBtnMethods.btnBottomRightCallBackMethod)) {
      this.onBottomRight = dynamicTableBtnMethods.btnBottomRightCallBackMethod;
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

  /**
   * Returns an Array<string> where the string is either all of the column
   * name that have searchSelected or the colum name=direction:
   * 'name' or 'name=asc'.
   * this.tableConfiguration.columns properties are also changed.
   *
   * @private
   * @param {string} columnName
   * @param {('sort' | 'search')} columnType
   * @return {*}  {Array<string>}
   * @memberof DynamicTableComponent
   */
  private getColumnArray(columnName: string, columnType: 'sort' | 'search'): Array<string> {
    const mRetVal: Array<string> = [];
    let mColumnInfo = '';
    this.tableConfiguration.columns.forEach((element, index) => {
      if (element.name !== columnName) {
        this.tableConfiguration.columns[index].sortSelected = false;
        if(this.tableConfiguration.columns[index].searchSelected) {
          mColumnInfo = this.tableConfiguration.columns[index].name;
          mRetVal.push(mColumnInfo);
        }
      } else {
        if(columnType === 'sort') {
          this.tableConfiguration.columns[index].sortSelected = true;
          this.tableConfiguration.columns[index].direction = this.tableConfiguration.columns[index].direction === 'asc' ? 'desc':'asc';
          mColumnInfo = columnName + '=' + this.tableConfiguration.columns[index].direction;
        } else {
          this.tableConfiguration.columns[index].searchSelected = true;
          mColumnInfo = columnName;
        }
        mRetVal.push(mColumnInfo);
      }
    });
    return mRetVal;
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
    if (!this._GWCommon.isNullOrUndefined(this.configurationName) && !this._GWCommon.isNullOrEmpty(this.configurationName)) {
      this.tableConfiguration = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
      if (!this._GWCommon.isNullOrUndefined(this.tableConfiguration)) {
        this.maxHeadHeight = this.tableConfiguration.maxHeadHeight;
        this.rowHeight = this.tableConfiguration.maxTableRowHeight;
        this._SearchCriteria = this._SearchSvc.getSearchCriteriaFromConfig(this.configurationName, this.tableConfiguration).payLoad;
        this.showHelp = this.tableConfiguration.showHelp;
        this.txtRecordsPerPage = this.tableConfiguration.numberOfRows;
        let mWidth: number = 0;
        this.tableConfiguration.columns.forEach((column: IDynamicTableColumn) => {
            mWidth += +column.width * 1.4;
          }
        );
        this.tableWidth = mWidth;
        this.tableHeight = this.tableConfiguration.tableHeight;
      }
      // subscribe to the data change event and update the local data
      this._Subscriptions.add(
        this._DataSvc.dataChanged.subscribe((results: ISearchResultsNVP) => {
          if (this.configurationName.trim().toLowerCase() === results.name.trim().toLowerCase()) {
            // update the local search criteria with the one used to perform the search
            this._SearchCriteria = results.payLoad.searchCriteria;
            // update the local data
            this._TableData = results.payLoad.data;
            this._TableDataSubject.next(results.payLoad.data);
            // get the "TotalRecords" column if it exists from the first row and update the local totalRecords
            this.totalRecords = results.payLoad.totalRecords;
            // set the activeRow to -1 b/c if there was one selected it's no longer valid
            this.activeRow = -1;
          }
        })
      );
    } else {
      const mLogDestinations: Array<LogDestination> = [];
      mLogDestinations.push(LogDestination.Console);
      mLogDestinations.push(LogDestination.Toast);
      const mLogOptions: ILogOptions = new LogOptions(
        'DynamicTableComponent.ngOnInit: configurationName is blank',
        LogLevel.Error,
        mLogDestinations,
        'DynamicTableComponent',
        'DynamicTableComponent',
        'ngOnInit',
        'system',
        'DynamicTableComponent'
      )
      this._LoggingSvc.log(mLogOptions);
    }
    // subscribe to the recordsPerPage change event and update the search criteria
    this._Subscriptions.add(
      this.recordsPerPageSubject
        .pipe(debounceTime(500), distinctUntilChanged())
        .subscribe((newText) => {
          if (this._GWCommon.isNumber(newText) && +newText > 0) {
            this.recordsPerPageMsg = '';
            const mSearchCriteria: ISearchCriteria = {
              ...this._SearchCriteria
              , pageSize: +newText
              , selectedPage: 1
              };
            this._SearchSvc.setSearchCriteria(this.configurationName, mSearchCriteria);
          } else {
            this.recordsPerPageMsg =
              'Value must be numeric and greater than zero!';
          }
        })
    );
    this._Subscriptions.add(
      this.searchTextSubject
        .pipe(debounceTime(500), distinctUntilChanged())
        .subscribe((newText) => {
          const mSearchCriteria: ISearchCriteria = {
            ...this._SearchCriteria
            , searchText: newText
            , selectedPage: 1
          };
          this._SearchSvc.setSearchCriteria(this.configurationName, mSearchCriteria);
        })
    );
  }

  /**
   * Handles the table > tbody > tr click event
   * Sets the active row and calls this._OnRowClickCallBackMethod
   *
   * Use the set rowClickBackMethod property to use a desired callback method
   * @param {number} rowNumber
   * @memberof DynamicTableComponent
   */
  onRowClick(rowNumber: number) {
      this._RowClickCount++;
      setTimeout(() => {
        if (this._RowClickCount === 1) {
          // single
          if(this._OnRowClickCallBackMethod && this._GWCommon.isFunction(this._OnRowClickCallBackMethod)) {
            this._OnRowClickCallBackMethod(rowNumber);
          }
          if (this.activeRow !== rowNumber) {
            this.activeRow = rowNumber;
          } else {
            this.activeRow = -1;
          }
        } else if (this._RowClickCount === 2) {
          // double
          this.onRowDoubleClick(rowNumber);
        }
        this._RowClickCount = 0;
      }, 250)
  }

  /**
   * Handles the table > tbody > tr dblclick event
   * Sets the active row and calls this._OnRowDoubleClickCallbackMethod
   *
   * Use the set rowDoubleClickBackMethod property to use a desired callback method
   * @param {number} rowNumber
   * @memberof DynamicTableComponent
   */
  onRowDoubleClick(rowNumber: number) {
    if (this.activeRow !== rowNumber) {
      this.activeRow = rowNumber;
    }
    if(this._OnRowDoubleClickCallbackMethod && this._GWCommon.isFunction(this._OnRowDoubleClickCallbackMethod)) {
      this._OnRowDoubleClickCallbackMethod(rowNumber);
    }
  }

  /**
   * Handles a columns checkbox click event.
   * @param { string } columnName
   * @param { any } event The $event of the HTML object
   */
  onSearchClick(columnName: string, event: any):void {
    let mColumns: Array<string> = this.getColumnArray(columnName, 'search');
    if(event.target && !event.target.checked) {
      mColumns = mColumns.filter(obj => { return obj !== columnName; });
    }
    const mSearchCriteria: ISearchCriteria = {
      ...this._SearchCriteria
      , searchColumns: mColumns
    };
    this._SearchSvc.setSearchCriteria(this.configurationName, mSearchCriteria);
  }

  /**
   * Handles sort change event.  Happens when clicking the column and or the sort direction "icon"
   *
   * @param {string} columnName
   * @memberof DynamicTableComponent
   */
  onSortChange(columnName: string): void {
    const mSearchCriteria: ISearchCriteria = {
      ...this._SearchCriteria
      , sortColumns: this.getColumnArray(columnName, 'sort')
    };
    this._SearchSvc.setSearchCriteria(this.configurationName, mSearchCriteria);
  }

  public setRowClickMethod(): void {

  }

  /**
   * Handles the top left button click.
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * First item in the button array
   * @memberof DynamicTableComponent
   */
  onTopLeft(): void {
    this._LoggingSvc.toast('You have not set the onTopLeft call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  /**
   * Handles the top right button click
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * Second item in the button array
   * @memberof DynamicTableComponent
   */
  onTopRight(): void {
    this._LoggingSvc.toast('You have not set the onTopRight call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  /**
   * Handles the button left button click
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * Third item in the button array
   * @memberof DynamicTableComponent
   */
  onBottomLeft(): void {
    this._LoggingSvc.toast('You have not set the onBottomLeft call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  /**
   * Handles the bottom right button click
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * Fourth item in the button array
   * @memberof DynamicTableComponent
   */
  onBottomRight(): void {
    this._LoggingSvc.toast('You have not set the onBottomRight call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  onHelp() {
    const mModalOptions: ModalOptions = new ModalOptions('DynamicTableComponent.onHelp', 'Help', this.helpTemplate, new WindowSize(450, 600));
    this._ModalSvc.open(mModalOptions);
  }
}
