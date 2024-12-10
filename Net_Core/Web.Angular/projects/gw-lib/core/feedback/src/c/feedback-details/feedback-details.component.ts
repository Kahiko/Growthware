import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
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
    MatTabsModule
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
		this._Profile = new Feedback('Anonymous', '');
		this.createForm();
  }

  delete(): void {
    throw new Error('Method not implemented.');
  }

  createForm(): void {
    this.frmProfile = this._FormBuilder.group({
      details: [this._Profile.details, [Validators.required]],
    });
  }

  populateProfile(): void {
    throw new Error('Method not implemented.');
  }

  save(): void {
    throw new Error('Method not implemented.');
  }

}
