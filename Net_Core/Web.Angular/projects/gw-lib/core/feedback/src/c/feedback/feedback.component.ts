import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { CanDeactivateFn } from '@angular/router';
// Library
import { AccountService } from '@growthware/core/account';
import { LoggingService, LogLevel } from '@growthware/core/logging';
// Feature
import { FeedbackService } from '../../feedback.service';
import { Feedback, IFeedback } from '../../feedback.model';

interface ISelectedableAction {
  action: string;
  title: string;

}
@Component({
  selector: 'gw-core-feedback',
  standalone: true,
  imports: [
		ReactiveFormsModule,
    // Angular Material
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTabsModule,
  ],
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.scss'
})
export class FeedbackComponent implements OnInit {
  private _AccountSvc: AccountService = inject(AccountService);
  private _LoggingSvc: LoggingService = inject(LoggingService);
  private _FeedbackSvc: FeedbackService = inject(FeedbackService);
  private _FormBuilder: FormBuilder = inject(FormBuilder);
  private _Profile: IFeedback = new Feedback('', '');

  // This was necessary b/c this.frmSubmit.reset(); was not working
  @ViewChild('frm') theNgForm: FormGroupDirective | undefined;
  
  // areaOccurred = new FormControl<ISelectedableAction | null>(null, Validators.required);

  frmSubmit!: FormGroup;
  submitted = false;

  get controls() {
		return this.frmSubmit.controls;
	}

	validLinks: ISelectedableAction[] = [];

  ngOnInit(): void {
    this._AccountSvc.getSelectableActions().then((actions) => {
      this.validLinks = actions;
    }).catch((error) => {                                         // Error Handler #1 (getSelectableActions);
      console.log(error);
    });
    this.createForm();
  }

  createForm(): void {
    this.frmSubmit = this._FormBuilder.group({
      areaOccurred: new FormControl<ISelectedableAction | null>(null, { validators: [Validators.required] }),
      details: ['', [Validators.required]],
    });
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'details':
        if (this.frmSubmit.get('details')?.hasError('required')) {
          return 'The Details are required.';
        }
        break;
      case 'actionControl':
        break;
      default:
        break;
    }
    return undefined;
  }

  onCancel(): void {
    if (this.theNgForm) {
      this.theNgForm.resetForm();
    }
  }

  onSubmit(): void {
    console.log(this.frmSubmit);
    this.populateProfile();
    this.save();
  }

  populateProfile(): void {
    this._Profile.details = this.frmSubmit.controls['details'].value;
    this._Profile.action = this.frmSubmit.controls['areaOccurred'].value;
    this._Profile.status = 'Submitted';
    this._Profile.submittedById = this._AccountSvc.authenticationResponse().id;
  }

  save(): void {
    this._FeedbackSvc.save(this._Profile).then((respnse: boolean) => {
      if (respnse) {
        this._LoggingSvc.toast('Feedback has been submitted', 'Submit Feedback:', LogLevel.Success);
      } else {
        this._LoggingSvc.toast('Feedback could not be submitted!', 'Submit Feedback:', LogLevel.Error);
      }
    }).catch(() => {
      this._LoggingSvc.toast('Feedback could not be submitted!', 'Submit Feedback:', LogLevel.Error);
    });
    this.onCancel();
  }
}
 
export const canLeaveFeedbackPage: CanDeactivateFn<FeedbackComponent> = (component) => {
  if (component.submitted || component.frmSubmit.pristine) {
    return true;
  }
  if (component.frmSubmit.get('details')?.dirty || component.frmSubmit.get('areaOccurred')?.dirty) {
    return window.confirm('Do you really want to leave? You will lose the entered data.')
  }
  return true;
}

