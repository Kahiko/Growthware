import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent, DynamicTableService } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { SearchService, SearchCriteriaNVP } from '@Growthware/Lib/src/lib/features/search';
import { DynamicTableBtnMethods, INameValuePare } from '@Growthware/Lib/src/lib/models';
import { ModalService, IModalOptions, ModalOptions, ModalSize } from '@Growthware/Lib/src/lib/features/modal';

@Component({
  selector: 'gw-lib-search-groups',
  templateUrl: './search-groups.component.html',
  styleUrls: ['./search-groups.component.scss']
})
export class SearchGroupsComponent implements OnInit {
  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  public configurationName = 'Groups';
  public results: any;

  constructor(
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
    private _ModalSvc: ModalService,
    private _SearchSvc: SearchService,
  ) { }

  ngOnInit(): void {
  }

}
