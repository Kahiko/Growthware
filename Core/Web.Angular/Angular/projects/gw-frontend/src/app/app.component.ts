import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// Library
import { ToasterComponent } from '@growthware/core/toast';
import { AccountService, IAccountInformation, IClientChoices } from '@growthware/core/account';
import { ISecurityEntityProfile, SecurityEntityService } from '@growthware/core/security-entities';
import { BehaviorSubject, Subscription } from 'rxjs';
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
export class AppComponent implements OnDestroy, OnInit {
	private _Skin: BehaviorSubject<string> = new BehaviorSubject<string>('default');
	private _Subscription: Subscription = new Subscription();

	readonly skin$ = this._Skin.asObservable();
	skin = 'default';
	title = 'gw-frontend';

	constructor(
		private _AccountSvc: AccountService,
		private _SecurityEntitySvc: SecurityEntityService
	) { }
	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngOnInit(): void {
		this._Subscription.add(
			this._AccountSvc.accountInformationChanged$.subscribe((val: IAccountInformation) => {
				// console.log('AppComponent.ngOnInit.val', val);
				this.setSkin(val.clientChoices);
			})
		);
	}

	private setSkin(clientChoices: IClientChoices): void {
		this._SecurityEntitySvc.getSecurityEntity(clientChoices.securityEntityID).then((response: ISecurityEntityProfile) => {
			// console.log('AppComponent.setSkin.response', response);
			this._Skin.next(response.skin.toLowerCase());
		});
	}
}
