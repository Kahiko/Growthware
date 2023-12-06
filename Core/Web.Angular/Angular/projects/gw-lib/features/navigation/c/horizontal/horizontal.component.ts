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
import { NavigationService } from '../../navigation.service';
import { MenuTypes } from '../../menu-types.enum';
import { INavLink } from '../../nav-link.model';

@Component({
  selector: 'gw-lib-horizontal',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    
    MatIconModule,
    MatListModule,
  ],
  templateUrl: './horizontal.component.html',
  styleUrls: ['./horizontal.component.scss']
})
export class HorizontalComponent extends BaseNavigationComponent implements OnInit {

  @Input() override name: string = '';

  constructor(
    accountSvc: AccountService,
    dataSvc: DataService,
    gwCommon: GWCommon,
    loggingSvc: LoggingService,
    menuListSvc: NavigationService,
    runter: Router,
  ) {
    super();
    this._AccountSvc = accountSvc;
    this._DataSvc = dataSvc;
    this._GWCommon = gwCommon;
    this._LoggingSvc = loggingSvc;
    this._NavigationSvc = menuListSvc;
    this._Router = runter;
  }

  ngOnInit(): void {
    this._MenuType = MenuTypes.Horizontal;
  }

  override onItemSelected(item: INavLink) {
    super.onItemSelected(item);
  }
}
