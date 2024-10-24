import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableService, DynamicTableComponent } from '@growthware/core/dynamic-table';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { GroupDetailsComponent } from '../group-details/group-details.component';
import { GroupService } from '../../group.service';

@Component({
	selector: 'gw-core-search-groups',
	standalone: true,
	imports: [
		DynamicTableComponent
	],
	templateUrl: './search-groups.component.html',
	styleUrls: ['./search-groups.component.scss']
})
export class SearchGroupsComponent extends BaseSearchComponent {

	constructor(
		theFeatureSvc: GroupService,
		dynamicTableSvc: DynamicTableService,
		modalSvc: ModalService,
		searchSvc: SearchService,
	) { 
		super();
		this.configurationName = 'Groups';
		this._TheFeatureName = 'Group';
		this._TheApi = 'GrowthwareGroup/SearchGroups';
		this._TheComponent = GroupDetailsComponent;
		this._TheWindowSize = new WindowSize(325,550);
		this._TheService = theFeatureSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}
}
