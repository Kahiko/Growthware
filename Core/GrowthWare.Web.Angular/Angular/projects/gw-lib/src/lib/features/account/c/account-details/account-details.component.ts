import { Component, OnInit } from '@angular/core';

import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

import { IAccountProfile } from '../../account-profile.model';
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {
  private _AccountProfile!: IAccountProfile;

  constructor(
    private _AccountSvc: AccountService,
    private _LoggingSvc: LoggingService
    ) { }

  ngOnInit(): void {
    console.log(this._AccountSvc.accountId);
    this._AccountSvc.getAccountById(this._AccountSvc.accountId).then((accountProfile: IAccountProfile) => {
      console.log(accountProfile);
    }).catch((reason) => {
      this._LoggingSvc.toast(reason, 'Account Error:', LogLevel.Error);
    });
  }

}
