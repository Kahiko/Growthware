import { Component, Input, ViewChild } from '@angular/core';
import { AfterViewInit, OnDestroy, OnInit } from '@angular/core';
import { Subscription, throwError } from 'rxjs';

import { IDynamicTableConfiguration } from './dynamic-table.interfaces';

import { PagerComponent } from '../pager/pager.component';

import { DynamicTableService, IResults } from './dynamic-table.service';
import { SearchCriteria, SearchService } from '../../services/search.service';

import { GWCommon } from '../../services/common';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss'],
})
export class DynamicTableComponent implements AfterViewInit, OnDestroy, OnInit {
  private _SearchCriteria = new SearchCriteria('none, set', '[none]', 'asc', 10, 1, '1=1');
  private _Subscriptions: Subscription = new Subscription();

  @Input() configurationName: string = '';
  @ViewChild('pager', { static: false }) pagerComponent: PagerComponent;

  public activeRow: number = 0;
  public tableConfiguration: IDynamicTableConfiguration;
  public tableData: any[] = [];

  public tableWidth: number = 200;
  public tableHeight: number = 206;

  constructor(
    private _GWCommon: GWCommon,
    private _DynamicTableSvc: DynamicTableService,
    private _SearchSvc: SearchService
  ) {}

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
      this._Subscriptions.add(
        // Suports when this.getData has been overridden
        this._DynamicTableSvc.dataChanged.subscribe({
          next: (results) => {
            if (
              this.configurationName.toLowerCase() ===
              results.name.trim().toLowerCase()
            ) {
              this.tableData = results.data;
            }
          },
          error: (e) => {
            console.error(e);
          },
        })
      );

      this._Subscriptions.add(
        // Supports when the searchCriteria has changed by an outside process
        // Example: PagerComponent
        this._SearchSvc.searchCriteriaChanged.subscribe({
          next: (name) => {
            if (
              name.trim().toLowerCase() ===
              this.configurationName.trim().toLowerCase()
            ) {
              this._SearchCriteria = this._SearchSvc.getSearchCriteria(name);
              this.getData();
            }
          },
        })
      );

      this.tableConfiguration = this._DynamicTableSvc.getTableConfiguration(
        this.configurationName
      );
      if (!this._GWCommon.isNullOrUndefined(this.tableConfiguration)) {
        let mColumns = '';
        let mWidth: number = 0;
        this.tableConfiguration.columns.forEach((column) => {
          mColumns += '[' + column.name + '], ';
          mWidth += +column.width;
        });
        mColumns = mColumns.substring(0, mColumns.length - 2);
        this.tableWidth = mWidth;
        this.tableHeight = this.tableConfiguration.tableHeight;
        // console.log(mWidth); // 6
        this._SearchCriteria.columns = mColumns;
        this._SearchCriteria.orderByColumn =
          this.tableConfiguration.orderByColumn;
        this._SearchCriteria.tableOrView = this.tableConfiguration.tableOrView;
        this._SearchSvc.setSearchCriteria(
          this.tableConfiguration.name,
          this._SearchCriteria
        );
      }
    } else {
      console.error(
        'DynamicTableComponent.ngOnInit: configurationName is blank'
      );
    }
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

  /**
   * @description getData retrieves data from the GWLibSearchService
   *
   * @see GWLibSearchService.getResults
   * @memberof DynamicTableComponent
   */
  public getData(): void {
    if (this._SearchCriteria.orderByColumn.indexOf('none') > 0 && this._GWCommon.isNullorEmpty(this._SearchCriteria.orderByColumn)) {
      throw ('The _SearchCriteria.orderByColumn has not been set for "' + this.configurationName + '"');
    }
    if (this._SearchCriteria.columns.length < 1) {
      throw 'this._SearchCriteria.columns must have at least 1 column defined!';
    }
    if (this._GWCommon.isNullorEmpty(this._SearchCriteria.tableOrView)) {
      throw 'this._SearchCriteria.tableOrView must have a value!';
    }
    this._SearchSvc
      .getResults(this._SearchCriteria)
      .then((results) => {
        if (!this._GWCommon.isNullOrUndefined(results) && results.length > 0) {
          this.tableData = results;
          if (results.length > 0) {
            this._DynamicTableSvc.setData(this.configurationName, results);
          }
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }
}
