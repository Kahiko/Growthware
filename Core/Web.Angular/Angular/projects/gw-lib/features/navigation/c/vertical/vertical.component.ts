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
import { ModalService } from '@Growthware/features/modal';
// Feature
import { BaseNavigationComponent } from '../../base-navigation.component'
import { NavigationService } from '../../navigation.service';
import { MenuTypes } from '../../menu-types.enum';
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
    menuListSvc: NavigationService,
    modalSvc: ModalService,
    runter: Router,
  ) {
    super();
    this._AccountSvc = accountSvc;
    this._DataSvc = dataSvc;
    this._GWCommon = gwCommon;
    this._LoggingSvc = loggingSvc;
    this._NavigationSvc = menuListSvc;
    this._ModalSvc = modalSvc;
    this._Router = runter;
  }

  ngOnInit(): void {
    this._MenuType = MenuTypes.Vertical;
  }

  override onItemSelected(item: INavLink) {
    super.onItemSelected(item);
  }
}