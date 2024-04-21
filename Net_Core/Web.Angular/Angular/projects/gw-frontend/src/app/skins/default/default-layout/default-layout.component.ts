import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
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
export class DefaultLayoutComponent implements OnDestroy, OnInit {
	private _Subscriptions: Subscription = new Subscription();

	showSideNavLinkText!: boolean;
	verticalNavLinks: Array<INavLink> = [];

	constructor(
    private _MenuListSvc: NavigationService
	) { }

	ngOnDestroy(): void {
		this._Subscriptions.unsubscribe();
	}

	ngOnInit(): void {
		this.showSideNavLinkText = this._MenuListSvc.getShowNavText();
		this._Subscriptions.add(
			this._MenuListSvc.showNavText$.subscribe((value)=>{
				this.showSideNavLinkText = value;
			})
		);
	}

	onShowSideNavLinkText(): void {
		this._MenuListSvc.setShowNavText(!this.showSideNavLinkText);
	}
}
