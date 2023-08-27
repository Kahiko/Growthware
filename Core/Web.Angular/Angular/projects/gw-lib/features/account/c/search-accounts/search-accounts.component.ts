import { Component } from '@angular/core';
// Library
import { BaseSearchComponent } from '@Growthware/shared/components';
import { DynamicTableService } from '@Growthware/features/dynamic-table';
import { DataService } from '@Growthware/shared/services';
import { SearchService } from '@Growthware/features/search';
import { ModalService, WindowSize } from '@Growthware/features/modal';
// Feature
import { AccountDetailsComponent } from '../account-details/account-details.component';
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-search-accounts',
  templateUrl: './search-accounts.component.html',
  styleUrls: ['./search-accounts.component.scss']
})
export class SearchAccountsComponent extends BaseSearchComponent {

  constructor(
    theFeatureSvc: AccountService,
    dataSvc: DataService,
    dynamicTableSvc: DynamicTableService,
    modalSvc: ModalService,
    searchSvc: SearchService,
  ) { 
    super();
    this.configurationName = 'Accounts';
    this._TheApi = 'GrowthwareAccount/SearchAccounts';
    this._TheComponent = AccountDetailsComponent;
    this._TheWindowSize = new WindowSize(450,900);
    this._TheService = theFeatureSvc;
    this._DataSvc = dataSvc;
    this._DynamicTableSvc = dynamicTableSvc;
    this._ModalSvc = modalSvc;
    this._SearchSvc = searchSvc;
  }
}
