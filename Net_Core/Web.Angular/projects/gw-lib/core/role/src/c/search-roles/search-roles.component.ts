import { Component } from '@angular/core';

// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableService, DynamicTableComponent } from '@growthware/core/dynamic-table';
import { DataService } from '@growthware/common/services';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { RoleDetailsComponent } from '../role-details/role-details.component';
import { RoleService } from '../../role.service';

@Component({
	selector: 'gw-core-search-roles',
	standalone: true,
	imports: [
		DynamicTableComponent
	],
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
		this._TheWindowSize = new WindowSize(400,450);
		this._TheService = theFeatureSvc;
		this._DataSvc = dataSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}
}