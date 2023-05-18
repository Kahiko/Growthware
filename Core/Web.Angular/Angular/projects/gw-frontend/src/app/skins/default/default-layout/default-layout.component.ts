import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { AccountService, MenuListService } from '@Growthware/Lib';
import { MenuType, INavLink } from '@Growthware/Lib';
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
  selector: 'gw-frontend-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
  animations: [sideNavTextAnimation],
  encapsulation: ViewEncapsulation.None,
})
export class DefaultLayoutComponent implements OnDestroy, OnInit {
  private _Subscriptions: Subscription = new Subscription();

  showSideNavLinkText!: boolean;
  verticalNavLinks: Array<INavLink> = [];

  constructor(
    private _AccountSvc: AccountService,
    private _MenuListSvc: MenuListService
  ) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.showSideNavLinkText = this._MenuListSvc.getShowNavText();
    this._Subscriptions.add(
      this._AccountSvc.sideNav$.subscribe((navLinks)=>{
        this.verticalNavLinks = navLinks;
      })
    );
    this._Subscriptions.add(
      this._MenuListSvc.showNavText$.subscribe((value)=>{
        this.showSideNavLinkText = value;
      })
    );
    this._Subscriptions.add(
      this._AccountSvc.authenticationResponse$.subscribe((_)=>{
        this._AccountSvc.getNavLinks(MenuType.Hierarchical);
      })
    );
  }

  onShowSideNavLinkText(): void {
    this._MenuListSvc.setShowNavText(!this.showSideNavLinkText);
  }
}
