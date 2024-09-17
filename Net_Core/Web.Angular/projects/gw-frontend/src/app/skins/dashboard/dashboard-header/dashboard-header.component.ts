import { Component, computed, input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Angular Material
import { MatSidenav } from '@angular/material/sidenav';
// Library
import { AccountService } from '@growthware/core/account';
import { GWCommon } from '@growthware/common/services';
import { LoginComponent } from '@growthware/core/account';
import { ModalService, ModalOptions, WindowSize } from '@growthware/core/modal';
import { ConfigurationService } from '@growthware/core/configuration';

@Component({
	selector: 'gw-frontend-dashboard-header',
	templateUrl: './dashboard-header.component.html',
	styleUrls: ['./dashboard-header.component.scss']
})
export class DashboardHeaderComponent implements OnDestroy, OnInit {
	private _Subscription: Subscription = new Subscription();

	accountName = computed(() => this._GWCommon.formatData(this._AccountSvc.authenticationResponse().account, "text:28"));
	applicationName: string = '';
	isAuthenticated = computed(() => this._AccountSvc.authenticationResponse().account.toLocaleLowerCase() != this._AccountSvc.anonymous.toLowerCase());
	version: string = '';

	sidenav = input.required<MatSidenav>();

	constructor(
		private _AccountSvc: AccountService,
		private _ConfigurationSvc: ConfigurationService,
		private _GWCommon: GWCommon,
		private _ModalSvc: ModalService,
		private _Router: Router) { }

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngOnInit(): void {
		this._Subscription.add(
			this._ConfigurationSvc.applicationName$.subscribe((val: string) => { this.applicationName = val; })
		);
		this._Subscription.add(
			this._ConfigurationSvc.version$.subscribe((val: string) => { this.version = val; })
		);
	}

	onLogin(): void {
		const mWindowSize: WindowSize = new WindowSize(225, 450);
		const mModalOptions: ModalOptions = new ModalOptions(this._AccountSvc.logInModalId, 'Logon', LoginComponent, mWindowSize);
		mModalOptions.buttons.okButton.callbackMethod = () => {
			this.onModalOk;
		};
		this._ModalSvc.open(mModalOptions);
	}

	onLogoClick(): void {
		if (this._AccountSvc.authenticationResponse().account.trim().toLocaleLowerCase() !== this._AccountSvc.anonymous.trim().toLocaleLowerCase()) {
			this._Router.navigate(['home']);
		} else {
			this._Router.navigate(['generic_home']);
		}
	}

	onModalOk() {

	}

	onLogout(): void {
		this._AccountSvc.logout();
	}
}
