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
import { LoaderComponent } from '@growthware/core/loader';
import { NavigationModule } from '@growthware/core/navigation';
// Library Standalone
import { HorizontalComponent } from '@growthware/core/navigation';
// import { HierarchicalHorizontalComponent } from '@growthware/core/navigation';
import { HierarchicalVerticalComponent } from '@growthware/core/navigation';
import { VerticalComponent } from '@growthware/core/navigation';
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
		LoaderComponent,

		NavigationModule
	],
	exports: [
		BlueArrowLayoutComponent
	]
})
export class BlueArrowModule { }
