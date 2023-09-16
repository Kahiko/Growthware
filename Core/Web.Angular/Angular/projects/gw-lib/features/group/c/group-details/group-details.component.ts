import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
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
export class GroupDetailsComponent implements OnInit {
  private _GroupProfile: IGroupProfile = new GroupProfile();

  canDelete: boolean = false;
  frmGroup!: FormGroup;
  rolesAvailable: Array<string> = [];
  rolesPickListName: string = 'roles';
  rolesSelected: Array<string> = [];
  
  constructor(
    private _DataSvc: DataService,
    private _FormBuilder: FormBuilder,
    private _GroupSvc: GroupService,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
  ) { }

  ngOnInit(): void {
    if(this._GroupSvc.editReason.toLowerCase() != "newprofile") {
      console.log('editRow', this._GroupSvc.editRow);
      console.log('Editing a group');
      this._GroupSvc.getGroupForEdit(this._GroupSvc.editRow.GroupSeqId).then((response: IGroupProfile) => {
        this._GroupProfile = response;
        setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListName + '_SelectedItems', this._GroupProfile.rolesInGroup); }, 500);
        setTimeout(() => { this._DataSvc.notifyDataChanged(this.rolesPickListName + '_AvailableItems', this._GroupProfile.rolesNotInGroup); }, 500);
        this.populateForm();
      })
    } else{
      console.log('Adding a new group');
    }
    this.populateForm();
  }

  get controls() {
    return this.frmGroup.controls;
  }

  closeModal(): void {
    this._ModalSvc.close(this._GroupSvc.editModalId);
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
    
  }

  onSubmit(form: FormGroup): void {
    this._LoggingSvc.toast('Group has been saved', 'Save Group', LogLevel.Success);
  }

  private populateForm(): void {
    this.frmGroup = this._FormBuilder.group({
      name: [this._GroupProfile.name, Validators.required],
      description: [this._GroupProfile.description],
    });
  }
}
