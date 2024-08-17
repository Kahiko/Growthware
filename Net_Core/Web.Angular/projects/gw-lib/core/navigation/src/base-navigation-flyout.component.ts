import { Component, AfterContentInit, OnDestroy, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { AccountService } from '@growthware/core/account';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LogLevel, LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { INavItem } from '@growthware/common/interfaces';
// Feature
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
export abstract class BaseNavigationFlyoutComponent implements AfterContentInit, OnDestroy {
  
	private _Subscription: Subscription = new Subscription();

	protected _AccountSvc!: AccountService;
	protected _GWCommon!: GWCommon;
	protected _DataSvc!: DataService;
	protected _LoggingSvc!: LoggingService;
	protected _MenuType: MenuTypes = MenuTypes.Hierarchical;
	protected _ModalSvc!: ModalService;
	protected _NavigationSvc!: NavigationService;
	protected _Router!: Router;

	name: string = '';
	menuData: INavItem[] = [];

	firstLevel!: ElementRef<HTMLUListElement>;

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngAfterContentInit(): void {
		// console.log('BaseNavigationFlyoutComponent.ngAfterContentInit._ModalSvc', this._ModalSvc);
		if(this._GWCommon.isNullOrEmpty(this.name)) {
			this._LoggingSvc.toast('the "name" property is required', 'BaseHierarchicalComponent', LogLevel.Error);
		} else {
			this._Subscription.add(
				this._AccountSvc.triggerMenuUpdate$.subscribe(() => { 
					this._NavigationSvc.getMenuData(this._MenuType, this.name);
				})
			);
			this._Subscription.add(
				this._DataSvc.dataChanged$.subscribe((data) => {
					if(data.name.toLowerCase() === this.name.toLowerCase()) {
						// console.log('BaseNavigationFlyoutComponent.ngAfterContentInit.data', data);
						this._GWCommon.buildNavItems(data.value).forEach((item) => {
							this.menuData.push(item);
						});
						this._GWCommon.buildUL(this.firstLevel.nativeElement, this.menuData, (action: string) => { return this.onItemSelected(action);});
					}
				})
			);
		}    
	}

	onItemSelected(action: string): void {
		// console.log('BaseNavigationFlyoutComponent.onItemSelected.action', action);
		this._Router.navigate([action.toLowerCase()]);
	}
}
