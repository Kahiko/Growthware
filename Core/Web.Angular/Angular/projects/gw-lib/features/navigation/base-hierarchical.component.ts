import { Component, OnDestroy, OnInit } from '@angular/core';
import { HostBinding } from '@angular/core';
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
export abstract class BaseHierarchicalComponent implements OnDestroy, OnInit {
  
  private _Subscription: Subscription = new Subscription();

  expanded!: boolean;
  @HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;

  showSideNavLinkText!: boolean;
  navLinks: Array<INavLink> = [];

  depth!: number;
  item!: INavLink;  

  protected _AccountSvc!: AccountService;
  protected _GWCommon!: GWCommon;
  protected _DataSvc!: DataService;
  protected _LoggingSvc!: LoggingService;
  protected _MenuListSvc!: MenuService;
  protected _Router!: Router;

  dataName: string = '';
  menuType: MenuType = MenuType.Hierarchical;

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    if(this._GWCommon.isNullOrEmpty(this.dataName)) {
      this._LoggingSvc.toast('the "dataName" property is required', 'BaseHierarchicalComponent', LogLevel.Error);
    } else {
      this._Subscription.add(
        this._AccountSvc.authenticationResponse$.subscribe((value) => { 
          this._MenuListSvc.getNavLinks(this.menuType, this.dataName);
        })
      );
      this._Subscription.add(
        this._DataSvc.dataChanged.subscribe((data) => {
          if(data.name.toLowerCase() === this.dataName.toLowerCase()) {
            this.navLinks = data.payLoad;
            console.log('this.dataName', this.dataName);
            console.log('BaseHierarchicalComponent.ngOnInit.navLinks', this.navLinks);
          }
        })
      );
    }
  }

  onItemSelected(item: INavLink) {
    if (!item.children || !item.children.length) {
      this._Router.navigate([item.link]);
      // this.navService.closeNav();
    }
    if (item.children && item.children.length) {
      this.expanded = !this.expanded;
    }
  }
}
