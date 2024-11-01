import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSelectModule } from '@angular/material/select';
// Library
import { GWCommon } from '@growthware/common/services';
import { GroupService } from '@growthware/core/group';
import { ISecurityInfo } from '@growthware/core/security';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { PickListComponent } from '@growthware/core/pick-list';
import { RoleService } from '@growthware/core/role';
import { SecurityService } from '@growthware/core/security';
import { SnakeListComponent } from '@growthware/core/snake-list';
// Feature
import { IAccountProfile } from '../../account-profile.model';
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-core-account-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatTabsModule,
    PickListComponent,
    SnakeListComponent
  ],
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {
  private _AccountProfile!: IAccountProfile;
  private _SecurityInfoAccount: null | ISecurityInfo = null;
  private _SecurityInfoGroups: null | ISecurityInfo = null;
  private _SecurityInfoRoles: null | ISecurityInfo = null;

  frmAccount!: FormGroup;

  canCancel: boolean = false;
  canDelete: boolean = false;
  canSave: boolean = false;

  derivedRoles: string[] = [];
  derivedRolesId: string = 'derivedRoles';

  groupsAvailable: Array<string> = [];
  groupsPickListName: string = 'groups';
  groupsSelected: Array<string> = [];

  isRegistration: boolean = false;
  isSysAdmin: boolean = false;

  litAccountWarning: string = '';
  litEMailWarning: string = '';
  litFirstNameWarning: string = '';
  litLastNameWarning: string = '';
  litStatusWarning: string = '';

	pickListTableContentsBackground = this._AccountSvc.clientChoices().evenRow;
	pickListTableContentsFont = this._AccountSvc.clientChoices().evenFont;
	pickListTableHeaderBackground = this._AccountSvc.clientChoices().oddRow;

  rolesAvailable: Array<string> = ['one', 'two'];
  rolesSelected: Array<string> = [];
  rolesPickListName: string = 'roles';

  selectedStatus: number = 0;
  selectedTimeZone: number = 0;

  showDerived: boolean = false;
  showGroups: boolean = false;
  showRoles: boolean = false;

  submitted: boolean = false;

  title: string = 'Account Information';

  validStatus  = [
    { id: 1, name: 'Active' },
    { id: 4, name: 'Change Password' },
    { id: 3, name: 'Disabled' }
  ];

  validTimezones = [
    { id: -10,  name: 'Hawaii (GMT -10)'              },
    { id: -9,   name: 'Alaska (GMT -9)'               },
    { id: -8,   name: 'Pacific Time (GMT -8)'         },
    { id: -7,   name: 'Mountain Time (GMT -7)'        },
    { id: -6,   name: 'Central Time (GMT -6)'         },
    { id: -5,   name: 'Eastern Time (GMT -5)'         },
    { id: -4,   name: 'Atlantic Time (GMT -4)'        },
    { id: -3,   name: 'Brasilia Time (GMT -3)'        },
    { id: 0,    name: 'Greenwich Mean Time (GMT +0)'  },
    { id: 1,    name: 'Central Europe Time (GMT +1)'  },
    { id: 2,    name: 'Eastern Europe Time (GMT +2)'  },
    { id: 3,    name: 'Middle Eastern Time (GMT +3)'  },
    { id: 4,    name: 'Abu Dhabi Time (GMT +4)'       },
    { id: 5,    name: 'Indian Time (GMT +5)'          },
    { id: 8,    name: 'Eastern China Time (GMT +8)'   },
    { id: 9,    name: 'Japan Time (GMT +9)'           },
    { id: 10,   name: 'Australian Time (GMT +10)'     },
    { id: 11,   name: 'Pacific Rim Time (GMT +11)'    },
    { id: 12,   name: 'New Zealand Time (GMT +12)'    },
  ];

  constructor(
    private _AccountSvc: AccountService,
    private _FormBuilder: FormBuilder,
    private _GroupSvc: GroupService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _RoleSvc: RoleService,
    private _Router: Router,
    private _SecuritySvc: SecurityService
  ) { }

  ngOnInit(): void {
    let mDesiredAccount: string = '';
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (this._Router.url) {
      case '/accounts':
        mDesiredAccount = this._AccountSvc.selectedRow.Account;
        if(this._AccountSvc.modalReason.toLowerCase() == 'newprofile') {
          mDesiredAccount = 'new';
        }
        this.canCancel = true;
        break;
      case '/accounts/edit-my-account':
        this._AccountSvc.modalReason = 'EditProfile';
        mDesiredAccount = this._AccountSvc.authenticationResponse().account;
        this.canDelete = false;
        break;
      case '/accounts/register':
        mDesiredAccount = 'new';
        this._AccountSvc.modalReason = 'RegisterProfile';
        this.canDelete = false;
        this.isRegistration = true;
        this.title = 'Register Account';
        break;
      default:
        break;
    }
    this._GroupSvc.getGroups().then((groups) => {                         // Request and Response Handler #1 (getGroups)
      if(groups != null) {
        this.groupsAvailable = groups;
      }
      return this._RoleSvc.getRoles();                                    // Request #2 (getRoles)
    }).catch((error) => {                                                 // Error Handler #1 (getGroups)
      this._LoggingSvc.toast('Error getting groups:\r\n' + error, 'Account Details:', LogLevel.Error);
    }).then((roles) => {                                                  // Response Handler #2 (getRoles)
      if(roles != null) {
        this.rolesAvailable = roles;
      }
      return this._SecuritySvc.getSecurityInfo('Accounts');               // Request #3 (getSecurityInfo('Accounts'))
    }).catch((error) => {                                                 // Error Handler #2 (getRoles)
      this._LoggingSvc.toast('Error getting roles:\r\n' + error, 'Account Details:', LogLevel.Error);
    }).then((reasonSecurityInfo) => {                                     // Response Handler #3 (getSecurityInfo('Accounts'))
      if(reasonSecurityInfo != null) {
        this._SecurityInfoAccount = reasonSecurityInfo;
      }
      return this._SecuritySvc.getSecurityInfo('View_Account_Group_Tab'); // Request #4 (getSecurityInfo('View_Account_Group_Tab'))
    }).catch((error) => {                                                 // Error Handler #3 (getSecurityInfo('Accounts'))
      this._LoggingSvc.toast('Error getting Security Info for Accounts:\r\n' + error, 'Account Details:', LogLevel.Error);
    }).then((groupTabSecurityInfo) => {                                   // Response Handler #4 (getSecurityInfo('View_Account_Group_Tab'))
      if(groupTabSecurityInfo != null) {
        this._SecurityInfoGroups = groupTabSecurityInfo;
      }
      return this._SecuritySvc.getSecurityInfo('View_Account_Role_Tab');  // Request #5 (getSecurityInfo('View_Account_Role_Tab'))
    }).catch((error) => {                                                 // Error Handler #4 (getSecurityInfo('View_Account_Group_Tab'))
      this._LoggingSvc.toast('Error getting Security Info for Group Tab:\r\n' + error, 'Account Details:', LogLevel.Error);
    }).then((roleTabSecurityInfo) => {                                    // Response Handler #5 (getSecurityInfo('View_Account_Role_Tab'))
      if(roleTabSecurityInfo != null) {
        this._SecurityInfoRoles = roleTabSecurityInfo;
      }
      return this._AccountSvc.getAccountForEdit(mDesiredAccount);         // Request #6 getAccountForEdit(mDesiredAccount)
    }).catch((error) => {                                                 // Error Handler #5 (getSecurityInfo('View_Account_Role_Tab'))
      this._LoggingSvc.toast('Error getting Security Info for Role Tab:\r\n' + error, 'Account Details:', LogLevel.Error);
    }).then((accountProfile) => {                                         // Response Handler #6 getAccountForEdit(mDesiredAccount)
      if(accountProfile != null) {
        this._AccountProfile = accountProfile;
        let mRoles: string[] = [];
        let mGroups: string[] = [];
        this.isSysAdmin = this._AccountProfile.isSystemAdmin;
        if(this._AccountProfile && this._AccountProfile.assignedRoles) {
          this.rolesSelected = this._AccountProfile.assignedRoles;
        }
        if(this._AccountProfile && this._AccountProfile.groups) {
          mGroups = this._AccountProfile.groups!;
          this.groupsSelected = mGroups;
        }
        this.applySecurity();
        this.populateForm();
      }
    }).catch((error) => {                                                 // Error Handler #6 getAccountForEdit(mDesiredAccount)
      this._LoggingSvc.toast('Error getting Account:\r\n' + error, 'Account Details:', LogLevel.Error);
    });
    this.applySecurity();
    this.populateForm();
  }

  private applySecurity() {
    switch (this._AccountSvc.modalReason.toLowerCase()) {
      case 'newprofile':
        this.canDelete = false;
        this.showDerived = false;
        if(this._SecurityInfoAccount != null) {
          this.canSave = this._SecurityInfoAccount.mayEdit;
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
      this._ModalSvc.close(this._AccountSvc.modalReason + '_Id');
      if(this._AccountSvc.modalReason.toLocaleLowerCase() === 'newprofile') {
        this._ModalSvc.close(this._AccountSvc.addEditModalId);
      }
      if(this._AccountSvc.modalReason.toLocaleLowerCase() === 'editprofile') {
        this._ModalSvc.close(this._AccountSvc.addEditModalId);
      }
    }
    this._AccountSvc.modalReason = '';
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
        if(this.isRegistration) {
          if (this.controls['email'].hasError('required')) {
            return 'Required';
          }            
        }
        if (this.controls['email'].hasError('email')) {
          return 'Not a valid email';
        }    
        break;
      case 'firstName':
        if(this.isRegistration) {
          if (this.controls['firstName'].hasError('required')) {
            return 'Required';
          }  
        }
        break;
      case 'lastName':
        if(this.isRegistration) {
          if (this.controls['lastName'].hasError('required')) {
            return 'Required';
          }  
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
    // this._AccountSvc.delete(this._AccountProfile.id).then((response) => {
    //   console.log('AccountDetailsComponent.onDelete', response);
    //   this._LoggingSvc.toast('Account has been deleted', 'Delete Account', LogLevel.Success);
    //   this.closeModal();
    // }).catch((error) => {
    //   this._LoggingSvc.toast('Error deleting account!', 'Delete Account', LogLevel.Error);
    // })
  }

  onSubmit(form: FormGroup): void {
    // console.log('Valid?', form.valid); // true or false
    if(form.valid) {
      this.submitted = true;
      this.populateProfile();
      if(!this.isRegistration) {
        this._AccountSvc.saveAccount(this._AccountProfile).then((response) => {
          if (response) {
            this._LoggingSvc.toast('Account has been saved', 'Save Account', LogLevel.Success);
            this.closeModal();  
          } else {
            this._LoggingSvc.toast('Account was not saved', 'Save Account', LogLevel.Error);
            this.closeModal();
          }
        }).catch((error: unknown) => {
          console.log('error', error);
          this._LoggingSvc.toast('Error saving account!', 'Save Account', LogLevel.Error);
        });          
      } else {
        // console.log('AccountProfile', this._AccountProfile);
        this._AccountSvc.registerAccount(this._AccountProfile).then((mMessage: string) => {
          this._LoggingSvc.toast(mMessage, 'Register Account', LogLevel.Success);
          this.closeModal();
        }).catch((error: unknown) => {
          console.log('error', error);
          this._LoggingSvc.toast('Error registring account!', 'Registration', LogLevel.Error);
        });
      }
    }
  }

  private populateForm(): void {
    if(!this._GWCommon.isNullOrUndefined(this._AccountProfile)) {
      this.selectedStatus = this._AccountProfile.status;
      this.selectedTimeZone = this._AccountProfile.timeZone;
      this.derivedRoles = this._AccountProfile.derivedRoles;
      if(!this.isRegistration) {
        this.frmAccount = this._FormBuilder.group({
          account: [this._AccountProfile.account, [Validators.required]],
          email: [this._AccountProfile.email, [Validators.required, Validators.email]],
          enableNotifications: [this._AccountProfile.enableNotifications],
          failedAttempts: [this._AccountProfile.failedAttempts],
          firstName: [this._AccountProfile.firstName],
          isSystemAdmin :[{value : this._AccountProfile.isSystemAdmin, disabled: !this._AccountSvc.authenticationResponse().isSystemAdmin}],
          lastName: [this._AccountProfile.lastName],
          location: [this._AccountProfile.location],
          middleName: [this._AccountProfile.middleName],
          preferredName: [this._AccountProfile.preferredName],
          statusSeqId: [this._AccountProfile.status],
          timeZone: [this._AccountProfile.timeZone],
        });        
      } else {
        this.frmAccount = this._FormBuilder.group({
          account: [this._AccountProfile.account],
          email: [this._AccountProfile.email, [Validators.required, Validators.email]],
          enableNotifications: [this._AccountProfile.enableNotifications],
          failedAttempts: [this._AccountProfile.failedAttempts],
          firstName: [this._AccountProfile.firstName, [Validators.required]],
          isSystemAdmin :[{value : this._AccountProfile.isSystemAdmin, disabled: !this._AccountSvc.authenticationResponse().isSystemAdmin}],
          lastName: [this._AccountProfile.lastName, [Validators.required]],
          location: [this._AccountProfile.location],
          middleName: [this._AccountProfile.middleName],
          preferredName: [this._AccountProfile.preferredName],
          statusSeqId: [this._AccountProfile.status],
          timeZone: [this._AccountProfile.timeZone],
        });        
      }
      
    } else {
      this.frmAccount = this._FormBuilder.group({
        account: ['', [Validators.required]],
        email: [''],
        enableNotifications: [false],
        failedAttempts: [0],
        firstName: [''],
        isSystemAdmin :[{value : false, disabled: !this._AccountSvc.authenticationResponse().isSystemAdmin}],
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
    if(this.isRegistration) {
      this._AccountProfile.account = this.controls['email'].getRawValue();
    }
    this._AccountProfile.assignedRoles = JSON.parse(JSON.stringify(this.rolesSelected));
    this._AccountProfile.email = this.controls['email'].getRawValue();
    this._AccountProfile.enableNotifications = this.controls['enableNotifications'].getRawValue();
    if (this.showRoles) {
      this._AccountProfile.failedAttempts = this.controls['failedAttempts'].getRawValue();      
      this._AccountProfile.status = this.selectedStatus;
    }
    this._AccountProfile.firstName = this.controls['firstName'].getRawValue();
    this._AccountProfile.groups = JSON.parse(JSON.stringify(this.groupsSelected));
    if (this.isSysAdmin) {
      this._AccountProfile.isSystemAdmin = this.controls['isSystemAdmin'].getRawValue();      
    }
    this._AccountProfile.lastName = this.controls['lastName'].getRawValue();
    this._AccountProfile.location = this.controls['location'].getRawValue();
    this._AccountProfile.middleName = this.controls['middleName'].getRawValue();
    this._AccountProfile.preferredName = this.controls['preferredName'].getRawValue();
    this._AccountProfile.timeZone = this.selectedTimeZone;
  }
}
