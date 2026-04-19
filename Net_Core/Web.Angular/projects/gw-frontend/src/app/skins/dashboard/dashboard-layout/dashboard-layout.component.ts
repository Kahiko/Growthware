import { Component, ViewEncapsulation } from '@angular/core';

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
    templateUrl: './dashboard-layout.component.html',
    styleUrls: ['./dashboard-layout.component.scss'],
    animations: [sideNavTextAnimation],
    encapsulation: ViewEncapsulation.None,
    imports: [
    RouterOutlet,
    MatDividerModule,
    MatListModule,
    MatSidenavModule,
    LoaderComponent,
    VerticalComponent,
    HierarchicalVerticalComponent,
    DashboardFooterComponent,
    DashboardHeaderComponent
]
})
export class DashboardLayoutComponent {

	constructor() { }

}
