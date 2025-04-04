import { CommonModule } from '@angular/common';
import { Component, effect, signal } from '@angular/core';
import { Title } from '@angular/platform-browser';
// Library
import { ToasterComponent } from '@growthware/core/toast';
import { AccountService } from '@growthware/core/account';
import { IClientChoices } from '@growthware/core/clientchoices';
import { ConfigurationService } from '@growthware/core/configuration';
import { ISecurityEntityProfile, SecurityEntityService } from '@growthware/core/security-entities';
// Application Components (UI Skins)
import { ArcLayoutComponent } from './skins/arc/arc-layout/arc-layout.component';
import { BlueArrowLayoutComponent } from './skins/blue-arrow/blue-arrow-layout/blue-arrow-layout.component';
import { DashboardLayoutComponent } from './skins/dashboard/dashboard-layout/dashboard-layout.component';
import { DefaultLayoutComponent } from './skins/default/default-layout/default-layout.component';
import { DevOpsLayoutComponent } from './skins/dev-ops/dev-ops-layout/dev-ops-layout.component';
import { ProfessionalLayoutComponent } from './skins/professional/professional-layout/professional-layout.component';

@Component({
	selector: 'gw-frontend-root',
	standalone: true,
	imports: [
		CommonModule,
		// Library
		ToasterComponent,
		// Application Modules (UI Skins)
		ArcLayoutComponent,
		DashboardLayoutComponent,
		DefaultLayoutComponent,
		BlueArrowLayoutComponent,
		DevOpsLayoutComponent,
		ProfessionalLayoutComponent
	],
	templateUrl: './app.component.html',
	styleUrl: './app.component.scss'
})
export class AppComponent {
	skin = signal<string>('default');
	title = 'gw-frontend';

	constructor(
		private _AccountSvc: AccountService,
		private _ConfigurationSvc: ConfigurationService,
		private _SecurityEntitySvc: SecurityEntityService,
		private _TitleService: Title,
	) { 
		// update the sking if the client choices change in the AccountService
		effect(() => {
			this.setSkin(this._AccountSvc.clientChoices());
		});
		effect(() => {
			this.title = this._ConfigurationSvc.applicationName();
			this._TitleService.setTitle(this.title + ' | Growthware');
		});
	}

	private setSkin(clientChoices: IClientChoices): void {
		this._SecurityEntitySvc.getSecurityEntity(clientChoices.securityEntityId).then((response: ISecurityEntityProfile) => {
			// console.log('AppComponent.setSkin.response', response);
			this.skin.set(response.skin.toLowerCase());
		});
	}
}
