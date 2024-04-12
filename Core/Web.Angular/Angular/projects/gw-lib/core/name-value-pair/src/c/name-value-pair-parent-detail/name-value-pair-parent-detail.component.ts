import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
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
// Feature
import { NameValuePairService } from '../../name-value-pairs.service';
import { INvpParentProfile, NvpParentProfile } from '../../name-value-pair-parent-profile.model';

@Component({
  selector: 'gw-core-name-value-pair-parent-detail',
  standalone: true,
  imports: [
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

  constructor(
    profileSvc: NameValuePairService,
    loggingSvc: LoggingService,
    modalSvc: ModalService,
	private _FormBuilder: FormBuilder,
	private _Router: Router,
  ) {
    super();
    this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
    this._ProfileSvc = profileSvc;
    this._ModalSvc = modalSvc;
  }

  ngOnInit(): void {
    this.createForm();
    this._ProfileSvc.getParentProfile().then((response: INvpParentProfile) => {
      if (response) {
        // console.log('NameValuePairParentDetailComponent.ngOnInit this._Profile', this._Profile);
        this._Profile = response;
        this.frmProfile.patchValue(this._Profile);
        this.selectedStatus = this._Profile.status;
      }
    }).catch((error: HttpErrorResponse | string) => {
      this._LoggingSvc.errorHandler(error, 'NameValuePairParentDetailComponent', 'ngOnInit');
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
      staticName: [this._Profile.Static_Name, [Validators.required]],
      schemaName: [this._Profile.Schema_Name, [Validators.required]],
	  display: [this._Profile.Display],
      description: [this._Profile.Description],
	  statusSeqId: [this._Profile.status],
    });
  }

  override populateProfile(): void {
    this.frmProfile = this._FormBuilder.group({

    });
  }

  override save(): void {
    throw new Error('Method not implemented.');
  }

  onCloseModal(): void {
    this._ModalSvc.close(this._ProfileSvc.addEditModalId);
  }

}
