import { Component, Input } from '@angular/core';
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
import { BaseHierarchicalComponent } from '../../base-hierarchical.component'
import { MenuService } from '../../menu.service';

@Component({
  selector: 'gw-lib-hierarchical-vertical',
  standalone: true,
  imports: [
    CommonModule,

    MatIconModule,
  ],
  templateUrl: './hierarchical-vertical.component.html',
  styleUrls: ['./hierarchical-vertical.component.scss']
})
export class HierarchicalVerticalComponent extends BaseHierarchicalComponent {

  @Input() override dataName: string = '';

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
}
