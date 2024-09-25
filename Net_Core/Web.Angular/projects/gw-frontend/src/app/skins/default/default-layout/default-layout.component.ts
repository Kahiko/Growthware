import { Component, inject, ViewEncapsulation } from '@angular/core';
// Library
import { INavLink, NavigationService } from '@growthware/core/navigation';
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
	selector: 'gw-frontend-default-layout',
	templateUrl: './default-layout.component.html',
	styleUrls: ['./default-layout.component.scss'],
	animations: [sideNavTextAnimation],
	encapsulation: ViewEncapsulation.None,
})
export class DefaultLayoutComponent {
	private _NavigationSvc = inject(NavigationService);

	showSideNavLinkText: boolean = true;

	onShowSideNavLinkText(): void {
		this.showSideNavLinkText = !this.showSideNavLinkText;
		this._NavigationSvc.setShowNavText(this.showSideNavLinkText);
	}
}
