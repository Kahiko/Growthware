import { Component, OnInit } from '@angular/core';
// Library
import { ISideNavLink, SideNavLink } from '@Growthware/Lib/src/lib/models';

@Component({
  selector: 'gw-frontend-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss']
})
export class DefaultComponent implements OnInit {

  showSideNavLinkText = false;
  sideNavLinks: Array<ISideNavLink> = [];

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
    mSideNavLink = new SideNavLink('manage_accounts', 'search-accounts', 'Search');
    this.sideNavLinks.push(mSideNavLink);
  }

}
