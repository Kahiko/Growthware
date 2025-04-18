import {
	Component,
	ElementRef,
	effect,
	HostBinding,
	inject,
	input,
	OnInit,
	signal
} from '@angular/core';
import { Router } from '@angular/router';
// Library
import { AccountService } from '@growthware/core/account';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { INavItem } from '@growthware/common/interfaces';
// Feature
import { INavLink } from '../../nav-link.model';
import { NavigationService } from '../../navigation.service';
import { MenuTypes } from '../../menu-types.enum';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
	selector: 'gw-core-navigation-base',
	standalone: true,
	imports: [],
	template: '',
	styles: []
})
export abstract class NavigationComponentBase implements OnInit {

	backgroundColor = input<string>('lightblue');
	expanded!: boolean;
	@HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;
	protected firstLevel!: ElementRef<HTMLUListElement>;
	fontColor = input<string>('black');
	flyout = input<string>('false');
	height = input<string>('32px');
	hoverBackgroundColor = input<string>('#2c3e50');
	menuData = signal<Array<INavLink>>([]);
	id = input.required<string>();
	readonly showSideNavLinkText = signal<boolean>(true);
	abstract _MenuType: MenuTypes

	protected _AccountSvc = inject(AccountService);
	protected _LoggingSvc: LoggingService = inject(LoggingService);
	protected _NavigationSvc = inject(NavigationService);
	protected _Router = inject(Router);
	private _GWCommon = inject(GWCommon);

	constructor() {
		effect(() => {
			// Just making a reference to the updateMenu$ signal to the effect will be registered
			this._AccountSvc.updateMenu$();
			// The signals value isn't important only that a change was triggered.
			// console.log('NavigationComponentBase.constructor', this._AccountSvc.updateMenu$());
			if (this.flyout() === 'false') {
				this._NavigationSvc.getNavLinks(this._MenuType).then((response) => {
					this.menuData.update(() => response);
				}).catch((error: HttpErrorResponse) => {
					this._LoggingSvc.errorHandler(error, 'NavigationComponentBase', 'constructor');
				});
			} else {
				this._NavigationSvc.getMenuData(this._MenuType).then((response) => {
					// convert the retunred data into IMenuItem
					const mNavItems: INavItem[] = [];
					this._GWCommon.buildNavItems(response).forEach((item) => {
						mNavItems.push(item);
					});
					// console.log('NavigationComponentBase.ngOnInit.mMenuData', mNavItems);
					const mNavLinks: INavLink[] = [];
					mNavItems.forEach((item) => {
						mNavLinks.push(this.populateNavLink(item));
					})
					this.menuData.update(() => mNavLinks);
					this.firstLevel.nativeElement.innerHTML = ''
					this._GWCommon.buildUL(this.firstLevel.nativeElement, mNavItems, (action: string) => { return this.onItemSelected(action); });
				}).catch((error: HttpErrorResponse) => {
					this._LoggingSvc.errorHandler(error, 'NavigationComponentBase', 'constructor');
				});
			}
		});
	}

	ngOnInit(): void {
		document.documentElement.style.setProperty('--fontColor', this.fontColor());
		document.documentElement.style.setProperty('--height', this.height());
		document.documentElement.style.setProperty('--hoverBackgroundColor', this.hoverBackgroundColor());
		document.documentElement.style.setProperty('--ulBackgroundColor', this.backgroundColor());
		document.documentElement.style.setProperty('--ulLiBackgroundColor', this.backgroundColor());
	}

	protected onItemSelected(navLink: INavLink | string): void {
		// console.log('NavigationComponentBase.onItemSelected.action', action);
		if (typeof navLink !== 'string') {
			this._NavigationSvc.navigateTo(navLink);
		} else {
			// get the NavLink from the menuData
			const mNavLink = this._GWCommon.hierarchySearch(this.menuData(), navLink, 'action') as INavLink;
			// so we only upate the "currentNaveLink" only navigate if there is no children
			if (mNavLink && mNavLink.children && mNavLink.children.length === 0) {
				this._NavigationSvc.navigateTo(mNavLink);
			}
		}
	}

	private populateNavLink(navItem: INavItem): INavLink {
		const mRetVal: INavLink = {
			action: navItem.action,
			description: navItem.description,
			disabled: false,
			icon: '',
			id: -1,
			isActive: false,
			label: navItem.label,
			link: navItem.action,
			linkBehavior: 0,
			linkText: navItem.label,
			isRouterLink: false,
			styleClass: '',
			parentId: -1,
			routerLinkActive: '',
			children: []
		};
		if (navItem.items && navItem.items.length > 0) {
			navItem.items.forEach((item) => {
				const mNavItem = this.populateNavLink(item);
				mRetVal.children.push(mNavItem);
			})
		}
		return mRetVal;
	}

	private populateNavItem(navLink: INavLink): INavItem {
		const mRetVal: INavItem = {
			action: navLink.action,
			description: navLink.description,
			items: [],
			label: navLink.label,
			parentId: navLink.parentId,
		};
		if (navLink.children && navLink.children.length > 0) {
			navLink.children.forEach((item) => {
				const mNavItem = this.populateNavItem(item);
				mRetVal.items.push(mNavItem);
			})
		}
		return mRetVal;
	}

	private printMenuData(response: INavLink[]): void {
		if (this._MenuType === MenuTypes.Hierarchical || this._MenuType === MenuTypes.Vertical) {
			console.log('NavigationComponentBase.ngOnInit.response', response);
		} else {
			response.forEach((item) => {
				console.log('NavigationComponentBase.ngOnInit.item', item);
			})
		}
	}
}

