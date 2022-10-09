import { Component, OnDestroy, OnInit } from '@angular/core';
// Library
import { AccountService, MenuListService } from '@Growthware/Lib';
import { INavLink, NavLink } from '@Growthware/Lib';
import { Subscription } from 'rxjs';
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
  selector: 'gw-frontend-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss'],
  animations: [sideNavTextAnimation],
})
export class DefaultComponent implements OnDestroy, OnInit {
  private _Subscriptions: Subscription = new Subscription();

  showSideNavLinkText = false;
  verticalNavLinks: Array<INavLink> = [];

  constructor(
    private _AccountSvc: AccountService,
    private _MenuListSvc: MenuListService
  ) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscriptions.add(
      this._AccountSvc.sideNavSubject.subscribe((navLinks)=>{
        this.verticalNavLinks = navLinks;
      })
    );
  }

  onShowSideNavLinkText(): void {
    this.showSideNavLinkText = !this.showSideNavLinkText;
    this._MenuListSvc.setShowNavText(this.showSideNavLinkText);
  }
}
