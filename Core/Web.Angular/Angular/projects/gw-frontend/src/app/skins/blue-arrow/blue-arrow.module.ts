import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
// import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
// import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
// import { MatToolbarModule } from '@angular/material/toolbar';
// Library Modules
import { LoaderModule } from '@Growthware/features/loader';
import { NavigationModule } from '@Growthware/features/navigation';
// Library Standalone
import { HorizontalComponent } from '@Growthware/features/navigation';
// import { HierarchicalHorizontalComponent } from '@Growthware/features/navigation';
import { HierarchicalVerticalComponent } from '@Growthware/features/navigation';
import { VerticalComponent } from '@Growthware/features/navigation';
// Modules/Components
import { BlueArrowRoutingModule } from './blue-arrow-routing.module';
import { BlueArrowFooterComponent } from './blue-arrow-footer/blue-arrow-footer.component';
import { BlueArrowHeaderComponent } from './blue-arrow-header/blue-arrow-header.component';
import { BlueArrowLayoutComponent } from './blue-arrow-layout/blue-arrow-layout.component';

@NgModule({
  declarations: [
    BlueArrowFooterComponent,
    BlueArrowHeaderComponent,
    BlueArrowLayoutComponent
  ],
  imports: [
    // Angular Material
    // MatButtonModule,
    MatDividerModule,
    // MatIconModule,
    MatListModule,
    // MatMenuModule,
    MatSidenavModule,
    // MatToolbarModule,
    
    HorizontalComponent,
    // HierarchicalHorizontalComponent,
    HierarchicalVerticalComponent,
    VerticalComponent,

    CommonModule,
    BlueArrowRoutingModule,
    LoaderModule,

    NavigationModule
  ],
  exports: [
    BlueArrowLayoutComponent
  ]
})
export class BlueArrowModule { }
