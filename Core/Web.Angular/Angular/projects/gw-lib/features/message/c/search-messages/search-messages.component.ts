import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@Growthware/shared/components';
import { DynamicTableService } from '@Growthware/features/dynamic-table';
import { DataService } from '@Growthware/shared/services';
import { SearchService } from '@Growthware/features/search';
import { ModalService, WindowSize } from '@Growthware/features/modal';
// Feature
import { MessageDetailsComponent } from '../message-details/message-details.component';
import { MessageService } from '../../message.service';

@Component({
  selector: 'gw-lib-search-messages',
  templateUrl: './search-messages.component.html',
  styleUrls: ['./search-messages.component.scss']
})
export class SearchMessagesComponent extends BaseSearchComponent {

  constructor(
    theFeatureSvc: MessageService,
    dataSvc: DataService,
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
    this._DataSvc = dataSvc;
    this._DynamicTableSvc = dynamicTableSvc;
    this._ModalSvc = modalSvc;
    this._SearchSvc = searchSvc;
  }
}
