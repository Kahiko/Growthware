import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
// Feature
import { IFeedback, Feedback } from '../../feedback.model';

@Component({
  selector: 'gw-core-feedback-details',
  standalone: true,
  imports: [
		FormsModule,
		ReactiveFormsModule,
    MatTabsModule
  ],
  templateUrl: './feedback-details.component.html',
  styleUrl: './feedback-details.component.scss'
})
export class FeedbackDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {
  private _FormBuilder!: FormBuilder;
  private _Profile: IFeedback = new Feedback();

  ngOnInit(): void {
		this._Profile = new Feedback();
		this.createForm();
  }

  delete(): void {
    throw new Error('Method not implemented.');
  }

  createForm(): void {
    this.frmProfile = this._FormBuilder.group({
      description: [this._Profile.description, [Validators.required]],
    });
  }

  populateProfile(): void {
    throw new Error('Method not implemented.');
  }

  save(): void {
    throw new Error('Method not implemented.');
  }

}
