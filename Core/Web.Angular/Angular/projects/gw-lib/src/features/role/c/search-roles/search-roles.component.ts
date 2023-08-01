import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent, DynamicTableService } from '@Growthware/Lib/src/features/dynamic-table';
import { DataService } from '@Growthware/Lib/src/services';
import { SearchService } from '@Growthware/Lib/src/features/search';
import { ModalService } from '@Growthware/Lib/src/features/modal';


@Component({
  selector: 'gw-lib-search-roles',
  templateUrl: './search-roles.component.html',
  styleUrls: ['./search-roles.component.scss']
})
export class SearchRolesComponent implements OnInit {
  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  public configurationName = 'Roles';
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
