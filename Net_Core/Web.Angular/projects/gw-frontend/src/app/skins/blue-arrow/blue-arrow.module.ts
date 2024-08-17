import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
// Library Modules
import { LoaderComponent } from '@growthware/core/loader';
// Library Standalone
import { HorizontalComponent } from '@growthware/core/navigation';
// import { HierarchicalHorizontalComponent } from '@growthware/core/navigation';
import { HierarchicalVerticalComponent } from '@growthware/core/navigation';
// import { HierarchicalVerticalFlyoutComponent } from '@growthware/core/navigation';
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
		MatDividerModule,
		MatListModule,
		MatSidenavModule,
    
		HorizontalComponent,
		HierarchicalVerticalComponent,
		// HierarchicalVerticalFlyoutComponent,
		VerticalComponent,

		CommonModule,
		BlueArrowRoutingModule,
		LoaderComponent,

	],
	exports: [
		BlueArrowLayoutComponent
	]
})
export class BlueArrowModule { }
