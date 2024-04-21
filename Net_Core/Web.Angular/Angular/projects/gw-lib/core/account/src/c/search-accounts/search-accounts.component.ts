import { Component } from '@angular/core';

// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableComponent, DynamicTableService } from '@growthware/core/dynamic-table';
import { DataService } from '@growthware/common/services';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { AccountDetailsComponent } from '../account-details/account-details.component';
import { AccountService } from '../../account.service';

@Component({
	selector: 'gw-core-search-accounts',
	standalone: true,
	imports: [
		DynamicTableComponent
	],
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
		this._TheFeatureName = 'Account';
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
