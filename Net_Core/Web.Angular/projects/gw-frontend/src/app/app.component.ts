import { CommonModule } from '@angular/common';
import { Component, effect, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Title } from '@angular/platform-browser';
// Library
import { ToasterComponent } from '@growthware/core/toast';
import { AccountService, IAccountInformation } from '@growthware/core/account';
import { IClientChoices } from '@growthware/core/clientchoices';
import { ConfigurationService } from '@growthware/core/configuration';
import { ISecurityEntityProfile, SecurityEntityService } from '@growthware/core/security-entities';
// Application Modules (UI Skins)
import { ArcModule } from './skins/arc/arc.module';
import { DashboardModule } from './skins/dashboard/dashboard.module';
import { DefaultModule } from './skins/default/default.module';
import { BlueArrowModule } from './skins/blue-arrow/blue-arrow.module';
import { DevOpsModule } from './skins/dev-ops/dev-ops.module';
import { ProfessionalModule } from './skins/professional/professional.module';

@Component({
	selector: 'gw-frontend-root',
	standalone: true,
	imports: [
		CommonModule,
		RouterOutlet,
		// Library
		ToasterComponent,
		// Application Modules (UI Skins)
		ArcModule,
		DashboardModule,
		DefaultModule,
		BlueArrowModule,
		DevOpsModule,
		ProfessionalModule
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
