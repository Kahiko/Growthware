import { Component } from '@angular/core';

// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableComponent, DynamicTableService } from '@growthware/core/dynamic-table';
import { DataService } from '@growthware/common/services';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { StateDetailsComponent } from '../state-details/state-details.component';
import { StatesService } from '../../states.service';

@Component({
	selector: 'gw-core-search-states',
	standalone: true,
	imports: [
		DynamicTableComponent
	],
	templateUrl: './search-states.component.html',
	styleUrls: ['./search-states.component.scss']
})
export class SearchStatesComponent extends BaseSearchComponent {

	constructor(
		theFeatureSvc: StatesService,
		dataSvc: DataService,
		dynamicTableSvc: DynamicTableService,
		modalSvc: ModalService,
		searchSvc: SearchService,
	) { 
		super();
		this.configurationName = 'States';
		this._TheFeatureName = 'State';
		this._TheApi = 'GrowthwareState/Search_States';
		this._TheComponent = StateDetailsComponent;
		this._TheWindowSize = new WindowSize(450,900);
		this._TheService = theFeatureSvc;
		this._DataSvc = dataSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}
}
