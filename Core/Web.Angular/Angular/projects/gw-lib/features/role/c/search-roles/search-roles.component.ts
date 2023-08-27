import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@Growthware/shared/components';
import { DynamicTableService } from '@Growthware/features/dynamic-table';
import { DataService } from '@Growthware/shared/services';
import { SearchService } from '@Growthware/features/search';
import { ModalService, WindowSize } from '@Growthware/features/modal';
// Feature
import { RoleDetailsComponent } from '../role-details/role-details.component';
import { RoleService } from '../../role.service';

@Component({
  selector: 'gw-lib-search-roles',
  templateUrl: './search-roles.component.html',
  styleUrls: ['./search-roles.component.scss']
})
export class SearchRolesComponent extends BaseSearchComponent {

  constructor(
    theFeatureSvc: RoleService,
    dataSvc: DataService,
    dynamicTableSvc: DynamicTableService,
    modalSvc: ModalService,
    searchSvc: SearchService,
  ) { 
    super();
    this.configurationName = 'Roles';
    this._TheFeatureName = 'Role';
    this._TheApi = 'GrowthwareRole/SearchRoles';
    this._TheComponent = RoleDetailsComponent;
    this._TheWindowSize = new WindowSize(450,900);
    this._TheService = theFeatureSvc;
    this._DataSvc = dataSvc;
    this._DynamicTableSvc = dynamicTableSvc;
    this._ModalSvc = modalSvc;
    this._SearchSvc = searchSvc;
  }
}
