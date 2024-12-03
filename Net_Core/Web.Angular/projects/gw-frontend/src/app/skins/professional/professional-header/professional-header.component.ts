import { Component, computed } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { AccountService } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';
import { HorizontalComponent } from '@growthware/core/navigation';


@Component({
	selector: 'gw-frontend-professional-header',
	standalone: true,
	templateUrl: './professional-header.component.html',
	styleUrls: ['./professional-header.component.scss'],
	imports: [
		HorizontalComponent,
	],
})
export class ProfessionalHeaderComponent {

	environment: string = 'Development';
	name = computed(() => this._AccountSvc.clientChoices().account);
	securityEntityName = computed(() => this._AccountSvc.clientChoices().securityEntityName);
	securityEntityTranslation = computed(() => this._ConfigurationSvc.securityEntityTranslation());
	version = computed(() => this._ConfigurationSvc.version());

	constructor(
		private _AccountSvc: AccountService,
		private _ConfigurationSvc: ConfigurationService,
	) { }

}
