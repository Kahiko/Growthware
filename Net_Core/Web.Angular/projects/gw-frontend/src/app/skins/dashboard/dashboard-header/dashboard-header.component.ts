import { Component, computed, input } from '@angular/core';
import { Router } from '@angular/router';
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
export class DashboardHeaderComponent {

	accountName = computed(() => this._GWCommon.formatData(this._AccountSvc.authenticationResponse().account, "text:28"));
	applicationName = computed<string>(() => this._ConfigurationSvc.applicationName());
	isAuthenticated = computed<boolean>(() => this._AccountSvc.authenticationResponse().account.toLocaleLowerCase() != this._AccountSvc.anonymous.toLowerCase());

	sidenav = input.required<MatSidenav>();

	constructor(
		private _AccountSvc: AccountService,
		private _ConfigurationSvc: ConfigurationService,
		private _GWCommon: GWCommon,
		private _ModalSvc: ModalService,
		private _Router: Router) { }

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
		this._ModalSvc.close(this._AccountSvc.logInModalId);
	}

	onLogout(): void {
		this._AccountSvc.logout();
	}
}
