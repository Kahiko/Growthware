import { Component, computed, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
// Angular Material
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
// Library
import { AccountService } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';
import { GWCommon } from '@growthware/common/services';
// Library Standalone
import { HierarchicalVerticalComponent, HorizontalComponent, VerticalComponent } from '@growthware/core/navigation';
import { LoaderComponent } from '@growthware/core/loader';
// Feature
import { BlueArrowFooterComponent } from '../blue-arrow-footer/blue-arrow-footer.component';

@Component({
	selector: 'gw-frontend-blue-arrow-layout',
	standalone: true,
	templateUrl: './blue-arrow-layout.component.html',
	styleUrls: ['./blue-arrow-layout.component.scss'],
	encapsulation: ViewEncapsulation.None,
	imports: [
		CommonModule,
		RouterOutlet,
		// Angular Material
		MatDividerModule,
		MatListModule,
		MatSidenavModule,
		// Library Standalone Menu Components
		HorizontalComponent,
		HierarchicalVerticalComponent,
		LoaderComponent,
		VerticalComponent,
		// Features
		BlueArrowFooterComponent,
	],
})
export class BlueArrowLayoutComponent {

	environment = computed(() => this._ConfigurationSvc.environment());
	navDescription: string = '';
	securityEntityName = computed(() => this._AccountSvc.clientChoices().securityEntityName);
	securityEntityTranslation = computed(() => this._ConfigurationSvc.securityEntityTranslation());
	version = computed(() => this._ConfigurationSvc.version());

	constructor(
		private _AccountSvc: AccountService,
		private _ConfigurationSvc: ConfigurationService,
		private _GWCommon: GWCommon,
	) {
		// do nothing atm
	}
}
