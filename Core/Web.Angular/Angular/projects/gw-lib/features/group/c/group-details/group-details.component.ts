import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatLineModule } from '@angular/material/core';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { DataService } from '@Growthware/shared/services';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
import { PickListModule } from '@Growthware/features/pick-list';
import { SearchService } from '@Growthware/features/search';
import { ISecurityInfo, SecurityService } from '@Growthware/features/security';
// Feature
import { GroupService } from '../../group.service';
import { IGroupProfile, GroupProfile } from '../../group-profile.model';

@Component({
  selector: 'gw-lib-group-details',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    PickListModule,
    ReactiveFormsModule,
    
    MatButtonModule, 
    MatFormFieldModule, 
    MatLineModule,
    MatIconModule, 
    MatInputModule,
    MatTabsModule
  ],
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.scss']
})
export class GroupDetailsComponent implements OnDestroy, OnInit {
  private _GroupProfile: IGroupProfile = new GroupProfile();
  private _Subscription: Subscription = new Subscription();

  canDelete: boolean = false;
  frmGroup!: FormGroup;
  height: number = 350;
  securityInfo!: ISecurityInfo;
  showDerived: boolean = false;
  rolesAvailable: Array<string> = [];
  rolesPickListName: string = 'roles';
  rolesSelected: Array<string> = [];
  
  constructor(
    private _DataSvc: DataService,
    private _FormBuilder: FormBuilder,
    private _GroupSvc: GroupService,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _SearchSvc: SearchService,
    private _SecuritySvc: SecurityService
  ) { }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    let mIdToGet = -1;
    if(this._GroupSvc.modalReason.toLowerCase() != "newprofile") {
      // console.log('selectedRow', this._GroupSvc.selectedRow);
      mIdToGet = this._GroupSvc.selectedRow.GroupSeqId;
    }
    this._GroupSvc.getGroupForEdit(mIdToGet).then((response: IGroupProfile) => {
      this._GroupProfile = response;
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListName + '_SelectedItems', this._GroupProfile.rolesInGroup); }, 500);
      setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListName + '_AvailableItems', this._GroupProfile.rolesNotInGroup); }, 500);
      this.populateForm();
      return this._SecuritySvc.getSecurityInfo('Manage_Groups');
    }).then((response: ISecurityInfo) => {
      this.securityInfo = response;
      this.applySecurity();
    })
    this.populateForm();
    this._Subscription.add(this._DataSvc.dataChanged.subscribe((data) => {
      // console.log('GroupDetailsComponent.ngOnInit',data.name.toLowerCase()). // used to determine the data name
      switch (data.name.toLowerCase()) {
        case 'roles':
          this._GroupProfile.rolesInGroup = data.payLoad;
          break;
        default:
          break;
      }
    }));
  }

  applySecurity(): void {
    this.canDelete = this.securityInfo.mayDelete;
    if(this._GroupSvc.modalReason.toLowerCase() == 'newprofile') {
      this.canDelete = false;
    }
  }

  get controls() {
    return this.frmGroup.controls;
  }

  closeModal(): void {
    // console.log('GroupDetailsComponent.closeModal.modalReason', this._GroupSvc.modalReason);
    // console.log('GroupDetailsComponent.closeModal.modalReason', this._GroupSvc.addEditModalId);
    this._ModalSvc.close(this._GroupSvc.addEditModalId);
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
    // console.log('GroupDetailsComponent.onDelete');
    this._GroupSvc.delete(this._GroupProfile.id).then((response) => {
      this.updateSearch();
      this._LoggingSvc.toast('The group has been deleted', 'Delete Group', LogLevel.Success);
      this.closeModal();
    }).catch((error) => {
      this._LoggingSvc.errorHandler(error, 'GroupService', 'delete');
      this._LoggingSvc.toast('The group could not be deleted', 'Delete Group', LogLevel.Error);
    });
  }

  onSubmit(form: FormGroup): void {
    this.populateProfile();
    this._GroupSvc.saveGroup(this._GroupProfile).then((response) => {
      this.updateSearch();
      this._LoggingSvc.toast('The group has been saved', 'Save Group', LogLevel.Success);
      this.closeModal();
    }).catch((_) => {
      this._LoggingSvc.toast('The group could not be saved', 'Save Group', LogLevel.Error);
    })
  }

  private populateForm(): void {
    this.frmGroup = this._FormBuilder.group({
      name: [this._GroupProfile.name, Validators.required],
      description: [this._GroupProfile.description],
    });
  }

  private populateProfile(): void {
    this._GroupProfile.name = this.controls['name'].getRawValue();
    this._GroupProfile.description = this.controls['description'].getRawValue();
  }

  updateSearch(): void {
    const mSearchCriteria = this._SearchSvc.getSearchCriteria("Groups"); // from SearchAccountsComponent line 25
    if(mSearchCriteria != null) {
      this._SearchSvc.setSearchCriteria("Groups", mSearchCriteria);
    }    
  }

}
