import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
// Library
import { AccountService } from '@Growthware/features/account';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { BaseNavigationComponent } from '../../base-navigation.component'
import { NavigationService } from '../../navigation.service';
import { NavigationModule } from '../../navigation.module';
import { MenuTypes } from '../../menu-types.enum';

@Component({
  selector: 'gw-lib-hierarchical-vertical',
  standalone: true,
  imports: [
    CommonModule,

    MatIconModule,

    NavigationModule,
  ],
  templateUrl: './hierarchical-vertical.component.html',
  styleUrls: ['./hierarchical-vertical.component.scss']
})
export class HierarchicalVerticalComponent extends BaseNavigationComponent implements OnInit {

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
    this._MenuType = MenuTypes.Hierarchical
  }
}
