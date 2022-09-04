import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';

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

  frmAccount!: FormGroup;

  canCancel: boolean = false;
  canDelete: boolean = false;
  canSave: boolean = false;

  showDerived: boolean = true;
  showRoles: boolean = true;
  showGroups: boolean = true;

  submitted: boolean = false;

  validStatus  = [
    { id: 1, name: "Active" },
    { id: 4, name: "Change Password" },
    { id: 3, name: "Disabled" }
  ]

  constructor(
    private _AccountSvc: AccountService,
    private _FormBuilder: FormBuilder,
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
    this.frmAccount = this._FormBuilder.group({
      account: ['', [Validators.required]],
      failedAttempts: [0],
      statusSeqId: [''],
      isSystemAdmin: [false],
    });
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

  get getControls() {
    return this.frmAccount.controls;
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'account':
        if (this.getControls['account'].hasError('required')) {
          return 'Required';
        }
        break;
      default:
        break;
    }
    return undefined;
  }

  onCancel(): void {
    this.closeModal();
  }

  onDelete(): void {
    this._LoggingSvc.toast('Account has been deleted', 'Delete Account', LogLevel.Success);
    this.closeModal();
  }

  onSubmit(form: FormGroup): void {
    console.log('Valid?', form.valid); // true or false
    console.log('Accounts', form.value.account);
    this._LoggingSvc.toast('Account has been saved', 'Save Account', LogLevel.Success);
    this.closeModal();
  }
}
