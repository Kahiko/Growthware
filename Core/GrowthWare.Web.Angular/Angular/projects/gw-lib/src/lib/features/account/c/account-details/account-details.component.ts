import { AfterViewInit, Component, HostListener, OnInit } from '@angular/core';

import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

import { IAccountProfile } from '../../account-profile.model';
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements AfterViewInit, OnInit {
  private _AccountProfile!: IAccountProfile;

  @HostListener('window:resize', ['$event'])
  handleResize(event: Event) {
    this.setTabHeights();
  }

  constructor(
    private _AccountSvc: AccountService,
    private _LoggingSvc: LoggingService
    ) { }

  ngAfterViewInit(): void {
      this.setTabHeights();
  }

  ngOnInit(): void {
    this._AccountSvc.getAccount(this._AccountSvc.account).then((accountProfile: IAccountProfile) => {
      console.log(accountProfile);
    }).catch((reason) => {
      this._LoggingSvc.toast(reason, 'Account Error:', LogLevel.Error);
    });
  }

  setTabHeights() {
    const groups = document.querySelectorAll("mat-tab-group");
    const tabCardBody = document.querySelectorAll('mat-tab-group');
    console.log(tabCardBody);
    groups.forEach(group => {
      const tabCont = group.querySelectorAll("mat-tab-body");
      const wrapper = group.querySelector(".mat-tab-body-wrapper")
      const maxHeight = Math.max(...Array.from(tabCont).map(body => body.clientHeight));
      // wrapper.setAttribute("style", `min-height:${maxHeight}px;`);
    });
  }

}
