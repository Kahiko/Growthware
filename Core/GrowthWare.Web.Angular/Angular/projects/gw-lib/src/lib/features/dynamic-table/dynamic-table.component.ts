import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { DynamicTableService } from './dynamic-table.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class DynamicTableComponent implements OnInit, OnDestroy {
  private _DynamicTableSvc: DynamicTableService;
  private _TableDataSub: Subscription;

  @Input() ConfigurationName: string;

  constructor(tableSvc: DynamicTableService) {
    this._DynamicTableSvc = tableSvc;
  }

  ngOnDestroy(): void {
    this._TableDataSub.unsubscribe();
  }

  ngOnInit(): void {
    console.log('DynamicTableComponent.ngOnInit');
    this._TableDataSub = this._DynamicTableSvc.dataChanged.subscribe({
      next: (v) => console.log(v),
      error: (e) => console.error(e),
      complete: () => console.info('complete')
  });
  }

}
