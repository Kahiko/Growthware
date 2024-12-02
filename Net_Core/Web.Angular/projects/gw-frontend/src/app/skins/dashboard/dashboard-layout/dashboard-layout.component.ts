import { Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
// Angular Material
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
// Library Standalone
import { VerticalComponent, HierarchicalVerticalComponent } from '@growthware/core/navigation';
import { LoaderComponent } from '@growthware/core/loader';
// Skin
import { sideNavTextAnimation } from '../animations/side-nav';
import { DashboardFooterComponent } from '../dashboard-footer/dashboard-footer.component';
import { DashboardHeaderComponent } from '../dashboard-header/dashboard-header.component';

@Component({
	selector: 'gw-frontend-dashboard-layout',
	standalone: true,
	templateUrl: './dashboard-layout.component.html',
	styleUrls: ['./dashboard-layout.component.scss'],
	animations: [sideNavTextAnimation],
	encapsulation: ViewEncapsulation.None,
	imports: [
		CommonModule,
		RouterOutlet,
		// Angular Material
		MatDividerModule,
		MatListModule,
		MatSidenavModule,
		// Library Modules
		LoaderComponent,
		// Library Standalone
		VerticalComponent,
		HierarchicalVerticalComponent,
		// Skin
		DashboardFooterComponent,
		DashboardHeaderComponent,
	],
})
export class DashboardLayoutComponent {

	constructor() { }

}
