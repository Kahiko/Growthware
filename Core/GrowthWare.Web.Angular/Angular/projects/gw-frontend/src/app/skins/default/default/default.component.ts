import { Component, OnInit } from '@angular/core';
// Library
import { AccountService, MenuListService } from '@Growthware/Lib';
import { INavLink, NavLink } from '@Growthware/Lib';
import { sideNavTextAnimation } from './animations/side-nav';

@Component({
  selector: 'gw-frontend-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss'],
  animations: [sideNavTextAnimation],
})
export class DefaultComponent implements OnInit {

  showSideNavLinkText = false;
  verticalNavLinks: Array<INavLink> = [];

  constructor(
    private _AccountSvc: AccountService,
    private _MenuListSvc: MenuListService
  ) { }

  ngOnInit(): void {
    this._AccountSvc.getNavLinks().then(
      (response) => {
        this.verticalNavLinks = response;
      }
    );

  }

  onShowSideNavLinkText(): void {
    this.showSideNavLinkText = !this.showSideNavLinkText;
    this._MenuListSvc.setShowNavText(this.showSideNavLinkText);
  }
}
