import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
// Library
// import { GWCommon } from '@Growthware/common-code';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
import { PickListModule } from '@Growthware/features/pick-list';
import { ISecurityInfo, SecurityService } from '@Growthware/features/security';
// Feature
import { RoleService } from '../../role.service';
import { IRoleProfile, RoleProfile } from '../../role-profile.model';

@Component({
  selector: 'gw-lib-role-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    PickListModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatCheckboxModule,
    MatIconModule, 
    MatInputModule,
    MatTabsModule,
  ],
  templateUrl: './role-details.component.html',
  styleUrls: ['./role-details.component.scss']
})
export class RoleDetailsComponent implements OnDestroy, OnInit {

  private _Role: IRoleProfile = new RoleProfile();

  canAdd: boolean = false;
  canDelete: boolean = false;
  frmRole!: FormGroup;
  membersPickListName: string = 'membersList';
  securityInfo!: ISecurityInfo;

  constructor(
    private _FormBuilder: FormBuilder,
    // private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _RoleSvc: RoleService,
    private _SecuritySvc: SecurityService,
  ) { 
    this.frmRole = this._FormBuilder.group({})
  }

  ngOnInit(): void {
    this._SecuritySvc.getSecurityInfo('Search_Roles').then((securityInfo: ISecurityInfo) => { // Request #1
      // Response Hendler #1
      // console.log('RoleDetailsComponent.ngOnInit.getSecurityInfo.response', response);
      this.canAdd = securityInfo.mayAdd;
      this.canDelete = securityInfo.mayDelete;
      let mRoleSeqId: number = -1;
      if(this._RoleSvc.editRow.RoleSeqId) {
        mRoleSeqId = this._RoleSvc.editRow.RoleSeqId;
      }
      if(mRoleSeqId === -1) {
        this.canDelete = false;
      }
      return this._RoleSvc.getRoleForEdit(mRoleSeqId); // Request #2
    }).catch((error: any) => { // Request #1 error
      this._LoggingSvc.toast("Error getting security info for 'EditRole' :\r\n" + error, 'Role Details:', LogLevel.Error);
    }).then((profile) => {
      if(profile) {
        this._Role = profile;
        if(profile.isSystemOnly) {
          this.canDelete = false;
        }
      }
      this.populateForm();
    }).catch((error: any) => {
      this._LoggingSvc.errorHandler(error, 'className', 'method');
      this.populateForm();
    });
    this.populateForm();
  }

  ngOnDestroy(): void {
    // nothing atm
  }

  get controls() {
    return this.frmRole.controls;
  }

  closeModal(): void {
    // console.log('GroupDetailsComponent.closeModal.editReason', this._GroupSvc.editReason);
    // console.log('GroupDetailsComponent.closeModal.editModalId', this._GroupSvc.editModalId);
    if(this._RoleSvc.editReason.toLowerCase() !== "newprofile") {
      this._ModalSvc.close(this._RoleSvc.editModalId);
    } else {
      this._ModalSvc.close(this._RoleSvc.addModalId);
    }
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'name':
        if (this.controls['name'].hasError('required')) {
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
    this.closeModal();
  }

  onSubmit(form: FormGroup): void {
    this.closeModal();
  }

  private populateForm(): void {
    this.frmRole = this._FormBuilder.group({
      name: [this._Role.name, Validators.required],
      description: [this._Role.description],
      isSystem :[{value : false, disabled: !true}],
      isSystemOnly :[{value : false, disabled: !true}],
    });
  }
}
