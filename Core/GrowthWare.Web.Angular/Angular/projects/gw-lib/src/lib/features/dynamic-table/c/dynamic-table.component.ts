import { Component, Input, ViewChild } from '@angular/core';
import { AfterViewInit, OnDestroy, OnInit } from '@angular/core';
import { TemplateRef } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

// Library Imports
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
// Interfaces/Models
import { IDynamicTableColumn, IDynamicTableConfiguration, ISearchCriteria } from '@Growthware/Lib/src/lib/models';
import { DynamicTableBtnMethods, SearchCriteria, SearchCriteriaNVP } from '@Growthware/Lib/src/lib/models';
import { ISearchResultsNVP } from '@Growthware/Lib/src/lib/models';
// Features (Components/Interfaces/Models/Services)
import { PagerComponent } from '@Growthware/Lib/src/lib/features/pager';
import { DataService, DynamicTableService, SearchService } from '@Growthware/Lib/src/lib/services';
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
  private _SearchCriteria!: SearchCriteria;
  private _Subscriptions: Subscription = new Subscription();

  @Input() configurationName: string = '';
  @ViewChild('pager', { static: false }) pagerComponent!: PagerComponent;
  @ViewChild('helpTemplate', { read: TemplateRef }) helpTemplate!:TemplateRef<any>;

  public activeRow: number = -1;
  public recordsPerPageSubject: Subject<number> = new Subject<number>();
  public recordsPerPageMsg: string = '';
  public searchTextSubject: Subject<string> = new Subject<string>();
  public searchText: string = '';
  public showHelp: boolean = true;
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
    private _ModalSvc: ModalService,
    private _SearchSvc: SearchService
  ) {}

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
      this._SearchCriteria = this._SearchSvc.getSearchCriteriaFromConfig(this.configurationName).payLoad;
      this.tableConfiguration = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
      if (!this._GWCommon.isNullOrUndefined(this.tableConfiguration)) {
        this.showHelp = this.tableConfiguration.showHelp;
        this.txtRecordsPerPage = this.tableConfiguration.numberOfRows;
        let mWidth: number = 0;
        this.tableConfiguration.columns.forEach((column: IDynamicTableColumn) => {
            mWidth += +column.width;
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
            this.tableData = results.payLoad.data;
            // get the "TotalRecords" column from the first row and update the local totalRecords
            if(this.tableData[0]) {
              const mFirstRow = this.tableData[0];
              if (!this._GWCommon.isNullOrUndefined(mFirstRow)) {
                this.totalRecords = parseInt(mFirstRow['TotalRecords']);
              }
            }
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
            const mSearchCriteria: ISearchCriteria = {...this._SearchCriteria};
            mSearchCriteria.pageSize = +newText;
            mSearchCriteria.selectedPage = 1;
            this.setSearchCriteria(mSearchCriteria);
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
          const mSearchCriteria: ISearchCriteria = {...this._SearchCriteria};
          mSearchCriteria.searchText = newText;
          mSearchCriteria.selectedPage = 1;
          this.setSearchCriteria(mSearchCriteria)
        })
    );
  }

  /**
   * Handles the table > tbody > tr click event
   * Sets the active row
   *
   * @param {number} rowNumber
   * @memberof DynamicTableComponent
   */
  public onRowClick(rowNumber: number) {
      this._RowClickCount++;
      setTimeout(() => {
          if (this._RowClickCount === 1) {
            // single
            if (this.activeRow !== rowNumber) {
            this.activeRow = rowNumber;
          } else {
            this.activeRow = -1;
          }
        } else if (this._RowClickCount === 2) {
          // double
          if (this.activeRow !== rowNumber) {
            this.activeRow = rowNumber;
          } else {
            this.activeRow = -1;
          }
          console.log('row double click');
        }
        this._RowClickCount = 0;
      }, 250)
  }
  public onSearchClick(columnName: string, event: any):void {
    // event.preventDefault();
    // event.stopPropagation();
    const mSearchCriteria: ISearchCriteria = {...this._SearchCriteria};
    let mColumns: Array<string> = this.getColumnArray(columnName, 'search');
    if(event.target && !event.target.checked) {
      mColumns = mColumns.filter(obj => { return obj !== columnName; });
    }
    mSearchCriteria.searchColumns = mColumns;
    this.setSearchCriteria(mSearchCriteria);
  }

  /**
   * Handles sort change event.  Happens when clicking the column and or the sort direction "icon"
   *
   * @param {string} columnName
   * @memberof DynamicTableComponent
   */
  public onSortChange(columnName: string): void {
    const mSearchCriteria: ISearchCriteria = {...this._SearchCriteria};
    mSearchCriteria.sortColumns = this.getColumnArray(columnName, 'sort');
    this.setSearchCriteria(mSearchCriteria);
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
   * Creates a new SearchCriteriaNVP and calls this._SearchSvc.setSearchCriteria
   *
   * @private
   * @memberof DynamicTableComponent
   */
  private setSearchCriteria(searchCriteria: ISearchCriteria): void {
    const mChangedCriteria = new SearchCriteriaNVP(
      this.configurationName,
      searchCriteria
    );
    this._SearchSvc.setSearchCriteria(mChangedCriteria);
  }

  /**
   * Handles the top left button click.
   * This method should be overwritten, it's up to the parent
   * component to implement any logic
   *
   * First item in the button array
   * @memberof DynamicTableComponent
   */
  public onTopLeft(): void {
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
  public onTopRight(): void {
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
  public onBottomLeft(): void {
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
  public onBottomRight(): void {
    this._LoggingSvc.toast('You have not set the onBottomRight call back method using the setButtonMethods', 'DynamicTableComponent', LogLevel.Error);
  }

  public onHelp() {
    const mModalOptions: ModalOptions = new ModalOptions('DynamicTableComponent.onHelp', 'Help', this.helpTemplate, new WindowSize(400, 600));
    this._ModalSvc.open(mModalOptions);
  }
}
