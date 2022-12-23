import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';

import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

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

  selectedStatus: number = 0;
  selectedTimeZone: number = 0;

  showDerived: boolean = true;
  showRoles: boolean = true;
  showGroups: boolean = true;

  submitted: boolean = false;

  validStatus  = [
    { id: 1, name: "Active" },
    { id: 4, name: "Change Password" },
    { id: 3, name: "Disabled" }
  ];

  validTimezones = [
    { id: -10,  name: "Hawaii (GMT -10)"              },
    { id: -9,   name: "Alaska (GMT -9)"               },
    { id: -8,   name: "Pacific Time (GMT -8)"         },
    { id: -7,   name: "Mountain Time (GMT -7)"        },
    { id: -6,   name: "Central Time (GMT -6)"         },
    { id: -5,   name: "Eastern Time (GMT -5)"         },
    { id: -4,   name: "Atlantic Time (GMT -4)"        },
    { id: -3,   name: "Brasilia Time (GMT -3)"        },
    { id: 0,    name: "Greenwich Mean Time (GMT +0)"  },
    { id: 1,    name: "Central Europe Time (GMT +1)"  },
    { id: 2,    name: "Eastern Europe Time (GMT +2)"  },
    { id: 3,    name: "Middle Eastern Time (GMT +3)"  },
    { id: 4,    name: "Abu Dhabi Time (GMT +4)"       },
    { id: 5,    name: "Indian Time (GMT +5)"          },
    { id: 8,    name: "Eastern China Time (GMT +8)"   },
    { id: 9,    name: "Japan Time (GMT +9)"           },
    { id: 10,   name: "Australian Time (GMT +10)"     },
    { id: 11,   name: "Pacific Rim Time (GMT +11)"    },
    { id: 12,   name: "New Zealand Time (GMT +12)"    },
  ];

  constructor(
    private _AccountSvc: AccountService,
    private _FormBuilder: FormBuilder,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router
    ) { }

  ngOnInit(): void {
    let mDesiredAccount: string = '';
    if(this._Router.url === '/search-accounts') {
      mDesiredAccount = this._AccountSvc.account;
      this.canCancel = true;
    } else {
      mDesiredAccount = this._AccountSvc.currentAccount;
    }
    this._AccountSvc.getAccount(mDesiredAccount).then((accountProfile: IAccountProfile) => {
      this._AccountProfile = accountProfile;
      this.populateForm();
    }).catch((reason) => {
      this._LoggingSvc.toast(reason, 'Account Error:', LogLevel.Error);
    });

    if(this._Router.url === '/edit-my-account') {
      this.canDelete = false;
    } else {
      // TODO: add more logic to check authorization
    }
    // TODO: add more logic to check authorization and show/hide Save button
    this.populateForm();
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

  private populateForm(): void {
    if(!this._GWCommon.isNullOrUndefined(this._AccountProfile)) {
      this.selectedStatus = this._AccountProfile.status;
      this.selectedTimeZone = this._AccountProfile.timeZone;
      this.frmAccount = this._FormBuilder.group({
        account: [this._AccountProfile.account, [Validators.required]],
        email: [this._AccountProfile.email],
        failedAttempts: [0],
        firstName: [this._AccountProfile.firstName],
        isSystemAdmin: [false],
        lastName: [this._AccountProfile.lastName],
        middleName: [this._AccountProfile.middleName],
        preferredName: [this._AccountProfile.preferredName],
        statusSeqId: [this._AccountProfile.status],
        timeZone: [this._AccountProfile.timeZone],
      });
    } else {
      this.frmAccount = this._FormBuilder.group({
        account: ['', [Validators.required]],
        email: [''],
        failedAttempts: [0],
        firstName: [''],
        isSystemAdmin: [false],
        lastName: [''],
        middleName: [''],
        preferredName: [''],
        statusSeqId: [1],
        timeZone: [-10],
      });
    }
  }
}
