import { Component, OnInit } from '@angular/core';
// Library
import { ISideNavLink, SideNavLink } from '@Growthware/Lib/src/lib/models';
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
  sideNavLinks: Array<ISideNavLink> = [];
  verticalNavLinks: Array<INavLink> = [];

  constructor() { }

  ngOnInit(): void {
    let mSideNavLink = new SideNavLink('home', 'home', 'Home');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('dialpad', 'counter', 'Counter');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('thermostat', 'fetch-data', 'Fetch Data');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('api', 'swagger', 'API', false);
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('manage_accounts', 'search-accounts', 'Manage Accounts');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('functions', 'search-functions', 'Manage Functions');
    this.sideNavLinks.push(mSideNavLink);

    let mNavLink = new NavLink('home', 'home', 'Home');
    this.verticalNavLinks.push(mNavLink);
    mNavLink = new NavLink('dialpad', 'counter', 'Counter');
    this.verticalNavLinks.push(mNavLink);
    mNavLink = new NavLink('thermostat', 'fetch-data', 'Fetch Data');
    this.verticalNavLinks.push(mNavLink);
    mNavLink = new NavLink('api', 'swagger', 'API', false);
    this.verticalNavLinks.push(mNavLink);

    mNavLink = new NavLink('admin_panel_settings', '', 'Administration', false);
    let mAdminChild = new NavLink('manage_accounts', 'search-accounts', 'Manage Accounts');
    mNavLink.children.push(mAdminChild)
    mAdminChild = new NavLink('functions', 'search-functions', 'Manage Functions');
    mNavLink.children.push(mAdminChild)
    this.verticalNavLinks.push(mNavLink);

  }

}
