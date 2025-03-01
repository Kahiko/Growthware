import { Component, computed, input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenav } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
// Library
import { AccountService, LoginComponent } from '@growthware/core/account';
import { GWCommon } from '@growthware/common/services';
import { ModalService, ModalOptions, WindowSize } from '@growthware/core/modal';
import { ConfigurationService } from '@growthware/core/configuration';
// Library Standalone
import { HorizontalComponent } from '@growthware/core/navigation';

@Component({
	selector: 'gw-frontend-default-header',
	standalone: true,
	templateUrl: './default-header.component.html',
	styleUrls: ['./default-header.component.scss'],
	imports: [
		RouterLink,
		// Library Standalone
		HorizontalComponent,
		// Angular Material
		MatButtonModule,
		MatIconModule,
		MatMenuModule,
		MatToolbarModule,
	]
})
export class DefaultHeaderComponent {

	accountName = computed(() => this._GWCommon.formatData(this._AccountSvc.authenticationResponse().account, "text:28"));
	applicationName = computed(() => this._ConfigurationSvc.applicationName());
	isAuthenticated = computed(() => this._AccountSvc.authenticationResponse().account.toLocaleLowerCase() != this._AccountSvc.anonymous.toLowerCase());
	version = computed(() => this._ConfigurationSvc.version());

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
			this.onModalOk();
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
