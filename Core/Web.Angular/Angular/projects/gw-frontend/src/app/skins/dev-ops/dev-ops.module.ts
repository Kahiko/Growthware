import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
// Library Modules
import { LoaderModule } from '@Growthware/features/loader';
import { NavigationModule } from '@Growthware/features/navigation';
// Library Standalone
// import { HorizontalComponent } from '@Growthware/features/navigation';
// import { HierarchicalHorizontalComponent } from '@Growthware/features/navigation';
// import { HierarchicalVerticalComponent } from '@Growthware/features/navigation';
import { HierarchicalHorizontalFlyoutComponent } from '@Growthware/features/navigation';
import { VerticalComponent } from '@Growthware/features/navigation';
// Modules/Components
import { DevOpsRoutingModule } from './dev-ops-routing.module';
import { DevOpsFooterComponent } from './dev-ops-footer/dev-ops-footer.component';
import { DevOpsHeaderComponent } from './dev-ops-header/dev-ops-header.component';
import { DevOpsLayoutComponent } from './dev-ops-layout/dev-ops-layout.component';


@NgModule({
  declarations: [
    DevOpsFooterComponent,
    DevOpsHeaderComponent,
    DevOpsLayoutComponent
  ],
  imports: [
    CommonModule,
    // Angular Material
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    // Library Modules
    LoaderModule,
    NavigationModule,
    // Library Standalone
    HierarchicalHorizontalFlyoutComponent,
    VerticalComponent,
    // Modules/Components
    DevOpsRoutingModule
  ],
  exports: [
    DevOpsLayoutComponent
  ]
})
export class DevOpsModule { }