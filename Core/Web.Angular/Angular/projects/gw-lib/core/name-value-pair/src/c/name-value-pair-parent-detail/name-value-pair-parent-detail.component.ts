import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { SecurityService, ISecurityInfo } from '@growthware/core/security';
// Feature
import { NameValuePairService } from '../../name-value-pairs.service';
import { INvpParentProfile, NvpParentProfile } from '../../name-value-pair-parent-profile.model';

@Component({
  selector: 'gw-core-name-value-pair-parent-detail',
  standalone: true,
  imports: [
    // Angular
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    // Angular Material
    MatButtonModule,
    MatFormFieldModule,
    MatGridListModule,
    MatSelectModule,
    MatTabsModule,
  ],
  templateUrl: './name-value-pair-parent-detail.component.html',
  styleUrls: ['./name-value-pair-parent-detail.component.scss']
})
export class NameValuePairParentDetailComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

  private _Action: string = '';
  private _Profile: INvpParentProfile = new NvpParentProfile();

  selectedStatus: number = 0;
  validStatus  = [
    { id: 1, name: 'Active' },
    { id: 2, name: 'Inactive' },
    { id: 3, name: 'Disabled' }
  ];
  isReadonly: boolean = true;

  constructor(
    profileSvc: NameValuePairService,
    loggingSvc: LoggingService,
    modalSvc: ModalService,
    securitySvc: SecurityService,
    private _FormBuilder: FormBuilder,
    private _Router: Router,
  ) {
    super();
    this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
    this._ModalSvc = modalSvc;
    this._ProfileSvc = profileSvc;
    this._SecuritySvc = securitySvc;
  }

  ngOnInit(): void {
    this.createForm();
    console.log('NameValuePairParentDetailComponent.ngOnInit modalReason: ', this._ProfileSvc.modalReason);

    this._SecuritySvc.getSecurityInfo('search_name_value_pairs').then((securityInfo: ISecurityInfo) => {  // Request #1
      if(securityInfo != null) {                                                                          // Response Handler #1
        this._SecurityInfo = securityInfo;
      }
      return this._ProfileSvc.getParentProfile();                                                         // Request #2
    }).catch().then((response: INvpParentProfile) => {
      if (response) {                                                                                     // Response Handler #2
        // console.log('NameValuePairParentDetailComponent.ngOnInit this._Profile', this._Profile);
        this.canSave = this._SecurityInfo.mayEdit;
        if (this._ProfileSvc.modalReason === 'Add') {
          this.isReadonly = false;
        }
        this._Profile = response;
        this.frmProfile.patchValue(this._Profile);
        this.selectedStatus = this._Profile.status;
      }
    });
  }

  override delete(): void {
    throw new Error('Method not implemented.');
  }

  getErrorMessage(fieldName: string) {
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (fieldName) {
      case 'schemaName':
        if (this.controls['schemaName'].hasError('required')) {
          return 'Required';
        }
        break;
      case 'staticName':
        if (this.controls['staticName'].hasError('required')) {
          return 'Required';
        }
        break;
      default:
        break;
    }
    return undefined;
  }

  override createForm(): void {
    this.frmProfile = this._FormBuilder.group({
      description: [this._Profile.description],
      display: [this._Profile.display],
      schemaName: [this._Profile.schemaName, [Validators.required]],
      staticName: [this._Profile.staticName, [Validators.required]],
      statusSeqId: [this._Profile.status],
    });
  }

  override populateProfile(): void {
    this._Profile.description = this.controls['description'].getRawValue();
    this._Profile.display = this.controls['display'].getRawValue();
    this._Profile.schemaName = this.controls['schemaName'].getRawValue();
    this._Profile.staticName = this.controls['staticName'].getRawValue();
    this._Profile.status = this.selectedStatus;
    // console.log('NameValuePairParentDetailComponent.populateProfile', this._Profile);
    this._ProfileSvc.saveNameValuePairParent(this._Profile);
  }

  override save(): void {
    this.onCloseModal();
  }

  onCloseModal(): void {
    this._ModalSvc.close(this._ProfileSvc.addEditModalId);
  }

}
