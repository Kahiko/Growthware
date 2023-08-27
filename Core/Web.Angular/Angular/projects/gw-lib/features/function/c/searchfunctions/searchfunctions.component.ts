import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@Growthware/shared/components';
import { DynamicTableService } from '@Growthware/features/dynamic-table';
import { DataService } from '@Growthware/shared/services';
import { SearchService } from '@Growthware/features/search';
import { ModalService, WindowSize } from '@Growthware/features/modal';
// Feature
import { FunctionDetailsComponent } from '../function-details/function-details.component';
import { FunctionService } from '../../function.service';

@Component({
  selector: 'gw-lib-searchfunctions',
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
    this._TheWindowSize = new WindowSize(450,900);
    this._TheService = theFeatureSvc;
    this._DataSvc = dataSvc;
    this._DynamicTableSvc = dynamicTableSvc;
    this._ModalSvc = modalSvc;
    this._SearchSvc = searchSvc;
  }
}
