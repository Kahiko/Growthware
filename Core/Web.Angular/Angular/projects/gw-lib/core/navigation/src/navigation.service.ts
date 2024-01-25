import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router, NavigationEnd } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
// Library
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
// Feature
import { INavLink, NavLink } from './nav-link.model';
import { MenuTypes } from './menu-types.enum';
import { LinkBehaviors } from './link-behaviors.enum';

@Injectable({
	providedIn: 'root',
})
export class NavigationService {

	private _ApiName: string = 'GrowthwareAccount/';
	private _Api_GetMenuData: string = '';
	private _Api_GetMenuItems: string = '';
	private _BaseURL: string = '';

	private _CurrentNavLink = new BehaviorSubject<INavLink>(new NavLink('', '', '', '', '', 0, '', true, ''));
	private _ShowNavText = new BehaviorSubject<boolean>(true); // Sets the inital value in all controls

	public currentNavLink$ = this._CurrentNavLink.asObservable();
	public currentUrl = new BehaviorSubject<string>('');
	readonly showNavText$ = this._ShowNavText.asObservable();

	constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private _Router: Router
	) {
		this._BaseURL = this._GWCommon.baseURL;
		this._Api_GetMenuData = this._BaseURL + this._ApiName + 'GetMenuData';
		this._Api_GetMenuItems = this._BaseURL + this._ApiName + 'GetMenuItems';
		this._Router.events.subscribe({
			next: (event: any) => {
				if (event instanceof NavigationEnd) {
					this.currentUrl.next(event.urlAfterRedirects);
				}
			},
		});
	}

	public getMenuData(menuType: MenuTypes, configuarionName: string): void {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('menuType', menuType);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter
		};
		this._HttpClient.get<INavLink[]>(this._Api_GetMenuData, mHttpOptions).subscribe({
			next: (response: INavLink[]) => {
				this._DataSvc.notifyDataChanged(configuarionName, response);
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'NavigationService', 'getNavLinks');
			},
			complete: () => {
				// here as example
			}
		});
	}

	public getNavLinks(menuType: MenuTypes, configuarionName: string): void {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('menuType', menuType);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter
		};
		this._HttpClient.get<INavLink[]>(this._Api_GetMenuItems, mHttpOptions).subscribe({
			next: (response: INavLink[]) => {
				this._DataSvc.notifyDataChanged(configuarionName, response);
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'NavigationService', 'getNavLinks');
			},
			complete: () => {
				// here as example
			}
		});
	}

	getShowNavText(): boolean {
		return this._ShowNavText.getValue();
	}

	navigateTo(navLink: INavLink): void {
		// console.log('NavigationService.navigateTo', navLink);
		this._CurrentNavLink.next(navLink);
		if (!navLink.children || !navLink.children.length) {
			switch (navLink.linkBehavior) {
			case LinkBehaviors.Internal:
				this._Router.navigate([navLink.action.toLowerCase()]);
				break;
			case LinkBehaviors.Popup:
				this._Router.navigate([navLink.action.toLowerCase()]);
				// TODO: need to fingure out how to get the windows size to here.
				// I don't like the idea of putting into the DB by that may be the best way.
				// this._Router.navigate([item.action.toLowerCase()]);
				break;
			case LinkBehaviors.External:
				window.open(navLink.link, '_blank');
				break;
			case LinkBehaviors.NewPage:
				window.open('/' + navLink.link.toLowerCase(), '_blank');
				break;
			default:
				this._Router.navigate([navLink.action.toLowerCase()]);
				break;
			}
		}    
	}

	setShowNavText(value: boolean): void {
		this._ShowNavText.next(value);
	}
}
