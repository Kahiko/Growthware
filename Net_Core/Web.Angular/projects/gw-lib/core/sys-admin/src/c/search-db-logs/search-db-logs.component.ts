import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableService, DynamicTableComponent } from '@growthware/core/dynamic-table';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { DBLogDetailsComponent } from '../db-log-details/db-log-details.component';
import { SysAdminService } from '../../sys-admin.service';

@Component({
  selector: 'gw-core-search-db-logs',
  standalone: true,
  imports: [
    DynamicTableComponent
  ],
  templateUrl: './search-db-logs.component.html',
  styleUrl: './search-db-logs.component.scss'
})
export class SearchDBLogsComponent extends BaseSearchComponent {

  constructor(
    theFeatureSvc: SysAdminService,
    dynamicTableSvc: DynamicTableService,
    modalSvc: ModalService,
    searchSvc: SearchService,
  ) {
    super();
    this.configurationName = 'DBLogs';
    this._TheFeatureName = 'DBLogs';
    this._TheApi = 'GrowthwareAPI/SearchDBLogs';
    this._TheComponent = DBLogDetailsComponent;
    this._TheWindowSize = new WindowSize(565, 950);
    this._TheService = theFeatureSvc;
    this._DynamicTableSvc = dynamicTableSvc;
    this._ModalSvc = modalSvc;
    this._SearchSvc = searchSvc;
  }
}
