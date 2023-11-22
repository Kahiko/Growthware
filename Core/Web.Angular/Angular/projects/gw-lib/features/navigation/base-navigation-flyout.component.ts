import { Component, AfterContentInit, OnDestroy, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { AccountService } from '@Growthware/features/account';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
// Feature
import { INavItem } from './nav-item.model';
import { NavigationService } from './navigation.service';
import { MenuTypes } from './menu-types.enum';

@Component({
    selector: 'gw-lib-base-search',
    standalone: true,
    imports: [CommonModule],
    template: ``,
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
        this._AccountSvc.triggerMenuUpdate$.subscribe((_) => { 
          this._NavigationSvc.getMenuData(this._MenuType, this.name);
        })
      );
      this._Subscription.add(
        this._DataSvc.dataChanged.subscribe((data) => {
          if(data.name.toLowerCase() === this.name.toLowerCase()) {
            // console.log('this.data.name', data.name);
            this._GWCommon.buildMenuData(data.payLoad).forEach((item) => {
              this.menuData.push(item);
            });
            this._GWCommon.buildUL(this.firstLevel.nativeElement, this.menuData, this.onItemSelected);
          }
        })
      );
    }    
  }

  protected onItemSelected(action: string): void {
    // console.log('BaseNavigationFlyoutComponent.onItemSelected.action', action);
    this._Router.navigate([action.toLowerCase()]);
  }
}
