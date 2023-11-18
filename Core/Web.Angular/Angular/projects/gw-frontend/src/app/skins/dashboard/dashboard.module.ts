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
// // Library Standalone
import { HorizontalComponent } from '@Growthware/features/navigation';
// import { HierarchicalHorizontalComponent } from '@Growthware/features/navigation';
import { HierarchicalVerticalComponent } from '@Growthware/features/navigation';
import { VerticalComponent } from '@Growthware/features/navigation';
// Skin - Modules/Components
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardFooterComponent } from './dashboard-footer/dashboard-footer.component';
import { DashboardHeaderComponent } from './dashboard-header/dashboard-header.component';
import { DashboardLayoutComponent } from './dashboard-layout/dashboard-layout.component';


@NgModule({
  declarations: [
    DashboardFooterComponent,
    DashboardHeaderComponent,
    DashboardLayoutComponent
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
    HorizontalComponent,
    VerticalComponent,
    HierarchicalVerticalComponent,
    // Skin
    DashboardRoutingModule
  ],
  exports: [
    DashboardLayoutComponent,
  ]
})
export class DashboardModule { }
