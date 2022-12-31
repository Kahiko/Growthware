import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { GroupService } from '@Growthware/Lib/src/lib/features/group';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalService } from '@Growthware/Lib/src/lib/features/modal';
import { RoleService } from '@Growthware/Lib/src/lib/features/role';

import { IAccountProfile } from '../../account-profile.model';
import { ISecurityInfo } from '../../security-info.model';
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

  showDerived: boolean = false;
  showRoles: boolean = false;
  showGroups: boolean = false;

  private _SecurityInfoAccount: null | ISecurityInfo = null;
  private _SecurityInfoGroups: null | ISecurityInfo = null;
  private _SecurityInfoRoles: null | ISecurityInfo = null;

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
    private _GroupSvc: GroupService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _RoleSvc: RoleService,
    private _Router: Router
    ) { }

  ngOnInit(): void {
    let mDesiredAccount: string = '';

    switch (this._Router.url) {
      case '/search-accounts':
        mDesiredAccount = this._AccountSvc.account;
        this.canCancel = true;
        break;
      case '/edit-my-account':
        this._AccountSvc.reason = 'EditProfile';
        mDesiredAccount = this._AccountSvc.currentAccount;
        this.canDelete = false;
        break;
      default:
        break;
    }
    // Request #1 in the chain
    this._GroupSvc.getGroups().then((response) => {
      // Response Handler #1
      if(response != null) {
        // set the avalible groups
      }
      // Request #2
      return this._RoleSvc.getRoles();
    }).catch((reason) => {
      this._LoggingSvc.toast(reason, 'Error AccountDetailsComponent.ngOnInit - getAccount:', LogLevel.Error);
    }).then((response) => {
      // Response Handler #2
      if(response != null) {
        // set the avalible groups
      }
      // Request #3
      return this._AccountSvc.getSecutiryInfo(this._AccountSvc.reason);
    }).catch((reason) => {
      this._LoggingSvc.toast(reason, 'Error AccountDetailsComponent.ngOnInit - getSecutiryInfo:', LogLevel.Error);
    }).then((response) => {
      // Response Handler #3
      if(response != null) {
        this._SecurityInfoAccount = response;
      }
      // Request #4
      return this._AccountSvc.getSecutiryInfo('View_Account_Group_Tab');
    }).then((response)=>{
      // Response Handler #4
      if(response != null) {
        this._SecurityInfoGroups = response;
      }
      // Request #5
      return this._AccountSvc.getSecutiryInfo('View_Account_Role_Tab');
    }).then((response) => {
      // Response Handler #5
      if(response != null) {
        this._SecurityInfoRoles = response;
      }
      // Request #6
      return this._AccountSvc.getAccount(mDesiredAccount);
    }).then((accountProfile) => {
      if(accountProfile != null) {
        this._AccountProfile = accountProfile;
        this.applySecurity();
        this.populateForm();
      } 
    });
    this.populateForm();
  }

  private applySecurity() {
    switch (this._AccountSvc.reason.toLowerCase()) {
      case 'addaccount':
        this.canDelete = false;
        this.showDerived = true;
        break;
        case 'editaccount':
        this.showDerived = true;
        if(this._SecurityInfoAccount != null) {
          this.canDelete = this._SecurityInfoAccount.mayDelete;
          this.canSave = this._SecurityInfoAccount.mayEdit
        }
        if(this._SecurityInfoGroups != null) {
          this.showGroups = this._SecurityInfoGroups.mayView;
        }
        if(this._SecurityInfoRoles != null) {
          this.showRoles = this._SecurityInfoRoles.mayView;
        }
      break;
      default:
        break;
    }       
  }

  closeModal(): void {
    if(this._Router.url === '/search-accounts') {
      if(this._AccountSvc.reason === 'add') {
        this._ModalSvc.close(this._AccountSvc.addModalId);
      }
      if(this._AccountSvc.reason === 'EditAccount') {
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
