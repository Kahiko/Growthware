import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent } from '@Growthware/src/features/dynamic-table';
// import { DynamicTableService, DynamicTableBtnMethods } from '@Growthware/src/features/dynamic-table';
// import { DataService } from '@Growthware/src/shared/services';
// import { SearchService, SearchCriteriaNVP } from '@Growthware/src/features/search';
// import { INameValuePare } from '@Growthware/src/shared/models';
// import { ModalService, IModalOptions, ModalOptions, ModalSize } from '@Growthware/src/features/modal';

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
