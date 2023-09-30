import { Component, AfterContentInit, OnDestroy, HostBinding } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { AccountService } from '@Growthware/features/account';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
// Feature
import { INavLink } from './nav-link.model';
import { MenuService } from './menu.service';
import { MenuType } from './menu-type.model';

@Component({
    selector: 'gw-lib-base-search',
    standalone: true,
    imports: [CommonModule],    
    template: ``,
    styles: [
    ]
  })
export abstract class BaseNavigationComponent implements AfterContentInit, OnDestroy {
  
  private _Subscription: Subscription = new Subscription();

  expanded!: boolean;
  @HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;
  navLinks: Array<INavLink> = [];
  showSideNavLinkText!: boolean;
  
  protected _AccountSvc!: AccountService;
  protected _GWCommon!: GWCommon;
  protected _DataSvc!: DataService;
  protected _LoggingSvc!: LoggingService;
  protected _MenuListSvc!: MenuService;
  protected _MenuType: MenuType = MenuType.Hierarchical;
  protected _Router!: Router;

  name: string = '';

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngAfterContentInit(): void {
    if(this._GWCommon.isNullOrEmpty(this.name)) {
      this._LoggingSvc.toast('the "name" property is required', 'BaseHierarchicalComponent', LogLevel.Error);
    } else {
      this._Subscription.add(
        this._AccountSvc.authenticationResponse$.subscribe((value) => { 
          this._MenuListSvc.getNavLinks(this._MenuType, this.name);
        })
      );
      this._Subscription.add(
        this._DataSvc.dataChanged.subscribe((data) => {
          if(data.name.toLowerCase() === this.name.toLowerCase()) {
            // console.log('this.dataName', this.dataName);
            // console.log('BaseHierarchicalComponent.ngOnInit.navLinks', this.navLinks);
            this.navLinks = data.payLoad;
          }
        })
      );
    }    
  }
}
