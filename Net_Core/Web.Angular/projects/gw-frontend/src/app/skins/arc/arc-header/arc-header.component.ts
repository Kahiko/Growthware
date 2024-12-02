import { Component, computed, effect } from '@angular/core';
import { Router } from '@angular/router';
// Library
import { AccountService } from '@growthware/core/account';
import { GWCommon } from '@growthware/common/services';
import { ConfigurationService } from '@growthware/core/configuration';
import { HorizontalComponent, INavLink, NavigationService } from '@growthware/core/navigation';

@Component({
	selector: 'gw-frontend-arc-header',
	standalone: true,
	templateUrl: './arc-header.component.html',
	styleUrls: ['./arc-header.component.scss'],
	imports: [
		HorizontalComponent,
	]
})
export class ArcHeaderComponent {

	environment = computed<string>(() => this._ConfigurationSvc.environment());
	navDescription: string = '';
	securityEntityName = computed(() => this._AccountSvc.clientChoices().securityEntityName);
	securityEntityTranslation = computed<string>(() => this._ConfigurationSvc.securityEntityTranslation());
	version = computed<string>(() => this._ConfigurationSvc.version());

	constructor(
		private _AccountSvc: AccountService,
		private _ConfigurationSvc: ConfigurationService,
		private _GWCommon: GWCommon,
		private _NavigationSvc: NavigationService,
		private _Router: Router
	) { 
		effect(() => {
			const mNavLink = this._NavigationSvc.currentNavLink$();
			if (mNavLink.description.length > 0) {
				this.navDescription = mNavLink.description;
			} else {
				this.navDescription = '';
			}			
		});
	}

	onLogoClick(): void {
		if (this._AccountSvc.authenticationResponse().account.trim().toLocaleLowerCase() !== this._AccountSvc.anonymous.trim().toLocaleLowerCase()) {
			this._Router.navigate(['home']);
		} else {
			this._Router.navigate(['generic_home']);
		}
	}

}
