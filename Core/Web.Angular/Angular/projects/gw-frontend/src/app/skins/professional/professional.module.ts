import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
// import { MatButtonModule } from '@angular/material/button';
// import { MatDividerModule } from '@angular/material/divider';
// import { MatIconModule } from '@angular/material/icon';
// import { MatListModule } from '@angular/material/list';
// import { MatMenuModule } from '@angular/material/menu';
// import { MatSidenavModule } from '@angular/material/sidenav';
// import { MatToolbarModule } from '@angular/material/toolbar';
// Library Modules
// import { LoaderModule } from '@Growthware/features/loader';
// import { NavigationModule } from '@Growthware/features/navigation';
// Library Standalone
// import { HorizontalComponent } from '@Growthware/features/navigation';
// import { HierarchicalHorizontalComponent } from '@Growthware/features/navigation';
// import { HierarchicalVerticalComponent } from '@Growthware/features/navigation';
// import { HierarchicalHorizontalFlyoutComponent } from '@Growthware/features/navigation';
// import { VerticalComponent } from '@Growthware/features/navigation';
// Modules/Components
import { ProfessionalRoutingModule } from './professional-routing.module';
import { ProfessionalFooterComponent } from './professional-footer/professional-footer.component';
import { ProfessionalHeaderComponent } from './professional-header/professional-header.component';
import { ProfessionalLayoutComponent } from './professional-layout/professional-layout.component';


@NgModule({
  declarations: [
    ProfessionalFooterComponent,
    ProfessionalHeaderComponent,
    ProfessionalLayoutComponent
  ],
  imports: [
    CommonModule,
    ProfessionalRoutingModule
  ],
  exports: [
    ProfessionalLayoutComponent
  ]
})
export class ProfessionalModule { }
