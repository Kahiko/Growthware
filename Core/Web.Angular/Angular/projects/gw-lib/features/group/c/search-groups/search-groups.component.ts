import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent } from '@Growthware/features/dynamic-table';
// import { DynamicTableService, DynamicTableBtnMethods } from '@Growthware/features/dynamic-table';
// import { DataService } from '@Growthware/shared/services';
// import { SearchService, SearchCriteriaNVP } from '@Growthware/features/search';
// import { INameValuePare } from '@Growthware/shared/models';
// import { ModalService, IModalOptions, ModalOptions, ModalSize } from '@Growthware/features/modal';

@Component({
  selector: 'gw-lib-search-groups',
  templateUrl: './search-groups.component.html',
  styleUrls: ['./search-groups.component.scss']
})
export class SearchGroupsComponent implements OnDestroy, OnInit {
  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  public configurationName = 'Groups';
  public results: any;

  constructor(
    // private _DataSvc: DataService,
    // private _DynamicTableSvc: DynamicTableService,
    // private _ModalSvc: ModalService,
    // private _SearchSvc: SearchService,
  ) { }

  ngOnDestroy(): void {
    this._SearchCriteriaChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    this._SearchCriteriaChangedSub.add(()=>{

    });
  }

}
