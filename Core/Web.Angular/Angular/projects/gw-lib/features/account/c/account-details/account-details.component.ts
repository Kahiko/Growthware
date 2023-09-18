import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
// Library
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { GroupService } from '@Growthware/features/group';
import { ISecurityInfo } from '@Growthware/features/security';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
import { RoleService } from '@Growthware/features/role';
import { SecurityService } from '@Growthware/features/security';
// Feature
import { IAccountProfile } from '../../account-profile.model';
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnDestroy, OnInit {
  private _AccountProfile!: IAccountProfile;
  private _SecurityInfoAccount: null | ISecurityInfo = null;
  private _SecurityInfoGroups: null | ISecurityInfo = null;
  private _SecurityInfoRoles: null | ISecurityInfo = null;
  private _Subscription: Subscription = new Subscription();

  frmAccount!: FormGroup;

  canCancel: boolean = false;
  canDelete: boolean = false;
  canSave: boolean = false;

  derivedRolesId: string = 'derivedRoles'

  groupsAvailable: Array<string> = [];
  groupsPickListName: string = 'groups';
  groupsSelected: Array<string> = [];

  litAccountWarning: string = '';
  litEMailWarning: string = '';
  litFirstNameWarning: string = '';
  litLastNameWarning: string = '';
  litStatusWarning: string = '';

  rolesAvailable: Array<string> = [];
  rolesPickListName: string = 'roles';
  rolesSelected: Array<string> = [];

  selectedStatus: number = 0;
  selectedTimeZone: number = 0;

  showDerived: boolean = false;
  showRoles: boolean = false;
  showGroups: boolean = false;

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
    private _DataSvc: DataService,
    private _GroupSvc: GroupService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _RoleSvc: RoleService,
    private _Router: Router,
    private _SecuritySvc: SecurityService
  ) { }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    let mDesiredAccount: string = '';

    switch (this._Router.url) {
      case '/accounts':
        if(this._AccountSvc.editReason.toLowerCase() != "newprofile") {
          // console.log('editRow', this._AccountSvc.editRow);
          mDesiredAccount = this._AccountSvc.editRow.Account;
        } else {
          mDesiredAccount = "new";
        }
        this.canCancel = true;
        break;
      case '/accounts/edit-my-account':
        this._AccountSvc.editReason = 'EditProfile';
        mDesiredAccount = this._AccountSvc.authenticationResponse.account;
        this.canDelete = false;
        break;
      default:
        break;
    }
    this._GroupSvc.getGroups().then((groups) => {                           // Response Handler #1
      if(groups != null) {
        // TODO: this would indicate that the pick-list component isn't loaded at this point
        // and we are simply adding a delay to give it time... need to find a better way
        // such as a different lifecycle hook?
        setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListName + '_AvailableItems', groups); }, 500);
      }
      return this._RoleSvc.getRoles();                                      // Request #2
    }).catch((error) => { 
      this._LoggingSvc.toast("Error getting groups:\r\n" + error, 'Account Details:', LogLevel.Error);
    }).then((roles) => {                                                    // Response Handler #2
      if(roles != null) {
        // TODO: this would indicate that the pick-list component isn't loaded at this point
        // and we are simply adding a delay to give it time... need to find a better way
        // such as a different lifecycle hook?
        setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListName + '_AvailableItems', roles); }, 500);
      }
      return this._SecuritySvc.getSecurityInfo('Accounts');              // Request #3
    }).catch((error) => {
      this._LoggingSvc.toast("Error getting roles:\r\n" + error, 'Account Details:', LogLevel.Error);
    }).then((reasonSecurityInfo) => {                                       // Response Handler #3
      if(reasonSecurityInfo != null) {
        this._SecurityInfoAccount = reasonSecurityInfo;
      }
      return this._SecuritySvc.getSecurityInfo('View_Account_Group_Tab');   // Request #4
    }).catch((error) => {
      this._LoggingSvc.toast("Error getting security info for 'EditAccount' :\r\n" + error, 'Account Details:', LogLevel.Error);
    }).then((groupTabSecurityInfo) => {                                     // Response Handler #4
      if(groupTabSecurityInfo != null) {
        this._SecurityInfoGroups = groupTabSecurityInfo;
      }
      return this._SecuritySvc.getSecurityInfo('View_Account_Role_Tab');    // Request #5
    }).catch((error) => {
      this._LoggingSvc.toast("Error getting security info for 'Group tab' :\r\n" + error, 'Account Details:', LogLevel.Error);
    }).then((roleTabSecurityInfo) => {                                      // Response Handler #5
      if(roleTabSecurityInfo != null) {
        this._SecurityInfoRoles = roleTabSecurityInfo;
      }
      return this._AccountSvc.getAccount(mDesiredAccount);                  // Request #6
    }).catch((error) => {
      this._LoggingSvc.toast("Error getting security info for 'Role tab' :\r\n" + error, 'Account Details:', LogLevel.Error);
    }).then((accountProfile) => {                                           // Response Handler #6
      if(accountProfile != null) {
        this._AccountProfile = accountProfile;
        let mRoles: string[] = [];
        let mGroups: string[] = [];
        if(!this._GWCommon.isNullOrUndefined(this._AccountProfile.assignedRoles)) {
          mRoles = this._AccountProfile.assignedRoles!;
          setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListName + '_SelectedItems', mRoles); }, 500);
        }
        if(!this._GWCommon.isNullOrUndefined(this._AccountProfile.groups)) {
          mGroups = this._AccountProfile.groups!;
          setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListName + '_SelectedItems', mGroups); }, 500);
        }
        this.applySecurity();
        this.populateForm();
      }
    }).catch((error) => {
      this._LoggingSvc.toast("Error getting account information :\r\n" + error, 'Account Details:', LogLevel.Error);
    });
    this._Subscription.add(this._DataSvc.dataChanged.subscribe((data) => {
      switch (data.name.toLowerCase()) {
        case "roles":
          this._AccountProfile.assignedRoles = data.payLoad;
          break;
        case "roles":
          this._AccountProfile.groups = data.payLoad;
          break
        default:
          break;
      }
    }));
    this.applySecurity();
    this.populateForm();
  }

  private applySecurity() {
    switch (this._AccountSvc.editReason.toLowerCase()) {
      case 'newprofile':
        this.canDelete = false;
        this.showDerived = false;
        if(this._SecurityInfoAccount != null) {
          this.canSave = this._SecurityInfoAccount.mayEdit
        }
        if(this._SecurityInfoGroups != null) {
          this.showGroups = this._SecurityInfoGroups.mayView;
        }
        if(this._SecurityInfoRoles != null) {
          this.showRoles = this._SecurityInfoRoles.mayView;
        }
        break;
      case 'editprofile':
        this.showDerived = true;
        if(this._SecurityInfoAccount != null) {
          this.canDelete = this._SecurityInfoAccount.mayDelete;
          this.canSave = this._SecurityInfoAccount.mayEdit;
        }
        if(this._SecurityInfoGroups != null) {
          this.showGroups = this._SecurityInfoGroups.mayView;
        }
        if(this._SecurityInfoRoles != null) {
          this.showRoles = this._SecurityInfoRoles.mayView;
        }
        break;
      case 'add':
        break;
      default:
        break;
    }
    if(this._Router.url === '/accounts/edit-my-account') {
      this.canDelete = false;
      this.showDerived = false;
      this.showGroups = false;
      this.showRoles = false;
    }
  }

  closeModal(): void {
    if(this._Router.url === '/accounts') {
      if(this._AccountSvc.editReason.toLocaleLowerCase() === 'newprofile') {
        this._ModalSvc.close(this._AccountSvc.addModalId);
      }
      if(this._AccountSvc.editReason.toLocaleLowerCase() === 'editprofile') {
        this._ModalSvc.close(this._AccountSvc.editModalId);
      }
    }
    this._AccountSvc.editReason = '';
  }

  get controls() {
    return this.frmAccount.controls;
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'account':
        if (this.controls['account'].hasError('required')) {
          return 'Required';
        }
        break;
      case 'email':
        if (this.controls['email'].hasError('required')) {
          return 'Required';
        }
        if (this.controls['email'].hasError('email')) {
          return 'Not a valid email';
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
    this._AccountSvc.delete(this._AccountProfile.id).then((response) => {
      console.log('AccountDetailsComponent.onDelete', response);
      this._LoggingSvc.toast('Account has been deleted', 'Delete Account', LogLevel.Success);
      this.closeModal();
    }).catch((error) => {
      this._LoggingSvc.toast('Error deleting account!', 'Delete Account', LogLevel.Error);
    })
  }

  onSubmit(form: FormGroup): void {
    // console.log('Valid?', form.valid); // true or false
    if(form.valid) {
      this.populateProfile();
      // console.log('AccountProfile', this._AccountProfile);
      this._AccountSvc.saveAccount(this._AccountProfile).then((response) => {
        this._LoggingSvc.toast('Account has been saved', 'Save Account', LogLevel.Success);
        this.closeModal();
      }).catch((error) => {
        
      });
    }
  }

  private populateForm(): void {
    if(!this._GWCommon.isNullOrUndefined(this._AccountProfile)) {
      this.selectedStatus = this._AccountProfile.status;
      this.selectedTimeZone = this._AccountProfile.timeZone;
      this._DataSvc.notifyDataChanged(this.derivedRolesId, this._AccountProfile.derivedRoles);
      this.frmAccount = this._FormBuilder.group({
        account: [this._AccountProfile.account, [Validators.required]],
        email: [this._AccountProfile.email, [Validators.required, Validators.email]],
        enableNotifications: [this._AccountProfile.enableNotifications],
        failedAttempts: [this._AccountProfile.failedAttempts],
        firstName: [this._AccountProfile.firstName],
        isSystemAdmin :[{value : this._AccountProfile.isSystemAdmin, disabled: !this._AccountSvc.authenticationResponse.isSystemAdmin}],
        lastName: [this._AccountProfile.lastName],
        location: [this._AccountProfile.location],
        middleName: [this._AccountProfile.middleName],
        preferredName: [this._AccountProfile.preferredName],
        statusSeqId: [this._AccountProfile.status],
        timeZone: [this._AccountProfile.timeZone],
      });
      
    } else {
      this.frmAccount = this._FormBuilder.group({
        account: ['', [Validators.required]],
        email: [''],
        enableNotifications: [false],
        failedAttempts: [0],
        firstName: [''],
        isSystemAdmin :[{value : false, disabled: !this._AccountSvc.authenticationResponse.isSystemAdmin}],
        lastName: [''],
        location: [''],
        middleName: [''],
        preferredName: [''],
        statusSeqId: [1],
        timeZone: [-10],
      });
    }
  }

  private populateProfile(): void {
    this._AccountProfile.account = this.controls['account'].getRawValue();
    // this._AccountProfile.assignedRoles = '';
    this._AccountProfile.email = this.controls['email'].getRawValue();
    this._AccountProfile.enableNotifications = this.controls['enableNotifications'].getRawValue();
    this._AccountProfile.failedAttempts = this.controls['failedAttempts'].getRawValue();
    this._AccountProfile.firstName = this.controls['firstName'].getRawValue();
    // this._AccountProfile.groups = '';
    this._AccountProfile.isSystemAdmin = this.controls['isSystemAdmin'].getRawValue();
    this._AccountProfile.lastName = this.controls['lastName'].getRawValue();
    this._AccountProfile.location = this.controls['location'].getRawValue();
    this._AccountProfile.middleName = this.controls['middleName'].getRawValue();
    this._AccountProfile.preferredName = this.controls['preferredName'].getRawValue();
    this._AccountProfile.status = this.selectedStatus;
    this._AccountProfile.timeZone = this.selectedTimeZone;
  }
}
