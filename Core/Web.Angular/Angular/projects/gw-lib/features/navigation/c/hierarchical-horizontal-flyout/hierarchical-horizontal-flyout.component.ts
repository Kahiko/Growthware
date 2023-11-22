import { Component, ElementRef, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
// Library
import { AccountService } from '@Growthware/features/account';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
// Feature
import { BaseNavigationFlyoutComponent } from '../../base-navigation-flyout.component';
import { NavigationService } from '../../navigation.service';
import { MenuTypes } from '../../menu-types.enum';

@Component({
  selector: 'gw-lib-hierarchical-horizontal-flyout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
  ],
  templateUrl: './hierarchical-horizontal-flyout.component.html',
  styleUrls: ['./hierarchical-horizontal-flyout.component.scss'],
  encapsulation: ViewEncapsulation.ShadowDom,
})
export class HierarchicalHorizontalFlyoutComponent extends BaseNavigationFlyoutComponent implements OnInit {

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

  // onItemSelected(item: any): void {
  //   alert('hi');
  //   // super.onItemSelected(item);
  //   // this._Router.navigate([item.action]);
  // }

}