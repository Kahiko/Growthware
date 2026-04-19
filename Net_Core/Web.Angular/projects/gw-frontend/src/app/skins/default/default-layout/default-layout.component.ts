import { Component, inject, ViewEncapsulation } from '@angular/core';

// Library
import { sideNavTextAnimation } from '../animations/side-nav';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
// Library Modules
import { LoaderComponent } from '@growthware/core/loader';
// Library Standalone
import { HierarchicalVerticalComponent, NavigationService, VerticalComponent } from '@growthware/core/navigation';
// Modules/Components
import { DefaultFooterComponent } from '../default-footer/default-footer.component';
import { DefaultHeaderComponent } from '../default-header/default-header.component';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'gw-frontend-default-layout',
    templateUrl: './default-layout.component.html',
    styleUrls: ['./default-layout.component.scss'],
    animations: [sideNavTextAnimation],
    encapsulation: ViewEncapsulation.None,
    imports: [
    RouterModule,
    DefaultFooterComponent,
    DefaultHeaderComponent,
    MatButtonModule,
    MatDividerModule,
    MatListModule,
    MatSidenavModule,
    LoaderComponent,
    VerticalComponent,
    HierarchicalVerticalComponent
]
})
export class DefaultLayoutComponent {
	private _NavigationSvc = inject(NavigationService);

	showSideNavLinkText: boolean = true;

	onShowSideNavLinkText(): void {
		this.showSideNavLinkText = !this.showSideNavLinkText;
		this._NavigationSvc.setShowNavText(this.showSideNavLinkText);
	}
}
