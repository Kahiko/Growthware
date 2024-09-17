import { Component, effect } from '@angular/core';
// Library
import { ConfigurationService } from '@growthware/core/configuration';

@Component({
	selector: 'gw-frontend-generic-home',
	templateUrl: './generic-home.component.html',
	styleUrls: ['./generic-home.component.scss']
})
export class GenericHomeComponent {
	applicationName: string = '';

	constructor(
    // private _AccountSvc: AccountService,
    private _ConfigurationSvc: ConfigurationService
	) { 
		effect(() => this.applicationName = this._ConfigurationSvc.applicationName());
	}
}
