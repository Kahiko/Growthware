import { Component, computed, ViewEncapsulation } from '@angular/core';
// Library
import { AccountService } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';
import { GWCommon } from '@growthware/common/services';

@Component({
	selector: 'gw-frontend-blue-arrow-layout',
	templateUrl: './blue-arrow-layout.component.html',
	styleUrls: ['./blue-arrow-layout.component.scss'],
	encapsulation: ViewEncapsulation.None,
})
export class BlueArrowLayoutComponent {

	environment = computed(() => this._ConfigurationSvc.environment());
	navDescription: string = '';
	securityEntityName = computed(() => this._AccountSvc.clientChoices().securityEntityName);
	securityEntityTranslation = computed(() => this._ConfigurationSvc.securityEntityTranslation());
	version = computed(() => this._ConfigurationSvc.version());

	constructor(
    private _AccountSvc: AccountService,
    private _ConfigurationSvc: ConfigurationService,
    private _GWCommon: GWCommon,
	) {
		// do nothing atm
	}
}
