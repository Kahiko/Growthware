import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
// Library
import { AccountService } from '@growthware/core/account';
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
// Feature
import { IFeedback, Feedback } from '../../feedback.model';
import { FeedbackService } from '../../feedback.service';

@Component({
  selector: 'gw-core-feedback-details',
  standalone: true,
  imports: [
		FormsModule,
		ReactiveFormsModule,
    MatButtonModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './feedback-details.component.html',
  styleUrl: './feedback-details.component.scss'
})
export class FeedbackDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {
  private _AccountSvc: AccountService = inject(AccountService);
  private _FormBuilder: FormBuilder = inject(FormBuilder);
  private _Profile: IFeedback = new Feedback('Anonymous', '');

  constructor(
    feedbackSvc: FeedbackService,
    loggingSvc: LoggingService,
    modalSvc: ModalService,
  ) {
    super();
    this._LoggingSvc = loggingSvc;
    this._ModalSvc = modalSvc;
    this._ProfileSvc = feedbackSvc;
  }

  ngOnInit(): void {
    this._ProfileSvc.getFeedback(this._ProfileSvc.selectedRow.FeedbackId).then((profile: IFeedback) => {  // Request/Handler #1
      this._Profile = profile;
      console.log('FeedbackDetailsComponent.ngOnInit._Profile', this._Profile);
      this.createForm();
    });
		this._Profile = new Feedback('Anonymous', '');
		this.createForm();
  }

  delete(): void {
    throw new Error('Method not implemented.');
  }

  createForm(): void {
    this.frmProfile = this._FormBuilder.group({
      action: [this._Profile.action],
      assignee: [this._Profile.assignee],
      dateOpened: [this._Profile.dateOpened],
      dateClosed: [this._Profile.dateClosed],
      details: [this._Profile.details],
      foundInVersion: [this._Profile.foundInVersion],
      notes: [this._Profile.notes],
      severity: [this._Profile.severity],
      status: [this._Profile.status],
      submittedBy: [{ value: this._Profile.submittedBy, disabled: true }]
    });
  }

  populateProfile(): void {
    const formModel = this.frmProfile.value;
    this._Profile.action = formModel.action;
    this._Profile.assignee = formModel.assignee;
    this._Profile.dateOpened = formModel.dateOpened;
    this._Profile.dateClosed = formModel.dateClosed;
    this._Profile.details = formModel.details;
    this._Profile.foundInVersion = formModel.foundInVersion;
    this._Profile.notes = formModel.notes;
    this._Profile.severity = formModel.severity;
    this._Profile.status = formModel.status;
  }

  save(): void {
    throw new Error('Method not implemented.');
  }

}
