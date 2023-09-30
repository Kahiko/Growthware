import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { AccountService } from '@Growthware/features/account';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { BaseNavigationComponent } from '../../base-navigation.component'
import { MenuService } from '../../menu.service';
// import { INavLink } from '../../nav-link.model';

@Component({
  selector: 'gw-lib-hierarchical-horizontal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hierarchical-horizontal.component.html',
  styleUrls: ['./hierarchical-horizontal.component.scss']
})
export class HierarchicalHorizontalComponent extends BaseNavigationComponent {

  @Input() override name: string = '';
  // @Input() override depth!: number;
  // @Input() override item!: INavLink;    

  constructor(
    accountSvc: AccountService,
    dataSvc: DataService,
    gwCommon: GWCommon,
    loggingSvc: LoggingService,
    menuListSvc: MenuService,
  ) {
    super();
    this._AccountSvc = accountSvc;
    this._DataSvc = dataSvc;
    this._GWCommon = gwCommon;
    this._LoggingSvc = loggingSvc;
    this._MenuListSvc = menuListSvc;
  }
}
