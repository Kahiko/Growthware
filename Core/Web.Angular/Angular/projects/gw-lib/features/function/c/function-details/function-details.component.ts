import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { DataService } from '@Growthware/shared/services';
// import { GWCommon } from '@Growthware/common-code';
// import { GroupService } from '@Growthware/features/group';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalService, IModalOptions, ModalOptions } from '@Growthware/features/modal';
// import { RoleService } from '@Growthware/features/role';
import { PickListModule } from '@Growthware/features/pick-list';
import { SecurityService, ISecurityInfo, SecurityInfo } from '@Growthware/features/security';
import { SnakeListModule } from '@Growthware/features/snake-list';
// Feature
import { FunctionService } from '../../function.service';
import { IFunctionProfile, FunctionProfile } from '../../function-profile.model';

@Component({
  selector: 'gw-lib-function-details',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    PickListModule,
    ReactiveFormsModule,
    SnakeListModule,

    MatButtonModule, 
    MatFormFieldModule, 
    MatIconModule, 
    MatInputModule,
    MatSelectModule,
    MatTabsModule

  ],
  templateUrl: './function-details.component.html',
  styleUrls: ['./function-details.component.scss']
})
export class FunctionDetailsComponent implements OnDestroy, OnInit {

  private _Profile: IFunctionProfile = new FunctionProfile();
  private _SecurityInfo: ISecurityInfo = new SecurityInfo();
  private _Subscription: Subscription = new Subscription();
  @ViewChild('helpAction') private _HelpAction!: TemplateRef<any>;
  @ViewChild('helpSource') private _HelpSource!: TemplateRef<any>;
  @ViewChild('helpControl') private _HelpControl!: TemplateRef<any>;
  private _HelpOptions: IModalOptions = new ModalOptions('help', 'Help', '', 1);

  frmProfile!: FormGroup;

  canDelete: boolean = false;
  canSave: boolean = false;

  derivedRolesId: string = 'derivedRoles'

  groupsAvailable: Array<string> = [];
  groupsPickListName: string = 'groups';
  groupsSelected: Array<string> = [];

  groupsPickListNameView: string = 'viewGroups';
  groupsPickListNameAdd: string = 'addGroups';
  groupsPickListNameEdit: string = 'editGroups';
  groupsPickListNameDelete: string = 'deleteGroups';

  rolesPickListNameView: string = 'viewRoles';
  rolesPickListNameAdd: string = 'addRoles';
  rolesPickListNameEdit: string = 'editRoles';
  rolesPickListNameDelete: string = 'deleteRoles';

  selectedFunctionType: number = 1;

  showRoles: boolean = false;
  showGroups: boolean = false;
  
  validFunctionTypes = [
    {id: 1, text:	'Module'},
    {id: 2, text:	'Security'},
    {id: 3, text:	'Menu Item'},
    {id: 4, text:	'Calendar'},
    {id: 5, text:	'File Manager'},   
  ];

  validLinkBehaviors = [
    {id: 1, text:	'Internal'},
    {id: 2, text:	'Popup'},
    {id: 3, text:	'External'},
    {id: 4, text:	'NewPage'},
  ];

  validNavigationTypes = [
    {id: 1, text:	'Horizontal'},
    {id: 2, text:	'Vertical'},
    {id: 3, text:	'Hierarchical'},
  ];

  constructor(
    private _ProfileSvc: FunctionService,
    private _FormBuilder: FormBuilder,
    private _DataSvc: DataService,
    // private _GroupSvc: GroupService,
    // private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    // private _RoleSvc: RoleService,
    private _SecuritySvc: SecurityService    
  ) { }

