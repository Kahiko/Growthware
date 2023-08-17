import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent } from '@Growthware/src/features/dynamic-table';
// import { DynamicTableService } from '@Growthware/src/features/dynamic-table';
// import { DataService } from '@Growthware/src/shared/services';
// import { SearchService } from '@Growthware/src/features/search';
// import { ModalService } from '@Growthware/src/features/modal';


@Component({
  selector: 'gw-lib-search-roles',
  templateUrl: './search-roles.component.html',
  styleUrls: ['./search-roles.component.scss']
})
export class SearchRolesComponent implements OnDestroy, OnInit {
  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  public configurationName = 'Roles';
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
  }

}
