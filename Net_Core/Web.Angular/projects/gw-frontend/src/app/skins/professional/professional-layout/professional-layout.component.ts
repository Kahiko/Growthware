import { Component, ViewEncapsulation, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// Angular Material
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
// Library Standalone
import { LoaderComponent } from '@growthware/core/loader';
import { HierarchicalVerticalComponent, VerticalComponent } from '@growthware/core/navigation';
// Modules/Components
import { ProfessionalFooterComponent } from '../professional-footer/professional-footer.component';
import { ProfessionalHeaderComponent } from '../professional-header/professional-header.component';

@Component({
    selector: 'gw-frontend-professional-layout',
    templateUrl: './professional-layout.component.html',
    styleUrls: ['./professional-layout.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.Eager,
    imports: [
        RouterOutlet,
        // Angular Material
        MatListModule,
        MatSidenavModule,
        // Library Standalone
        LoaderComponent,
        HierarchicalVerticalComponent,
        VerticalComponent,
        // Skin Compontents
        ProfessionalFooterComponent,
        ProfessionalHeaderComponent,
    ]
})
export class ProfessionalLayoutComponent {

}
