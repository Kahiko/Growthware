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
// import { HierarchicalHorizontalFlyoutComponent } from '@growthware/core/navigation';
import { VerticalComponent } from '@growthware/core/navigation';
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
		// Angular Material
		// MatButtonModule,
		MatDividerModule,
		// MatIconModule,
		MatListModule,
		// MatMenuModule,
		MatSidenavModule,
		// MatToolbarModule,
		// Library Modules
		LoaderComponent,
		NavigationModule,
		// Library Standalone
		HorizontalComponent,
		// HierarchicalHorizontalComponent,
		HierarchicalVerticalComponent,
		// HierarchicalHorizontalFlyoutComponent,
		VerticalComponent,
		// Modules/Components
		ProfessionalRoutingModule,
	],
	exports: [
		ProfessionalLayoutComponent
	]
})
export class ProfessionalModule { }
