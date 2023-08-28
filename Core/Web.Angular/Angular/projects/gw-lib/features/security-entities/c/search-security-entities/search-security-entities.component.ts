import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@Growthware/shared/components';
import { DynamicTableService } from '@Growthware/features/dynamic-table';
import { DataService } from '@Growthware/shared/services';
import { SearchService } from '@Growthware/features/search';
import { ModalService, WindowSize } from '@Growthware/features/modal';
// Feature
import { SecurityEntityDetailsComponent } from '../security-entity-details/security-entity-details.component';
import { SecurityEntityService } from '../../security-entity.service';

@Component({
  selector: 'gw-lib-search-security-entities',
  templateUrl: './search-security-entities.component.html',
  styleUrls: ['./search-security-entities.component.scss']
})
export class SearchSecurityEntitiesComponent extends BaseSearchComponent {

  constructor(
    theFeatureSvc: SecurityEntityService,
    dataSvc: DataService,
    dynamicTableSvc: DynamicTableService,
    modalSvc: ModalService,
    searchSvc: SearchService,
  ) { 
    super();
    this.configurationName = 'Security_Entities';
    this._TheFeatureName = 'Security Entity';
    this._TheApi = 'GrowthwareSecurityEntity/Security_Entities';
    this._TheComponent = SecurityEntityDetailsComponent;
    this._TheWindowSize = new WindowSize(450,900);
    this._TheService = theFeatureSvc;
    this._DataSvc = dataSvc;
    this._DynamicTableSvc = dynamicTableSvc;
    this._ModalSvc = modalSvc;
    this._SearchSvc = searchSvc;
  }
}
