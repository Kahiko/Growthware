import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableComponent, DynamicTableService } from '@growthware/core/dynamic-table';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { MessageDetailsComponent } from '../message-details/message-details.component';
import { MessageService } from '../../message.service';

@Component({
	selector: 'gw-core-search-messages',
	standalone: true,
	imports: [
		DynamicTableComponent
	],
	templateUrl: './search-messages.component.html',
	styleUrls: ['./search-messages.component.scss']
})
export class SearchMessagesComponent extends BaseSearchComponent {

	constructor(
		theFeatureSvc: MessageService,
		dynamicTableSvc: DynamicTableService,
		modalSvc: ModalService,
		searchSvc: SearchService,
	) { 
		super();
		this.configurationName = 'Messages';
		this._TheFeatureName = 'Message';
		this._TheApi = 'GrowthwareMessage/SearchMessages';
		this._TheComponent = MessageDetailsComponent;
		this._TheWindowSize = new WindowSize(450,900);
		this._TheService = theFeatureSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}
}
