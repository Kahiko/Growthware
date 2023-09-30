import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// Library
import { AccountService } from '@Growthware/features/account';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { BaseNavigationComponent } from '../../base-navigation.component'
import { MenuService } from '../../menu.service';
// import { NavigationModule } from '../../navigation.module';
import { MenuType } from '../../menu-type.model';
import { INavLink } from '../../nav-link.model';

@Component({
  selector: 'gw-lib-vertical',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    
    MatIconModule,
    MatListModule,
  ],
  templateUrl: './vertical.component.html',
  styleUrls: ['./vertical.component.scss']
})
export class VerticalComponent extends BaseNavigationComponent implements OnInit {

  @Input() override name: string = '';

  constructor(
    accountSvc: AccountService,
    dataSvc: DataService,
    gwCommon: GWCommon,
    loggingSvc: LoggingService,
    menuListSvc: MenuService,
    runter: Router,
  ) {
    super();
    this._AccountSvc = accountSvc;
    this._DataSvc = dataSvc;
    this._GWCommon = gwCommon;
    this._LoggingSvc = loggingSvc;
    this._MenuListSvc = menuListSvc;
    this._Router = runter;
  }

  ngOnInit(): void {
    this._MenuType = MenuType.Vertical;
  }

  onItemSelected(item: INavLink) {
    if (!item.children || !item.children.length) {
      this._Router.navigate([item.link]);
    }
    if (item.children && item.children.length) {
      this.expanded = !this.expanded;
    }
  }
}
