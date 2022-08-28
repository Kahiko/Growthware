import { Component, OnInit } from '@angular/core';
// Library
import { MenuListService } from '@Growthware/Lib';
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

  constructor(private _MenuListSvc: MenuListService) { }

  ngOnInit(): void {
    let mNavLink = new NavLink('home', 'home', 'Home');
    this.verticalNavLinks.push(mNavLink);
    mNavLink = new NavLink('dialpad', 'counter', 'Counter');
    this.verticalNavLinks.push(mNavLink);
    mNavLink = new NavLink('thermostat', 'fetch-data', 'Fetch Data');
    this.verticalNavLinks.push(mNavLink);
    mNavLink = new NavLink('api', 'swagger', 'API', false);
    this.verticalNavLinks.push(mNavLink);
    // Nested Administration link
    mNavLink = new NavLink('admin_panel_settings', '', 'Administration', false);
    let mAdminChild = new NavLink('manage_accounts', 'search-accounts', 'Manage Accounts');
    mNavLink.children.push(mAdminChild)
    mAdminChild = new NavLink('functions', 'search-functions', 'Manage Functions');
    mNavLink.children.push(mAdminChild)
    this.verticalNavLinks.push(mNavLink);

  }

  onShowSideNavLinkText(): void {
    this.showSideNavLinkText = !this.showSideNavLinkText;
    this._MenuListSvc.setShowNavText(this.showSideNavLinkText);
  }
}
