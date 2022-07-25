import { Component } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';

interface ISideNavLink {
  icon: string;
  isRoute: boolean;
  liClass: string;
  linkText: string;
  routerLinkActive: string;
  routerLink: string;
}

export const animateText = trigger('animateText', [
  state(
    'hide',
    style({
      display: 'none',
      opacity: 0,
    })
  ),
  state(
    'show',
    style({
      display: 'block',
      opacity: 1,
    })
  ),
  transition('close => open', animate('350ms ease-in')),
  transition('open => close', animate('200ms ease-out')),
]);

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [
    trigger('animateText', [
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
      transition('hide => show', animate('3500ms ease-in')),
      transition('show => hide', animate('200ms ease-out')),
    ]),
  ],
})
export class AppComponent {
  title = 'FrontEnd';
  showFiller = false;
  sideNavLinks: Array<ISideNavLink> = [];

  constructor() {
    let mm = {
      icon: 'home',
      isRoute: true,
      routerLinkActive: 'link-active',
      routerLink: '/',
      liClass: 'fa fa-gears',
      linkText: 'Home',
    };
    this.sideNavLinks.push(mm);
    mm = {
      icon: 'dialpad',
      isRoute: true,
      routerLinkActive: 'link-active',
      routerLink: 'counter',
      liClass: 'fa fa-gears',
      linkText: 'Counter',
    };
    this.sideNavLinks.push(mm);
    mm = {
      icon: 'thermostat',
      isRoute: true,
      routerLinkActive: 'link-active',
      routerLink: 'fetch-data',
      liClass: 'fa fa-gears',
      linkText: 'Fetch Data',
    };
    this.sideNavLinks.push(mm);
    mm = {
      icon: 'api',
      isRoute: false,
      routerLinkActive: 'link-active',
      routerLink: 'swagger',
      liClass: 'fa fa-gears',
      linkText: 'API',
    };
    this.sideNavLinks.push(mm);
    mm = {
      icon: 'manage_accounts',
      isRoute: true,
      routerLinkActive: 'link-active',
      routerLink: 'search-accounts',
      liClass: 'fa fa-gears',
      linkText: 'Search',
    };
    this.sideNavLinks.push(mm);
  }
}
