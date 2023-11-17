import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@Growthware/shared/components';
import { ConfigurationService } from '@Growthware/features/configuration';
import { DataService } from '@Growthware/shared/services';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
import { IKeyValuePair, INameValuePair } from '@Growthware/shared/models';
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
    MatSelectModule,
  ],
  templateUrl: './security-entity-details.component.html',
  styleUrls: ['./security-entity-details.component.scss']
})
export class SecurityEntityDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

  private _Profile: ISecurityEntityProfile = new SecurityEntityProfile();

  canEnterName: boolean = false;
  securityEntityTranslation: string = 'Security Entity';
  securityEntityName: string = '';

  selectedDal: string = '';
  selectedEncryptionType: number = -1;
  selectedSkin:string = '';
  selectedParent: number = -1;
  selectedStatusSeqId: number = 0;
  selectedStyle:string = '';

  validDataAccessLayers: INameValuePair[] = [
    { name: 'SQLServer', payLoad: 'SQL Server' },
    { name: 'MySql', payLoad: 'MySql' },
    { name: 'Oracle', payLoad: 'Oracle' },
  ];

  validEncryptionTypes: IKeyValuePair[] = [
    { key: 0, value: 'None' },
    { key: 3, value: 'Aes' },
    { key: 2, value: 'Des' },
    { key: 1, value: 'TripleDes' },
  ];

  validSkins: INameValuePair[] = [
    { name: 'Arc', payLoad: 'Arc' },
    { name: 'Blue Arrow', payLoad: 'Blue Arrow' },
    { name: 'Dashboard', payLoad: 'Dashboard' },
    { name: 'Default', payLoad: 'Default' },
    { name: 'DevOps', payLoad: 'DevOps' },
    { name: 'Professional', payLoad: 'Professional' },
  ];

  validParents: IKeyValuePair[] = [];

  validStatuses: IKeyValuePair[] = [
    {key: 1, value: 'Active'},
    {key: 2, value: 'Inactive'}
  ];

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
    } else {
      this.canEnterName = true;
    }
    // console.log('SecurityEntityDetailsComponent.ngOnInit.mEditId', mEditId);
    this._SecuritySvc.getSecurityInfo('search_security_entities').then((securityInfo) => {  // #1 Request/Handler getSecurityInfo
      // console.log('SecurityEntityDetailsComponent.ngOnInit.securityInfo', securityInfo);
      this._SecurityInfo = securityInfo;
      return this._ProfileSvc.getValidParents(mEditId);                                     // #2 Request getValidParents
    }).catch((error: any) => {                                                              // #1 Error Handler getSecurityInfo
      this._LoggingSvc.toast("Error getting security info:\r\n" + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
    }).then((parrents: IKeyValuePair[]) => {                                                // #2 Request getValidParents Handler
      // console.log('SecurityEntityDetailsComponent.ngOnInit.parrents', parrents);
      this.validParents = parrents;
      return this._ProfileSvc.getSecurityEntity(mEditId);                                   // #3 Request getSecurityEntity
    }).catch((error: any) => {                                                              // #2 Error Handler
      this._LoggingSvc.toast("Error getting Security Entity:\r\n" + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
    }).then((profile: ISecurityEntityProfile) => {                                          // #3 Request getProfile Handler
      // console.log('SecurityEntityDetailsComponent.ngOnInit.profile', profile);
      this._Profile = profile;
      this.populateForm();
    }).catch((error: any) => {                                                              // #3 Error Handler
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
      description: [this._Profile.description, [Validators.required]],
      url: [this._Profile.url],
      dataAccessLayerAssemblyName: [this._Profile.dataAccessLayerAssemblyName, [Validators.required]],
      dataAccessLayerNamespace: [this._Profile.dataAccessLayerNamespace, [Validators.required]],
      connectionString: [this._Profile.connectionString, [Validators.required]],
    });
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'connectionString':
        if (this.controls['connectionString'].hasError('required')) {
          return 'Required';
        }
        break;
      case 'description':
        if (this.controls['description'].hasError('required')) {
          return 'Required';
        }
        break;
      case 'dataAccessLayerAssemblyName':
        if (this.controls['dataAccessLayerAssemblyName'].hasError('required')) {
          return 'Required';
        }
        break;
      case 'dataAccessLayerNamespace':
        if (this.controls['dataAccessLayerNamespace'].hasError('required')) {
          return 'Required';
        }
        break;
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

  populateForm(): void {
    this.securityEntityName = this._Profile.name;
    this.selectedDal = this._Profile.dataAccessLayer;
    this.selectedEncryptionType = this._Profile.encryptionType;
    this.selectedParent = this._Profile.parentSeqId
    this.selectedSkin = this._Profile.skin;
    this.selectedStatusSeqId = this._Profile.statusSeqId;
    this.selectedStyle = this._Profile.style;
    this.createForm();
  }

  override populateProfile(): void {
    this._Profile.connectionString = this.controls['connectionString'].getRawValue();
    this._Profile.description = this.controls['description'].getRawValue();
    this._Profile.dataAccessLayer = this.selectedDal;
    this._Profile.dataAccessLayerAssemblyName = this.controls['dataAccessLayerAssemblyName'].getRawValue();
    this._Profile.dataAccessLayerNamespace = this.controls['dataAccessLayerNamespace'].getRawValue();
    this._Profile.encryptionType = this.selectedEncryptionType;
    // this._Profile.id // No need to set
    this._Profile.name = this.controls['name'].getRawValue();
    this._Profile.parentSeqId = this.selectedParent;
    this._Profile.skin = this.selectedSkin;
    this._Profile.statusSeqId = this.selectedStatusSeqId;
    // this._Profile.style = this.selectedStyle; // legacy havent figured out how to do this yet
    this._Profile.url = this.controls['url'].getRawValue();
    // console.log('SecurityEntityDetailsComponent.populateProfile._Profile', this._Profile);
  }

  override save(): void {
    this._ProfileSvc.save(this._Profile).then((response: boolean) => {
      if(response) {
        this._LoggingSvc.toast(this.securityEntityTranslation + ' has been saved', this.securityEntityTranslation + ' Details:', LogLevel.Success);
        this.onClose();
      }
    }).catch((error: any) => {
      this._LoggingSvc.toast("Error saving " + this.securityEntityTranslation + ":\r\n" + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
    });
  }

}
