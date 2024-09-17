import { Component, computed, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { AccountService } from '@growthware/core/account';
import { GWCommon } from '@growthware/common/services';
import { ConfigurationService } from '@growthware/core/configuration';
// import { LoginComponent } from '@growthware/core/account';
// import { ModalService, ModalOptions, WindowSize } from '@growthware/core/modal';
import { INavLink, NavigationService } from '@growthware/core/navigation';
// Skin

@Component({
	selector: 'gw-frontend-arc-header',
	templateUrl: './arc-header.component.html',
	styleUrls: ['./arc-header.component.scss']
})
export class ArcHeaderComponent implements OnDestroy, OnInit {
	private _Subscription: Subscription = new Subscription();

	applicationName: string = '';
	environment: string = 'Development';
	navDescription: string = '';
	securityEntityName = computed(() => this._AccountSvc.clientChoices().securityEntityName);
	securityEntityTranslation: string = '';
	version: string = '';

	constructor(
		private _AccountSvc: AccountService,
		private _ConfigurationSvc: ConfigurationService,
		private _GWCommon: GWCommon,
		// private _ModalSvc: ModalService,
		private _NavigationSvc: NavigationService,
		private _Router: Router
	) {
		// do nothing atm
	}
	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngOnInit(): void {
		this._Subscription.add(
			this._ConfigurationSvc.securityEntityTranslation$.subscribe((val: string) => { this.securityEntityTranslation = val; })
		);
		this._Subscription.add(
			this._ConfigurationSvc.applicationName$.subscribe((val: string) => { this.applicationName = val; })
		);
		this._Subscription.add(
			this._ConfigurationSvc.version$.subscribe((val: string) => { this.version = val; })
		);
		this._Subscription.add(
			this._NavigationSvc.currentNavLink$.subscribe((val: INavLink) => {
				// console.log('ArcHeaderComponent.ngOnInit.description', val.description);
				if (val.description.length > 0) {
					this.navDescription = val.description;
				} else {
					this.navDescription = 'Home';
				}
			})
		);
	}

	onLogoClick(): void {
		if (this._AccountSvc.authenticationResponse().account.trim().toLocaleLowerCase() !== this._AccountSvc.anonymous.trim().toLocaleLowerCase()) {
			this._Router.navigate(['home']);
		} else {
			this._Router.navigate(['generic_home']);
		}
	}

}
