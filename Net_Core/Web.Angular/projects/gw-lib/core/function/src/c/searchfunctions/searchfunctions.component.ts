import { Component } from '@angular/core';

// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableService, DynamicTableComponent } from '@growthware/core/dynamic-table';
import { DataService } from '@growthware/common/services';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { FunctionDetailsComponent } from '../function-details/function-details.component';
import { FunctionService } from '../../function.service';

@Component({
	selector: 'gw-core-searchfunctions',
	standalone: true,
	imports: [
		DynamicTableComponent
	],
	templateUrl: './searchfunctions.component.html',
	styleUrls: ['./searchfunctions.component.scss']
})
export class SearchfunctionsComponent extends BaseSearchComponent {

	constructor(
		theFeatureSvc: FunctionService,
		dataSvc: DataService,
		dynamicTableSvc: DynamicTableService,
		modalSvc: ModalService,
		searchSvc: SearchService,
	) { 
		super();
		this.configurationName = 'Functions';
		this._TheFeatureName = 'Function';
		this._TheApi = 'GrowthwareFunction/SearchFunctions';
		this._TheComponent = FunctionDetailsComponent;
		this._TheWindowSize = new WindowSize(625,950);
		this._TheService = theFeatureSvc;
		this._DataSvc = dataSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}
}