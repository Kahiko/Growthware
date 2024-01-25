import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
// import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
// Library Modules
import { LoaderComponent } from '@growthware/core/loader';
import { NavigationModule } from '@growthware/core/navigation';
// Library Standalone
import { HorizontalComponent } from '@growthware/core/navigation';
// import { HierarchicalHorizontalComponent } from '@growthware/core/navigation';
import { HierarchicalVerticalComponent } from '@growthware/core/navigation';
import { VerticalComponent } from '@growthware/core/navigation';
// Modules/Components
import { ArcRoutingModule } from './arc-routing.module';
import { ArcFooterComponent } from './arc-footer/arc-footer.component';
import { ArcHeaderComponent } from './arc-header/arc-header.component';
import { ArcLayoutComponent } from './arc-layout/arc-layout.component';

@NgModule({
	declarations: [
		ArcFooterComponent,
		ArcHeaderComponent,
		ArcLayoutComponent
	],
	imports: [
		CommonModule,
		// Skin Modules
		ArcRoutingModule,
		// Angular Material Modules
		MatButtonModule,
		MatDividerModule,
		// MatIconModule,
		MatListModule,
		// MatMenuModule,
		MatSidenavModule,
		MatToolbarModule,

		// Library Modules
		HorizontalComponent,
		// HierarchicalHorizontalComponent,
		HierarchicalVerticalComponent,
		VerticalComponent,
		LoaderComponent,
		NavigationModule
	],
	exports: [
		ArcLayoutComponent
	]
})
export class ArcModule { }
