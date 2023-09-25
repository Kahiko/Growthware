import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { DataService } from '@Growthware/shared/services';
// import { GWCommon } from '@Growthware/common-code';
import { GroupService } from '@Growthware/features/group';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
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
    MatIconModule, 
    MatInputModule,
    MatTabsModule

  ],
  templateUrl: './function-details.component.html',
  styleUrls: ['./function-details.component.scss']
})
export class FunctionDetailsComponent implements OnDestroy, OnInit {

  private _Profile: IFunctionProfile = new FunctionProfile();
  private _SecurityInfo: ISecurityInfo = new SecurityInfo();
  private _Subscription: Subscription = new Subscription();

  frmProfile!: FormGroup;

  canCancel: boolean = false;
  canDelete: boolean = false;
  canSave: boolean = false;

  derivedRolesId: string = 'derivedRoles'

  groupsAvailable: Array<string> = [];
  groupsPickListName: string = 'groups';
  groupsSelected: Array<string> = [];

  rolesPickListName: string = 'roles';

  showDerived: boolean = false;
  showRoles: boolean = false;
  showGroups: boolean = false;

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
    console.log('editRow', this._ProfileSvc.editRow);
    let mEditId = -1;
    if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
      mEditId = this._ProfileSvc.editRow.FunctionSeqId;
    }
    console.log('mEditId', mEditId);
    console.log('_SecurityInfo', this._SecurityInfo);
    console.log('_GroupSvc', this._GroupSvc);
    console.log('_RoleSvc', this._RoleSvc);
    console.log('_SecuritySvc', this._SecuritySvc);

    setTimeout(() => { this._DataSvc.notifyDataChanged(this.groupsPickListName + '_AvailableItems', []); }, 500);

    this._LoggingSvc.toast('FunctionDetailsComponent.ngOnInit', 'Function Details:', LogLevel.Info);
    this.populateForm();
    this.applySecurity();

  }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  get controls() {
    return this.frmProfile.controls;
  }

  private applySecurity() {
    // nothing atm
  }

  closeModal(): void {
    this._ProfileSvc.editReason = '';
    if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
      this._ModalSvc.close(this._ProfileSvc.addModalId);
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

  private populateForm(): void {
    this.frmProfile = this._FormBuilder.group({
      name: [this._Profile.name, [Validators.required]],
    });
  }

  populateProfile(): void {
    // nothing atm
  }
}
