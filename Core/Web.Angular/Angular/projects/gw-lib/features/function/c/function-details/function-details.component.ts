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
import { GroupService } from '@Growthware/features/group';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalService, IModalOptions, ModalOptions } from '@Growthware/features/modal';
import { RoleService } from '@Growthware/features/role';
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

  avalibleParents = [{key: -1, value: 'None'}];
  
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
  selectedNavigationType: number = 1;
  selectedParentId: number = -1;
  selectedLinkBehavior: number = 1;

  showRoles: boolean = false;
  showGroups: boolean = false;
  
  validFunctionTypes = [{key: -1, value: 'None'}];

  validLinkBehaviors = [{key: -1, value: 'None'}];

  validNavigationTypes = [{key: -1, value: 'None'}];

  constructor(
    private _ProfileSvc: FunctionService,
    private _FormBuilder: FormBuilder,
    private _DataSvc: DataService,
    private _GroupSvc: GroupService,
    // private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _RoleSvc: RoleService,
    private _SecuritySvc: SecurityService    
  ) { }

  ngOnInit(): void {
    // console.log('editReason', this._ProfileSvc.editReason);
    // console.log('editRow', this._ProfileSvc.editRow);
    let mEditId = -1;
    if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
      mEditId = this._ProfileSvc.editRow.FunctionSeqId;
    }
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
      // console.log('FunctionDetailsComponent.ngOnInit.profile', profile);
      if(profile) {
        this._Profile = profile;
      }
      return this._ProfileSvc.getFuncitonTypes();                                   // #5 Request
    }).catch((error) => {                                                           // Request #4 Error Handler
      this._LoggingSvc.toast("Error getting function:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((functionTypes: any) => {                                               // Request #5 Handler
      // console.log('FunctionDetailsComponent.ngOnInit.functionTypes', functionTypes);
      this.validFunctionTypes = functionTypes;
      return this._ProfileSvc.getNavigationTypes();                                 // #6 Request
    }).catch((error) => {                                                           // Request #5 Error Handler
      this._LoggingSvc.toast("Error getting function types:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((navigationTypes: any) => {                                             // Request #6 Handler
      // console.log('FunctionDetailsComponent.ngOnInit.navigationTypes', navigationTypes);
      this.validNavigationTypes = navigationTypes;
      return this._ProfileSvc.getLinkBehaviors();                                   // #7 Request
    }).catch((error: any) => {                                                      // Request #6 Error Handler
      this._LoggingSvc.toast("Error getting navigation types:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((linkBehaviors: any) => {                                               // Request #7 Handler
      // console.log('FunctionDetailsComponent.ngOnInit.linkBehaviors', linkBehaviors);
      this.validLinkBehaviors = linkBehaviors;
      return this._ProfileSvc.getAvalibleParents();                                  // #8 Request
    }).catch((error) => {                                                            // Request #7 Error Handler
      this._LoggingSvc.toast("Error getting link behaviors:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((avalibleParents: any)=>{                                                // Request #8 Handler
      // console.log('FunctionDetailsComponent.ngOnInit.avalibleParents', avalibleParents);
      this.avalibleParents = avalibleParents;                                        // #9 Request
      return this._RoleSvc.getRoles();
    }).catch((error) => {                                                            // Request #8 Error Handler
      this._LoggingSvc.toast("Error getting avalible parents:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((roles: any) => {                                                        // Request #9 Handler
      // console.log('FunctionDetailsComponent.ngOnInit.roles', roles);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameAdd + '_AvailableItems', roles); }, 500);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameDelete + '_AvailableItems', roles); }, 500);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameEdit + '_AvailableItems', roles); }, 500);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameView + '_AvailableItems', roles); }, 500);
      return this._GroupSvc.getGroups();
    }).catch((error: any) => {                                                        // Request #9 Error Handler
      this._LoggingSvc.toast("Error getting avalible roles:\r\n" + error, 'Function Details:', LogLevel.Error);
    }).then((groups: any) => {
      // console.log('FunctionDetailsComponent.ngOnInit.groups', groups);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameAdd + '_AvailableItems', groups); }, 500);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameDelete + '_AvailableItems', groups); }, 500);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameEdit + '_AvailableItems', groups); }, 500);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameView + '_AvailableItems', groups); }, 500);
      this.applySecurity();
      this.populateForm();
    });

    setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListName + '_AvailableItems', []); }, 500);
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
    setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameAdd + '_SelectedItems', this._Profile.assignedAddRoles); }, 500);
    setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameDelete + '_SelectedItems', this._Profile.assignedDeleteRoles); }, 500);
    setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameEdit + '_SelectedItems', this._Profile.assignedEditRoles); }, 500);
    setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListNameView + '_SelectedItems', this._Profile.assignedViewRoles); }, 500);

    setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameAdd + '_SelectedItems', this._Profile.addGroups); }, 500);
    setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameDelete + '_SelectedItems', this._Profile.deleteGroups); }, 500);
    setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameEdit + '_SelectedItems', this._Profile.editGroups); }, 500);
    setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListNameView + '_SelectedItems', this._Profile.viewGroups); }, 500);

    // const mGroups = this._Profile.groups!;
    // setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListName + '_SelectedItems', mGroups); }, 500);
    this.selectedFunctionType = this._Profile.functionTypeSeqId;
    this.selectedNavigationType = this._Profile.navigationTypeSeqId;
    this.selectedParentId = this._Profile.parentId;
    this.selectedLinkBehavior = this._Profile.linkBehavior;
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
