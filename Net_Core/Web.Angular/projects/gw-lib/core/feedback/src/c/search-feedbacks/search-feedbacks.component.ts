import { Component, inject } from '@angular/core';
// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableService, DynamicTableComponent } from '@growthware/core/dynamic-table';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { FeedbackDetailsComponent } from '../feedback-details/feedback-details.component';
import { FeedbackService } from '../../feedback.service';

@Component({
  selector: 'gw-core-search-feedbacks',
  standalone: true,
  imports: [
	DynamicTableComponent
  ],
  templateUrl: './search-feedbacks.component.html',
  styleUrl: './search-feedbacks.component.scss'
})
export class SearchFeedbacksComponent extends BaseSearchComponent {


	constructor(
		theFeatureSvc: FeedbackService,
		dynamicTableSvc: DynamicTableService,
		modalSvc: ModalService,
		searchSvc: SearchService,
	) { 
		super();
		this.configurationName = 'Search_Feedbacks';
		this._TheFeatureName = 'Feedbacks';
		this._TheApi = 'GrowthwareFeedback/SearchFeedbacks';
		this._TheComponent = FeedbackDetailsComponent;
		this._TheWindowSize = new WindowSize(400,400);
		this._TheService = theFeatureSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}
}
