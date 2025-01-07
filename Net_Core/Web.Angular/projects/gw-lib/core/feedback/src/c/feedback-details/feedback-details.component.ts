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
import { SearchService } from '@growthware/core/search';
import { ISelectedableAction } from '@growthware/core/account';
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
  private _SearchSvc: SearchService = inject(SearchService);
  private _FormBuilder: FormBuilder = inject(FormBuilder);
  private _Profile: IFeedback = new Feedback('Anonymous', '');

  avalibleDevelopers: Array<string> = [];
  avalibleQA: Array<string> = [];
  avalibleStatuses: Array<string> = ['Submitted', 'Open', 'Open-in progress', 'Closed', 'Closed-cannot reproduce', 'Closed-works as designed'];
  avalibleTypes: Array<string> = ['Bug', 'Feature-change', 'Feature-request', 'Question', 'Other'];
  selectedAction: string = '--';
  selectedStatus: string = '--';
  selectedType: string = '--';
  validLinks: ISelectedableAction[] = [];

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
    this._ProfileSvc.getFeedbackAccounts().then((response: any) => {  // Request and Response Handler #1 (getFeedbackAccounts)
      // console.log('FeedbackDetailsComponent.ngOnInit.accounts', response);
      this.avalibleDevelopers = response.item1;
      this.avalibleQA = response.item2;
      return this._AccountSvc.getSelectableActions();                 // Request #2 (getSelectableActions)
    }).catch((error: any) => {                                        // Error Handler #1 (getFeedbackAccounts)
      this._LoggingSvc.toast('Error getting Accounts:\r\n' + error, 'Feedback Details:', LogLevel.Error);
    }).then((actions: ISelectedableAction[]) => {                     // Response Handler #2 (getSelectableActions)
      this.validLinks = actions;
      return this._ProfileSvc.getFeedback(this._ProfileSvc.selectedRow.FeedbackId); // Request #3 (getFeedback)
    }).catch((error: any) => {
      this._LoggingSvc.toast('Error getting Avalible Actions:\r\n' + error, 'Feedback Details:', LogLevel.Error);
    }).then((profile: IFeedback) => {                                 // Response Handler #3 (getFeedback)
      this._Profile = profile;
      this.createForm();
    }).catch((error: any) => {                                        // Error Handler #3 (getFeedback)
      this._LoggingSvc.toast('Error getting Feedback details:\r\n' + error, 'Feedback Details:', LogLevel.Error);
    });
		this._Profile = new Feedback('Anonymous', '');
		this.createForm();
  }

  delete(): void {
    throw new Error('Method not implemented.');
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'status':
        if (this.selectedStatus === '--') {
          return 'The Status is required.';
        }
        break;
      case 'severity':
        if (this.frmProfile.get('severity')?.hasError('required') || (this.frmProfile.get('severity')?.value < 1 || this.frmProfile.get('severity')?.value > 5)) {
          return 'The Severity is required and must be between 1 and 5.';
        }
        break;
      default:
        break;
    }
    return undefined;
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
      // action: new FormControl<ISelectedableAction | null>(null, { validators: [Validators.required] }),
      assignee: [this._Profile.assignee],
      dateOpened: [this._Profile.dateOpened],
      dateClosed: [this._Profile.dateClosed],
      details: [this._Profile.details],
      foundInVersion: [this._Profile.foundInVersion],
      notes: [this._Profile.notes],
      severity: [this._Profile.severity, [Validators.required, Validators.min(1), Validators.max(5)]],
      status: [this._Profile.status, [Validators.required]],
      submittedBy: [{ value: this._Profile.submittedBy, disabled: true }],
      targetVersion: [this._Profile.targetVersion],
      verifiedBy: [{ value: this._Profile.verifiedBy }]
    });
  }

  populateProfile(): void {
    const mDateClosed = new Date(this.frmProfile.get('dateClosed')?.value);
    const mSelectedAction = this.validLinks.find(x => x.action.toUpperCase() === this.selectedAction.toUpperCase());
    const mSelectedStatus = this.avalibleStatuses.find(x => x.toUpperCase() === this.selectedStatus.toUpperCase());
    const mSelectedType = this.avalibleTypes.find(x => x.toUpperCase() === this.selectedType.toUpperCase());
    const mFrmProfile = this.frmProfile.value;

    //this._Profile.feedbackId                // We don't change this handled by the data store
    this._Profile.action = mSelectedAction?.action ?? '';
    this._Profile.assignee = mFrmProfile.assignee;
    // this._Profile.assigneeId: number;      // Set in API
    if (mDateClosed && mDateClosed.getFullYear() !== 1753) {
      this._Profile.dateClosed = mDateClosed.toISOString();
    }
    // this._Profile.dateOpened: string;      // We don't change this
    // this._Profile.details: string;         // We don't change this
    // this._Profile.foundInVersion: string;  // We don't change this
    // this._Profile.functionSeqId: number;   // We don't change this
    this._Profile.notes = mFrmProfile.notes;
    this._Profile.severity = '' + mFrmProfile.severity;
    this._Profile.status = mSelectedStatus ?? '';
    // this._Profile.submittedBy: string;     // We don't change this
    // this._Profile.submittedById: number;   // We don't change this
    this._Profile.targetVersion = mFrmProfile.targetVersion;
    this._Profile.type = mSelectedType ?? '';
    // this._Profile.updatedBy: string;       // We don't change this
    // this._Profile.updatedById: number;     // Set in API
    this._Profile.verifiedBy = mFrmProfile.verifiedBy.value;
    // this._Profile.verifiedById: number;    // Set in API using verifiedBy
  }

  save(): void {
    console.log('FeedbackDetailsComponent.save', this._Profile);
    this._ProfileSvc.save(this._Profile).then((respnse: boolean) => {
      if (respnse) {
        this._LoggingSvc.toast('Feedback has been submitted', 'Submit Feedback:', LogLevel.Success);
        const mSearchCriteria = this._SearchSvc.getSearchCriteria('Search_Feedbacks'); // from SearchAccountsComponent (this.configurationName)
        if (mSearchCriteria != null) {
          this._SearchSvc.setSearchCriteria('Search_Feedbacks', mSearchCriteria);
        }
        this.onClose();
      } else {
        this._LoggingSvc.toast('Feedback could not be submitted!', 'Submit Feedback:', LogLevel.Error);
      }
    }).catch(() => {
      this._LoggingSvc.toast('Feedback could not be submitted!', 'Submit Feedback:', LogLevel.Error);
    });
    // throw new Error('Method not implemented.');
  }

	setToCurrentTime(elementId: string, event: Event): void {
    // not sure why this (click) is bubbling up to to the parent, but save was being called after this event
    event.preventDefault();
		const mNow = new Date();
		this.controls[elementId].setValue(mNow);
	}
}
