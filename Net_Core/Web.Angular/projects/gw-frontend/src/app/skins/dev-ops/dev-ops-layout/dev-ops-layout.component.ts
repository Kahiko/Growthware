import { Component, ViewEncapsulation } from '@angular/core';

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
    templateUrl: './dev-ops-layout.component.html',
    styleUrls: ['./dev-ops-layout.component.scss'],
    animations: [sideNavTextAnimation],
    encapsulation: ViewEncapsulation.None,
    imports: [
    RouterOutlet,
    MatSidenavModule,
    DevOpsFooterComponent,
    DevOpsHeaderComponent,
    LoaderComponent,
    VerticalComponent
]
})
export class DevOpsLayoutComponent {

}
