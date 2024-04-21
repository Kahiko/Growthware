import { Component, Input, OnInit } from '@angular/core';

import { Router } from '@angular/router';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
// Library
import { AccountService } from '@growthware/core/account';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
// Feature
import { BaseNavigationComponent } from '../../base-navigation.component';
import { NavigationService } from '../../navigation.service';
import { MenuTypes } from '../../menu-types.enum';
import { VerticalListItemComponent } from '../vertical-list-item/vertical-list-item.component';

@Component({
	selector: 'gw-core-hierarchical-vertical',
	standalone: true,
	imports: [
		VerticalListItemComponent,
		
		MatIconModule,
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
  	this._MenuType = MenuTypes.Hierarchical;
  }
}
