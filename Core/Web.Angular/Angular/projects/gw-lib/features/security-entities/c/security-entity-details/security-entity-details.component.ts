import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@Growthware/shared/components';
import { ConfigurationService } from '@Growthware/features/configuration';
import { DataService } from '@Growthware/shared/services';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
import { SecurityService } from '@Growthware/features/security';
// Feature
import { SecurityEntityService } from '../../security-entity.service';
import { ISecurityEntityProfile, SecurityEntityProfile } from '../../security-entity-profile.model';

@Component({
  selector: 'gw-lib-security-entity-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,


    MatButtonModule,
    MatIconModule,
    MatInputModule,
  ],
  templateUrl: './security-entity-details.component.html',
  styleUrls: ['./security-entity-details.component.scss']
})
export class SecurityEntityDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

  private _Profile: ISecurityEntityProfile = new SecurityEntityProfile();

  canEnterName: boolean = false;
  securityEntityTranslation: string = 'Security Entity';
  securityEntityName: string = '';

  constructor(
    private _FormBuilder: FormBuilder,
    private _ConfigurationSvc: ConfigurationService,
    dataSvc: DataService,
    loggingSvc: LoggingService,
    modalSvc: ModalService,
    profileSvc: SecurityEntityService,
    securitySvc: SecurityService
  ) {
    super();
    this._DataSvc = dataSvc;
    this._LoggingSvc = loggingSvc;
    this._ModalSvc = modalSvc;
    this._ProfileSvc = profileSvc;
    this._SecuritySvc = securitySvc;
  }

  ngOnInit(): void {
    // console.log('SecurityEntityDetailsComponent.ngOnInit.editReason', this._ProfileSvc.editReason);
    // console.log('SecurityEntityDetailsComponent.ngOnInit.editRow', this._ProfileSvc.editRow);
    // console.log('SecurityEntityDetailsComponent.ngOnInit._Profile', this._Profile);
    this._Subscription.add(
      this._ConfigurationSvc.securityEntityTranslation$.subscribe((val: string) => { 
        this.securityEntityTranslation = val;
      })
    );
    let mEditId = -1;
    if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
      mEditId = this._ProfileSvc.editRow.SecurityEntitySeqId;
    }
    // console.log('SecurityEntityDetailsComponent.ngOnInit.mEditId', mEditId);
    this._SecuritySvc.getSecurityInfo('search_security_entities').then((securityInfo) => {  // #1 Request/Handler getSecurityInfo
      // console.log('SecurityEntityDetailsComponent.ngOnInit.securityInfo', securityInfo);
      this._SecurityInfo = securityInfo;
      return this._ProfileSvc.getSecurityEntity(mEditId);                                   // #2 Request getSecurityEntity
    }).catch((error: any) => {                                                              // #1 Error Handler getSecurityInfo
      this._LoggingSvc.toast("Error getting security info:\r\n" + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
    }).then((profile: ISecurityEntityProfile) => {                                          // #2 Request getProfile Handler
      // console.log('SecurityEntityDetailsComponent.ngOnInit.profile', profile);
      this._Profile = profile;
      this.populateForm();
    }).catch((error: any) => {                                                              // #2 Error Handler
      this._LoggingSvc.toast("Error getting Security Entity:\r\n" + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
    });
    this.createForm();
  }

  override delete(): void {
    throw new Error('Method not implemented.');
  }
  override createForm(): void {
    this.frmProfile = this._FormBuilder.group({
      name: [this._Profile.name, [Validators.required]],
    });
  }

  populateForm(): void {
    this.securityEntityName = this._Profile.name;
    this.createForm();
  }

  override populateProfile(): void {
    // do nothing atm
  }

  override save(): void {
    this._LoggingSvc.toast(this.securityEntityTranslation + ' has been saved', this.securityEntityTranslation + ' Details:', LogLevel.Success);
    this.onClose();
  }

}