  ngOnInit(): void {
    // console.log('editReason', this._ProfileSvc.editReason);
    console.log('editRow', this._ProfileSvc.editRow);
    let mEditId = -1;
    if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
      mEditId = this._ProfileSvc.editRow.FunctionSeqId;
    }
    console.log('mEditId', mEditId);
    console.log('_SecurityInfo', this._SecurityInfo);
    // console.log('_GroupSvc', this._GroupSvc);
    // console.log('_RoleSvc', this._RoleSvc);
    /**
     * FunctionSecurity
     * View_Function_Role_Tab
     * View_Function_Group_Tab
     */
    this._SecuritySvc.getSecurityInfo('FunctionSecurity').then((securityInfo) => {  // #1 Request/Handler
      // console.log('securityInfo', securityInfo);
      this._SecurityInfo = securityInfo;
      this.canSave = this._SecurityInfo.mayEdit || this._SecurityInfo.mayAdd;
      this.canDelete = this._SecurityInfo.mayDelete
      return this._SecuritySvc.getSecurityInfo('View_Function_Role_Tab');           // #2 Request
    }).catch((error) => {                                                           // Request #1 Error Handler
      this._LoggingSvc.toast("Error getting function security:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((roleSecurity) => {                                                     // Request #2 Handler
      // console.log('roleSecurity', roleSecurity);
      if(roleSecurity) {
        this.showRoles = roleSecurity.mayView;
      }
      return this._SecuritySvc.getSecurityInfo('View_Function_Group_Tab');          // #3 Request
    }).catch((error) => {                                                           // Request #2 Error Handler
      this._LoggingSvc.toast("Error getting security for the groups tab:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((groupSecurity) => {                                                    // Request #3 Handler
      // console.log('groupSecurity', groupSecurity);
      if(groupSecurity) {
        this.showGroups = groupSecurity.mayView;
      }      
      return this._ProfileSvc.getFunction(mEditId);                                 // #4 Request
    }).catch((error) => {
      this._LoggingSvc.toast("Error getting function:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((profile) => {                                                          // Request #4 Handler
      console.log('profile', profile);
      if(profile) {
        this._Profile = profile;
      }
      this.applySecurity();
      this.populateForm();
    }).catch((error) => {                                                           // Request #4 Error Handler
      this._LoggingSvc.toast("Error getting function:\r\n" + error, 'Function Details:', LogLevel.Error);
    })

    setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListName + '_AvailableItems', []); }, 500);

    this._LoggingSvc.toast('FunctionDetailsComponent.ngOnInit', 'Function Details:', LogLevel.Info);
    this.populateForm();
  }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
    this._ProfileSvc.editReason = '';
  }

  get controls() {
    return this.frmProfile.controls;
  }

  private applySecurity() {
    // nothing atm
  }

  closeModal(): void {
    if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
      this._ModalSvc.close(this._ProfileSvc.editModalId);
    } else {
      this._ModalSvc.close(this._ProfileSvc.addModalId);
    }
  }

  onSubmit(form: FormGroup): void {
    this.populateProfile();
  }

  onCancel(): void {
    this.closeModal();
  }

  onDelete(): void {
    // nothing atm
  }

  onHelp(controleName: string): void {
    switch (controleName) {
      case 'Action':
        this._HelpOptions.contentPayLoad = this._HelpAction;
        break;
      case 'Source':
        this._HelpOptions.contentPayLoad = this._HelpSource;
        break
      case 'Control':
        this._HelpOptions.contentPayLoad = this._HelpControl;
        break;
      default:
        break;
    }
    this._ModalSvc.open(this._HelpOptions);
  }  

  private populateForm(): void {
    this.selectedFunctionType = this._Profile.functionTypeSeqId;
    this.frmProfile = this._FormBuilder.group({
      action: [this._Profile.action, [Validators.required]],
      description: [this._Profile.description],
      enableViewState: [this._Profile.enableViewState],
      enableNotifications: [this._Profile.enableNotifications],
      id: [{value: this._Profile.id, disabled: true}],
      isNavigable: [this._Profile.isNavigable],
      linkBehavior: [this._Profile.linkBehavior],
      // functionTypeSeqId: [this._Profile.functionTypeSeqId],
      groups: [this._Profile.groups],
      name: [this._Profile.name],
      metaKeywords: [this._Profile.metaKeywords],
      navigationTypeSeqId: [this._Profile.navigationTypeSeqId],
      notes: [this._Profile.notes],
      noUI: [this._Profile.noUI],
      parentId: [this._Profile.parentId],
      redirectOnTimeout: [this._Profile.redirectOnTimeout],
      roles: [this._Profile.roles],
      source: [this._Profile.source],
      controller: [this._Profile.controller],
    });
  }

  populateProfile(): void {
    // nothing atm
  }
}
