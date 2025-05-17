import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { ModalService } from '@growthware/core/modal';
// Feature
import { SysAdminService } from '../../sys-admin.service';
import { SelectedRow } from '../../selected-row.model';

@Component({
  selector: 'gw-core-db-log-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatTabsModule
  ],
  templateUrl: './db-log-details.component.html',
  styleUrl: './db-log-details.component.scss'
})
export class DBLogDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

  theForm: FormGroup;
  private _SysAdminSvc!: SysAdminService;
  private _Profile!: SelectedRow;

  constructor(
    private _FormBuilder: FormBuilder,
    modalSvc: ModalService,
    profileSvc: SysAdminService,
    sysAdminService: SysAdminService,
  ) {
    super();
    this.theForm = this._FormBuilder.group({});
    this._ModalSvc = modalSvc;
    this._ProfileSvc = profileSvc;
    this._SysAdminSvc = sysAdminService;
  }

  ngOnInit(): void {
    // 1.) Get data - in this case getting the data from the selectedRow is enough because the data doesn't change.
    this._Profile = this._SysAdminSvc.selectedRow;
    // 2.) create form - createForm
    this.createForm();
  }

  override delete(): void {
    throw new Error('Method not implemented.');
  }
  override createForm(): void {
    this.theForm = this._FormBuilder.group({
      account: [{ value: this._Profile.Account, disabled: true }],
      className: [{ value: this._Profile.ClassName, disabled: true }],
      component: [{ value: this._Profile.Component, disabled: true }],
      level: [{ value: this._Profile.Level, disabled: true }],
      logSeqId: [{ value: this._Profile.LogSeqId, disabled: true }],
      methodName: [{ value: this._Profile.MethodName, disabled: true }],
      msg: [{ value: this._Profile.Msg, disabled: true }],
    });
  }

  getLocation(): string {
    const component = this.theForm?.get('component')?.value || '';
    const className = this.theForm?.get('className')?.value || '';
    const methodName = this.theForm?.get('methodName')?.value || '';
    return `${component}.${className}.${methodName}()`;
  }

  override populateProfile(): void {
    throw new Error('Method not implemented.');
  }
  override save(): void {
    throw new Error('Method not implemented.');
  }
}
