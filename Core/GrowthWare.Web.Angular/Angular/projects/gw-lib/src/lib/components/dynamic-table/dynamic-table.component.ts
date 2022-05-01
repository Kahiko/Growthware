import { Component, OnInit } from '@angular/core';
import { DynamicTableService } from './dynamic-table.service';

@Component({
  selector: 'gw-lib-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class DynamicTableComponent implements OnInit {
  private _DynamicTableSvc: DynamicTableService;

  constructor(dynamicTableSvc: DynamicTableService) {
    this._DynamicTableSvc = dynamicTableSvc;
  }

  ngOnInit(): void {
  }

}
