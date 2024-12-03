import { Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
// Angular Material
import { MatSidenavModule } from '@angular/material/sidenav';
// Library Standalone
import { LoaderComponent } from '@growthware/core/loader';
import { VerticalComponent } from '@growthware/core/navigation';
// Skin
import { sideNavTextAnimation } from '../animations/side-nav';
// Skin Components
import { DevOpsFooterComponent } from '../dev-ops-footer/dev-ops-footer.component';
import { DevOpsHeaderComponent } from '../dev-ops-header/dev-ops-header.component';

@Component({
	selector: 'gw-frontend-dev-ops-layout',
	standalone: true,
	templateUrl: './dev-ops-layout.component.html',
	styleUrls: ['./dev-ops-layout.component.scss'],
	animations: [sideNavTextAnimation],
	encapsulation: ViewEncapsulation.None,
	imports: [
		CommonModule,
		RouterOutlet,
		// Angular Material
		MatSidenavModule,
		// Skin Components
		DevOpsFooterComponent,
		DevOpsHeaderComponent,
		// Library Standalone
		LoaderComponent,
		VerticalComponent,
	],
})
export class DevOpsLayoutComponent {

}
