import { Component, OnDestroy, OnInit } from '@angular/core';
import { HostBinding } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
// Feature
// import { INavLink } from './nav-link.model';
import { MenuService } from './menu.service';

@Component({
    selector: 'gw-lib-base-search',
    standalone: true,
    imports: [CommonModule],    
    template: ``,
    styles: [
    ]
  })
export abstract class BaseHierarchicalComponent implements OnDestroy, OnInit {
  expanded!: boolean;
  showSideNavLinkText!: boolean;
  @HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;

  // protected depth!: number;
  // protected item!: INavLink;  

  protected _GWCommon!: GWCommon;
  protected _DataSvc!: DataService;
  protected _LoggingSvc!: LoggingService;
  protected _MenuListSvc!: MenuService;

  dataName: string = '';

  ngOnDestroy(): void {
    //  nothing atm
  }

  ngOnInit(): void {
    console.log('BaseHierarchicalComponent.ngOnInit', this.dataName);
    if(this._GWCommon.isNullOrEmpty(this.dataName)) {
      this._LoggingSvc.toast('the "dataName" property is required', 'BaseHierarchicalComponent', LogLevel.Error);
    } else {

    }
  }
}
