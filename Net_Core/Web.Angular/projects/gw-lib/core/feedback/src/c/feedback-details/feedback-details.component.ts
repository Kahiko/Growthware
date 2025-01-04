import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
// Library
import { AccountService } from '@growthware/core/account';
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { ISelectedableAction } from '@growthware/core/account/src/selectedable-action.model';
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
    MatNativeDateModule,
    MatSelectModule
  ],
  templateUrl: './feedback-details.component.html',
  styleUrl: './feedback-details.component.scss'
})
export class FeedbackDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {
  private _AccountSvc: AccountService = inject(AccountService);
  private _FormBuilder: FormBuilder = inject(FormBuilder);
  private _Profile: IFeedback = new Feedback('Anonymous', '');

  public avalibleDevelopers: Array<string> = [];
  public avalibleQA: Array<string> = [];
  public avalibleStatuses: Array<string> = ['Submitted', 'Open', 'Open-in progress', 'Closed', 'Closed-cannot reproduce', 'Closed-works as designed'];
  public avalibleTypes: Array<string> = ['Bug', 'Feature-change', 'Feature-request', 'Question', 'Other'];
  selectedAction: string = '--';
  selectedStatus: string = '--';
  selectedType: string = '--';
  public validLinks: ISelectedableAction[] = [];

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
    this._ProfileSvc.getFeedbackAccounts().then((response: any) => {                // Request and Response Handler #1 (getFeedbackAccounts)
      // console.log('FeedbackDetailsComponent.ngOnInit.accounts', response);
      this.avalibleDevelopers = response.item1;
      this.avalibleQA = response.item2;
      return this._AccountSvc.getSelectableActions();                               // Request #2 (getSelectableActions)
    }).catch((error: any) => {                                                      // Error Handler #1 (getFeedbackAccounts)
      this._LoggingSvc.toast('Error getting Accounts:\r\n' + error, 'Feedback Details:', LogLevel.Error);
    }).then((actions: ISelectedableAction[]) => {                                   // Response Handler #2 (getSelectableActions)
      this.validLinks = actions;
      return this._ProfileSvc.getFeedback(this._ProfileSvc.selectedRow.FeedbackId); // Request #3 (getFeedback)
    }).catch((error: any) => {
      this._LoggingSvc.toast('Error getting Avalible Actions:\r\n' + error, 'Feedback Details:', LogLevel.Error);
    }).then((profile: IFeedback) => {                                               // Response Handler #3 (getFeedback)
      this._Profile = profile;
      this.createForm();
    }).catch((error: any) => {                                                      // Error Handler #3 (getFeedback)
      this._LoggingSvc.toast('Error getting Feedback details:\r\n' + error, 'Feedback Details:', LogLevel.Error);
    });
		this._Profile = new Feedback('Anonymous', '');
		this.createForm();
  }

  delete(): void {
    throw new Error('Method not implemented.');
  }

  createForm(): void {
    const mSelectedAction = this.validLinks.find(x => x.action.toUpperCase() === this._Profile.action.toUpperCase());
    if (mSelectedAction) {
      this.selectedAction = mSelectedAction.action;
    }
    const mSelectedStatus = this.avalibleStatuses.find(x => x.toUpperCase() === this._Profile.status.toUpperCase());
    if (mSelectedStatus) {
      this.selectedStatus = mSelectedStatus;
    }
    const mSelectedType = this.avalibleTypes.find(x => x.toUpperCase() === this._Profile.type.toUpperCase());
    if (mSelectedType) {
      this.selectedType = mSelectedType;
    }
    this.frmProfile = this._FormBuilder.group({
      action: new FormControl<ISelectedableAction | null>(null, { validators: [Validators.required] }),
      assignee: [this._Profile.assignee],
      dateOpened: [this._Profile.dateOpened],
      dateClosed: [this._Profile.dateClosed],
      details: [this._Profile.details],
      foundInVersion: [this._Profile.foundInVersion],
      notes: [this._Profile.notes],
      severity: [this._Profile.severity],
      submittedBy: [{ value: this._Profile.submittedBy, disabled: true }],
      verifiedBy: [{ value: this._Profile.verifiedBy }]
    });
  }

  populateProfile(): void {
    const mFrmProfile = this.frmProfile.value;
    this._Profile.action = mFrmProfile.action;
    this._Profile.assignee = mFrmProfile.assignee;
    this._Profile.dateOpened = mFrmProfile.dateOpened;
    this._Profile.dateClosed = mFrmProfile.dateClosed;
    this._Profile.details = mFrmProfile.details;
    this._Profile.foundInVersion = mFrmProfile.foundInVersion;
    this._Profile.notes = mFrmProfile.notes;
    this._Profile.severity = mFrmProfile.severity;
    this._Profile.status = mFrmProfile.status;
  }

  save(): void {
    throw new Error('Method not implemented.');
  }

}
