import { Component, ElementRef, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { Router, RouterModule } from '@angular/router';
// Library
import { AccountService } from '@growthware/core/account';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
// Feature
import { BaseNavigationFlyoutComponent } from '../../base-navigation-flyout.component';
import { NavigationService } from '../../navigation.service';
import { MenuTypes } from '../../menu-types.enum';

@Component({
	selector: 'gw-core-hierarchical-vertical-flyout',
	standalone: true,
	imports: [
		RouterModule
	],
	templateUrl: './hierarchical-vertical-flyout.component.html',
	styleUrls: ['./hierarchical-vertical-flyout.component.scss'],
	encapsulation: ViewEncapsulation.ShadowDom,
})
export class HierarchicalVerticalFlyoutComponent extends BaseNavigationFlyoutComponent implements OnInit {

  @Input() backgroundColor: string = 'lightblue'; // #1bc2a2
  @Input() hoverBackgroundColor: string = '#2c3e50'; // #2c3e50
  @Input() fontColor: string = 'white'; // #fff
  @Input() override name: string = '';
  @Input() height: string = '32px';

  @ViewChild('firstLevel', {static: false}) override firstLevel!: ElementRef<HTMLUListElement>;

  constructor(
  	accountSvc: AccountService,
  	dataSvc: DataService,
  	gwCommon: GWCommon,
  	loggingSvc: LoggingService,
  	menuListSvc: NavigationService,
  	modalSvc: ModalService,
  	router: Router,
  ) { 
  	super();
  	this._AccountSvc = accountSvc;
  	this._DataSvc = dataSvc;
  	this._GWCommon = gwCommon;
  	this._LoggingSvc = loggingSvc;
  	this._NavigationSvc = menuListSvc;
  	this._ModalSvc = modalSvc;
  	this._Router = router;
  }

  ngOnInit(): void {
  	this._MenuType = MenuTypes.Hierarchical;
  	document.documentElement.style.setProperty('--fontColor', this.fontColor);
  	document.documentElement.style.setProperty('--height', this.height);
  	document.documentElement.style.setProperty('--hoverBackgroundColor', this.hoverBackgroundColor);
  	document.documentElement.style.setProperty('--ulBackgroundColor', this.backgroundColor);
  	document.documentElement.style.setProperty('--ulLiBackgroundColor', this.backgroundColor);
  }

}