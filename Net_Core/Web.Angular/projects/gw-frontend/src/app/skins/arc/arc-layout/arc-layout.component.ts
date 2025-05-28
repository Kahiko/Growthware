import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
// Library
import { AccountService } from '@growthware/core/account';
import { LoaderComponent } from '@growthware/core/loader';
import { HierarchicalVerticalComponent, VerticalComponent } from '@growthware/core/navigation';
import { HorizontalComponent, NavigationService } from '@growthware/core/navigation';
// Feature
import { ArcFooterComponent } from '../arc-footer/arc-footer.component';
import { ArcHeaderComponent } from '../arc-header/arc-header.component';
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
	selector: 'gw-frontend-arc-layout',
	standalone: true,
	templateUrl: './arc-layout.component.html',
	styleUrls: ['./arc-layout.component.scss'],
	animations: [sideNavTextAnimation],
	encapsulation: ViewEncapsulation.None,
	imports: [
		CommonModule,
		RouterOutlet,
		// Feature
		ArcFooterComponent,
		ArcHeaderComponent,

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
		HierarchicalVerticalComponent,
		VerticalComponent,
		LoaderComponent,
	],
})
export class ArcLayoutComponent implements OnInit {
	private _AccountSvc = inject(AccountService);

	public greeting: string = '';


	ngOnInit(): void {
		console.log('', this._AccountSvc.authenticationResponse());
		this.greeting = '';
		if (this._AccountSvc.authenticationResponse().account.trim().toLocaleLowerCase() !== this._AccountSvc.anonymous.trim().toLocaleLowerCase()) {
			this.greeting = 'Hello, ' + this._AccountSvc.authenticationResponse().account;
		}
	}

}
