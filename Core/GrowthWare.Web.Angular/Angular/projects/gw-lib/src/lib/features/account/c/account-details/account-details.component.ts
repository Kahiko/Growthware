import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalService } from '@Growthware/Lib/src/lib/features/modal';

import { IAccountProfile } from '../../account-profile.model';
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {
  private _AccountProfile!: IAccountProfile;

  private _ValidStatus  = [
    { "id": 1, "Name": "Active" },
    { "id": 4, "Name": "Change Password" },
    { "id": 3, "Name": "Disabled" }
]

  canCancel: boolean = false;
  canDelete: boolean = false;
  canSave: boolean = false;

  constructor(
    private _AccountSvc: AccountService,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router
    ) { }

  ngOnInit(): void {
    this._AccountSvc.getAccount(this._AccountSvc.account).then((accountProfile: IAccountProfile) => {
      console.log(accountProfile);
    }).catch((reason) => {
      this._LoggingSvc.toast(reason, 'Account Error:', LogLevel.Error);
    });
    // console.log(this._Router.url);
    if(this._Router.url === '/search-accounts') {
      this.canCancel = true;
    }

    if(this._Router.url === '/edit-my-account') {
      this.canDelete = false;
    } else {
      // TODO: add more logic to check authorization
    }
    // TODO: add more logic to check authorization and show/hide Save button
  }

  closeModal(): void {
    if(this._Router.url === '/search-accounts') {
      if(this._AccountSvc.reason === 'add') {
        this._ModalSvc.close(this._AccountSvc.addModalId);
      }
      if(this._AccountSvc.reason === 'edit') {
        this._ModalSvc.close(this._AccountSvc.editModalId);
      }
    }
    this._AccountSvc.reason = '';
  }

  onCancel(): void {
    this.closeModal();
  }

  onDelete(): void {
    this._LoggingSvc.toast('Account has been deleted', 'Delete Account', LogLevel.Success);
    this.closeModal();
  }

  onSave(): void {
    this._LoggingSvc.toast('Account has been saved', 'Save Account', LogLevel.Success);
    this.closeModal();
  }
}
