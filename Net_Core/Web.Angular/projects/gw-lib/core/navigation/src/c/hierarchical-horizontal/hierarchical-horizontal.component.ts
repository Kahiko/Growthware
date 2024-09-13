import { Component, Input } from '@angular/core';

// Library
import { AccountService } from '@growthware/core/account';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
// Feature
import { BaseNavigationComponent } from '../../base-navigation.component';
import { NavigationService } from '../../navigation.service';
// import { INavLink } from '../../nav-link.model';

@Component({
	selector: 'gw-core-hierarchical-horizontal',
	standalone: true,
	imports: [],
	templateUrl: './hierarchical-horizontal.component.html',
	styleUrls: ['./hierarchical-horizontal.component.scss']
})
export class HierarchicalHorizontalComponent extends BaseNavigationComponent {

	@Input() override name: string = '';

	constructor(
		accountSvc: AccountService,
		dataSvc: DataService,
		gwCommon: GWCommon,
		loggingSvc: LoggingService,
		menuListSvc: NavigationService,
	) {
		super();
		this._AccountSvc = accountSvc;
		this._DataSvc = dataSvc;
		this._GWCommon = gwCommon;
		this._LoggingSvc = loggingSvc;
		this._NavigationSvc = menuListSvc;
	}
}
