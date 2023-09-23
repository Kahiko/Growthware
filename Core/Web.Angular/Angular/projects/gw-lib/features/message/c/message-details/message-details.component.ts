import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Library
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { MessageService } from '../../message.service';
import { ModalService } from '@Growthware/features/modal';
import { IMessageProfile, MessageProfile } from '../../message-profile.model';

@Component({
  selector: 'gw-lib-message-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './message-details.component.html',
  styleUrls: ['./message-details.component.scss']
})
export class MessageDetailsComponent implements OnInit {

  private _Profile: IMessageProfile = new MessageProfile();

  frmMessage!: FormGroup;

  constructor(
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _MessageSvc: MessageService,
    private _ModalSvc: ModalService,
  ) { }

  ngOnInit(): void {
    this.populateForm(); // need to make sure the form controls are set ¯\_(ツ)_/¯
    let mIdToGet = -1;
    // console.log('editRow', this._MessageSvc.editRow);
    if(this._MessageSvc.editReason.toLowerCase() != "newprofile") {
      mIdToGet = this._MessageSvc.editRow.MessageSeqId;
    }
    // console.log('mIdToGet', mIdToGet);
    
    this._MessageSvc.getProfile(mIdToGet).then((response: IMessageProfile) => {
      // console.log('getProfile', this._MessageSvc.editRow);
      this._Profile = response;
      console.log('MessageDetailsComponent.ngOnInit.getProfile', this._Profile);
      this.populateForm();
      this.applySecurity();
    }).catch((error: any) => {
      this._LoggingSvc.toast("Error getting the message :\r\n" + error, 'Message Details:', LogLevel.Error);
    })
  }

  private applySecurity(): void {
    // nothing atm
  }

  get controls() {
    return this.frmMessage.controls;
  }

  closeModal(): void {
    // console.log('GroupDetailsComponent.closeModal.editReason', this._GroupSvc.editReason);
    // console.log('GroupDetailsComponent.closeModal.editModalId', this._GroupSvc.editModalId);
    if(this._MessageSvc.editReason.toLowerCase() !== "newprofile") {
      this._ModalSvc.close(this._MessageSvc.editModalId);
    } else {
      this._ModalSvc.close(this._MessageSvc.addModalId);
    }
  }
  onSubmit(form: FormGroup): void {
    this.closeModal();
  }

  onCancel(): void {
    this.closeModal();
  }

  private populateForm(): void {
    this.frmMessage = this._FormBuilder.group({
      avalibleTags: [this._Profile.avalibleTags],
      description: [this._Profile.description],
      messageBody: [this._Profile.body, Validators.required],
      name: [{value : this._Profile.name, disabled: this._Profile.id > -1}],
      title: [this._Profile.title, Validators.required],
    });
  }
}
