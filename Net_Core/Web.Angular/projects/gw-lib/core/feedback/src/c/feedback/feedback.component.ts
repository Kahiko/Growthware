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
  private _FormBuilder: FormBuilder = inject(FormBuilder);

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
    });
    this.createForm();
  }

  createForm(): void {
    this.frmSubmit = this._FormBuilder.group({
      areaOccurred: new FormControl<ISelectedableAction | null>(null, { validators: [Validators.required] }),
      description: ['', [Validators.required]],
    });
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'description':
        if (this.frmSubmit.get('description')?.hasError('required')) {
          return 'The Description is required.';
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
    this.onCancel();
  }
}
 
export const canLeaveFeedbackPage: CanDeactivateFn<FeedbackComponent> = (component) => {
  if (component.submitted || component.frmSubmit.pristine) {
    return true;
  }
  if (component.frmSubmit.get('description')?.dirty || component.frmSubmit.get('areaOccurred')?.dirty) {
    return window.confirm('Do you really want to leave? You will lose the entered data.')
  }
  return true;
}

