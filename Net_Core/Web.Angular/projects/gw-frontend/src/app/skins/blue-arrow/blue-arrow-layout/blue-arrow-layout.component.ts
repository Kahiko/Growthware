import { Component, computed, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
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
export class BlueArrowLayoutComponent implements OnDestroy, OnInit {

	private _Subscription: Subscription = new Subscription();

	applicationName: string = '';
	environment: string = 'Development';
	navDescription: string = '';
	securityEntityName = computed(() => this._AccountSvc.clientChoicesSig().securityEntityName);
	securityEntityTranslation: string = '';
	version: string = '';

	constructor(
    private _AccountSvc: AccountService,
    private _ConfigurationSvc: ConfigurationService,
    private _GWCommon: GWCommon,
	) {
		// do nothing atm
	}

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngOnInit(): void {
		this._Subscription.add(
			this._ConfigurationSvc.securityEntityTranslation$.subscribe((val: string) => { this.securityEntityTranslation = val;})
		);    
		this._Subscription.add(
			this._ConfigurationSvc.applicationName$.subscribe((val: string) => { this.applicationName = val; })      
		);
		this._Subscription.add(
			this._ConfigurationSvc.version$.subscribe((val: string) => { this.version = val; })
		);
	}

}
