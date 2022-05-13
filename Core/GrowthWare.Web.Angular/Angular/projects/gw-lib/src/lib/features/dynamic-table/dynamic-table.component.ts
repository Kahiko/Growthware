import { Component, Input, OnDestroy } from '@angular/core';
import { OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Subscription } from 'rxjs';

import { Common } from 'projects/gw-lib/src/public-api'
import { GWLibDynamicTableService } from './dynamic-table.service';
import { IDynamicTableConfiguration } from './dynamic-table.interfaces';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class GWLibDynamicTableComponent implements OnInit, OnDestroy {
  private _DynamicTableSvc: GWLibDynamicTableService;
  private _TableDataSub: Subscription;

  @Input() ConfigurationName: string;

  public tableConfiguration: IDynamicTableConfiguration;
  public tableData: any[];

  constructor(tableSvc: GWLibDynamicTableService) {
    this._DynamicTableSvc = tableSvc;
  }

  formatData(data: any, type: string) {
    return Common.formatData(data, type);
  }

  ngOnDestroy(): void {
    this._TableDataSub.unsubscribe();
  }

  ngOnInit(): void {
    // console.log('DynamicTableComponent.ngOnInit');
    this._TableDataSub = this._DynamicTableSvc.dataChanged.subscribe({
      next: (name) => {
        if(this.ConfigurationName.toLowerCase() === name.toLowerCase()) {
          this.tableData = this._DynamicTableSvc.getData(this.ConfigurationName);
        }
      },
      error: (e) => console.error(e)
    });
    this.tableConfiguration = this._DynamicTableSvc.getTableConfiguration(this.ConfigurationName);
  }

}
