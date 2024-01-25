import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { INavLink, NavigationService } from '@growthware/core/navigation';
// Skin
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
	selector: 'gw-frontend-dashboard-layout',
	templateUrl: './dashboard-layout.component.html',
	styleUrls: ['./dashboard-layout.component.scss'],
	animations: [sideNavTextAnimation],
	encapsulation: ViewEncapsulation.None
})
export class DashboardLayoutComponent implements OnDestroy, OnInit {
	private _Subscriptions: Subscription = new Subscription();

	verticalNavLinks: Array<INavLink> = [];

	constructor(
    private _MenuListSvc: NavigationService
	) { }

	ngOnDestroy(): void {
		this._Subscriptions.unsubscribe();
	}

	ngOnInit(): void {
		this._Subscriptions.add(
			this._MenuListSvc.showNavText$.subscribe((value)=>{
			})
		);
	}
}
