import { Component } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ISideNavLink, SideNavLink } from '@Growthware/Lib/src/lib/models';

export const animateText = trigger('animateText', [
  state(
    'hide',
    style({
      width: 0,
      opacity: 0,
    })
  ),
  state(
    'show',
    style({
      width: 150,
      opacity: 1,
    })
  ),
  transition('hide => show', animate('3500ms ease-in')), // not working
  transition('show => hide', animate('200ms ease-out')), // not working
]);

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [animateText],
})
export class AppComponent {
  title = 'FrontEnd';
  showFiller = false;
  sideNavLinks: Array<ISideNavLink> = [];

  constructor() {
    let mSideNavLink = new SideNavLink('home', '', 'Home');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('dialpad', 'counter', 'Counter');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('thermostat', 'fetch-data', 'Fetch Data');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('api', 'swagger', 'API');
    this.sideNavLinks.push(mSideNavLink);
    mSideNavLink = new SideNavLink('manage_accounts', 'search-accounts', 'Search');
    this.sideNavLinks.push(mSideNavLink);
  }
}
