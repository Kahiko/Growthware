import { Component, Input, ViewChild } from '@angular/core';
import { AfterViewInit, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { PagerComponent } from '@Growthware/Lib/src/lib/features/pager';
import { IDynamicTableColumn, IDynamicTableConfiguration, ISearchResultsNVP } from '@Growthware/Lib/src/lib/models';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { DataService, DynamicTableService } from '@Growthware/Lib/src/lib/services';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class DynamicTableComponent implements AfterViewInit, OnDestroy, OnInit {
  private _Subscriptions: Subscription = new Subscription();;

  @Input() configurationName: string = '';
  @ViewChild('pager', { static: false }) pagerComponent: PagerComponent;

  public activeRow: number = -1;
  public recordsPerPageSubject: Subject<number> = new Subject<number>();
  public tableConfiguration: IDynamicTableConfiguration;
  public tableData: any[] = [];

  public tableWidth: number = 200;
  public tableHeight: number = 206;
  public totalRecords: number = -1;
  public txtRecordsPerPage: number = 0;

  constructor(
    private _GWCommon: GWCommon,
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
  ) { }

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
      !this._GWCommon.isNullorEmpty(this.configurationName)
    ) {
      this.tableConfiguration = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
      if (!this._GWCommon.isNullOrUndefined(this.tableConfiguration)) {
        this.txtRecordsPerPage = this.tableConfiguration.numberOfRows;
        let mColumns = '';
        let mWidth: number = 0;
        this.tableConfiguration.columns.forEach((column: IDynamicTableColumn) => {
          mColumns += '[' + column.name + '], ';
          mWidth += +column.width;
        });
        mColumns = mColumns.substring(0, mColumns.length - 2);
        this.tableWidth = mWidth;
        this.tableHeight = this.tableConfiguration.tableHeight;
      }
      this._Subscriptions.add(
        this._DataSvc.dataChanged.subscribe((results: ISearchResultsNVP) => {
          if(this.configurationName.trim().toLowerCase() === results.name.trim().toLowerCase()) {
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
    this._Subscriptions.add(
      this.recordsPerPageSubject
      .pipe(debounceTime(800), distinctUntilChanged())
      .subscribe((newText) => {
        if (!this._GWCommon.isNullOrUndefined(newText) && !this._GWCommon.isNullorEmpty(newText.toString())) {
          console.log(newText);
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
}
