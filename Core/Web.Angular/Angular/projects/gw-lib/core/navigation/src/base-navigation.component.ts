import { Component, AfterContentInit, OnDestroy, HostBinding } from '@angular/core';

import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { AccountService } from '@growthware/core/account';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LogLevel, LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
// Feature
import { INavLink } from './nav-link.model';
import { NavigationService } from './navigation.service';
import { MenuTypes } from './menu-types.enum';

@Component({
	selector: 'gw-core-base-search',
	standalone: true,
	imports: [],
	template: '',
	styles: [
	]
})
export abstract class BaseNavigationComponent implements AfterContentInit, OnDestroy {
  
	private _Subscription: Subscription = new Subscription();

	expanded!: boolean;
  @HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;
  depth: number = 0;
  navLinks: Array<INavLink> = [];
  showSideNavLinkText!: boolean;
  
  protected _AccountSvc!: AccountService;
  protected _GWCommon!: GWCommon;
  protected _DataSvc!: DataService;
  protected _LoggingSvc!: LoggingService;
  protected _MenuType: MenuTypes = MenuTypes.Hierarchical;
  protected _ModalSvc!: ModalService;
  protected _NavigationSvc!: NavigationService;
  protected _Router!: Router;

  name: string = '';

  ngOnDestroy(): void {
  	this._Subscription.unsubscribe();
  }

  ngAfterContentInit(): void {
  	// console.log('BaseNavigationComponent.ngAfterContentInit._ModalSvc', this._ModalSvc);
  	if(this._GWCommon.isNullOrEmpty(this.name)) {
  		this._LoggingSvc.toast('the "name" property is required', 'BaseHierarchicalComponent', LogLevel.Error);
  	} else {
  		this._Subscription.add(
  			this._AccountSvc.triggerMenuUpdate$.subscribe(() => { 
  				this._NavigationSvc.getNavLinks(this._MenuType, this.name);
  			})
  		);
  		this._Subscription.add(
  			this._DataSvc.dataChanged$.subscribe((data) => {
  				if(data.name.toLowerCase() === this.name.toLowerCase()) {
  					// console.log('this.dataName', this.dataName);
  					// console.log('BaseHierarchicalComponent.ngOnInit.navLinks', this.navLinks);
  					this.navLinks = data.value;
  				}
  			})
  		);
  	}    
  }

  protected onItemSelected(item: INavLink) {
  	// console.log('BaseNavigationComponent.onItemSelected.item', item);
  	if (item.children && item.children.length) {
  		this.expanded = !this.expanded;
  	}
  	this._NavigationSvc.navigateTo(item);
  }
}
