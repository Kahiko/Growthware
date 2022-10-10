import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { AccountService, MenuListService } from '@Growthware/Lib';
import { INavLink, NavLink } from '@Growthware/Lib';
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
  selector: 'gw-frontend-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
  animations: [sideNavTextAnimation],
})
export class DefaultLayoutComponent implements OnDestroy, OnInit {
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
