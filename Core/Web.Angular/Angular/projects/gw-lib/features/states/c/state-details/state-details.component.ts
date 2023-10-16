import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@Growthware/shared/components';
import { DataService } from '@Growthware/shared/services';
import { LoggingService } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
import { SecurityService } from '@Growthware/features/security';
// Feature
import { IStateProfile, StateProfile } from '../../state-profile.model';
import { StatesService } from '../../states.service';

@Component({
  selector: 'gw-lib-state-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    MatTabsModule
  ],
  templateUrl: './state-details.component.html',
  styleUrls: ['./state-details.component.scss']
})
export class StateDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

  private _Profile: IStateProfile = new StateProfile();

  constructor(
    private _FormBuilder: FormBuilder,
    dataSvc: DataService,
    loggingSvc: LoggingService,
    modalSvc: ModalService,
    profileSvc: StatesService,
    securitySvc: SecurityService,
  ) {
    super();
    this._DataSvc = dataSvc;
    this._LoggingSvc = loggingSvc;
    this._ModalSvc = modalSvc;
    this._ProfileSvc = profileSvc;
    this._SecuritySvc = securitySvc;
  }

  ngOnInit(): void {
    // console.log('editReason', this._ProfileSvc.editReason);
    // console.log('editRow', this._ProfileSvc.editRow);
    let mEditId = -1;
    if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
      mEditId = this._ProfileSvc.editRow.FunctionSeqId;
    }
    console.log('mEditId', mEditId);
    this.createForm();
  }

  delete() {

  }

  createForm() {
    console.log('StateDetailsComponent.createForm._Profile', this._Profile);
    this.frmProfile = this._FormBuilder.group({

    });
  }
  populateProfile() {

  }

  save() {

  }
}
