
import { Component, effect, inject, ViewEncapsulation, ChangeDetectionStrategy } from '@angular/core';
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
import { HorizontalComponent, HierarchicalVerticalComponent, VerticalComponent } from '@growthware/core/navigation';
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
	changeDetection: ChangeDetectionStrategy.Eager,
	imports: [
    RouterOutlet,
    ArcFooterComponent,
    ArcHeaderComponent,
    MatButtonModule,
    MatDividerModule,
    MatListModule,
    MatSidenavModule,
    MatToolbarModule,
    HorizontalComponent,
    HierarchicalVerticalComponent,
    VerticalComponent,
    LoaderComponent
],
})
export class ArcLayoutComponent {
	private _AccountSvc = inject(AccountService);

	public greeting: string = '';

	constructor() {
		effect(() => {
			this.greeting = '';
			if (this._AccountSvc.authenticationResponse().account.trim().toLocaleLowerCase() !== this._AccountSvc.anonymous.trim().toLocaleLowerCase()) {
				this.greeting = 'Hello, ' + this._AccountSvc.authenticationResponse().preferredName;
			}
		});
	}
}
