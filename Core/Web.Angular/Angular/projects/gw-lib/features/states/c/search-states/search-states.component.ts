import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@Growthware/shared/components';
import { DynamicTableService } from '@Growthware/features/dynamic-table';
import { DataService } from '@Growthware/shared/services';
import { SearchService } from '@Growthware/features/search';
import { ModalService, WindowSize } from '@Growthware/features/modal';
// Feature
import { StateDetailsComponent } from '../state-details/state-details.component';
import { StatesService } from '../../states.service';

@Component({
  selector: 'gw-lib-search-states',
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
