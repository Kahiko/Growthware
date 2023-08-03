import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
// Feature
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnDestroy, OnInit {

  private _Return: string = '';
  private _Subscription: Subscription = new Subscription();

  hideOldPassword: boolean = true;
  frmChangePassword!: FormGroup;

  constructor(
    private _AccountSvc: AccountService,
    private _ActivatedRoute: ActivatedRoute,
    private _GWCommon: GWCommon,
    private _FormBuilder: FormBuilder,
    private _Router: Router,
  ) { }

  ngOnInit(): void {
    this._ActivatedRoute.queryParams.subscribe((params) => {
      this._Return = params['return'] || '/home';
    });
    this.hideOldPassword = this._AccountSvc.authenticationResponse.status == 4;
    this.populateForm();
    this._Subscription.add(this.getControls['newPassword'].valueChanges.subscribe((data) => {
      this.onPasswordChange();
    }));
    this._Subscription.add(this.getControls['confirmPassword'].valueChanges.subscribe((data) => {
      this.onPasswordChange();
    }));
  }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  onSubmit(form: FormGroup): void {
    // logic to change password
    this._AccountSvc.changePassword(this.getControls['oldPassword'].value, this.getControls['newPassword'].value).then((response) => {
      if(response) {
        // on success change routes
        if(!this._GWCommon.isNullOrEmpty(this._Return)) {
          this._Router.navigateByUrl(this._Return);
        }
      }
    });
  }

  onPasswordChange() {
    if (this.getControls['newPassword'].value == this.getControls['confirmPassword'].value) {
      this.getControls['confirmPassword'].setErrors(null);
    } else {
      this.getControls['confirmPassword'].setErrors({ mismatch: true });
    }
  }

  get getControls() {
    return this.frmChangePassword.controls;
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'oldPassword':
        if(this._AccountSvc.authenticationResponse.status != 4) {
          if (this.getControls['oldPassword'].hasError('required')) {
            return 'Required';
          }
        }
        break;
      case 'newPassword':
        if (this.getControls['newPassword'].hasError('required')) {
          return 'Required';
        }
        break;
      case 'confirmPassword':
        let mRetVal: string = '';
        if (this.getControls['confirmPassword'].hasError('required')) {
          mRetVal = 'Required';
        } else {
          if(this.getControls['confirmPassword'].value != this.getControls['newPassword'].value) {
            mRetVal = 'New and Confirm must match exactly!';
          }
        }
        if(!this._GWCommon.isNullOrEmpty(mRetVal)) {
          return mRetVal;
        }
        break;
      default:
        break;
    }
    return undefined;
  }

  private populateForm(): void {
    if(this._AccountSvc.authenticationResponse.status == 4) {
      this.frmChangePassword = this._FormBuilder.group({
        oldPassword: ['forced change'],
        newPassword: ['', [Validators.required]],
        confirmPassword: ['', [Validators.required]],
      });
    } else {
      this.frmChangePassword = this._FormBuilder.group({
        oldPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required]],
        confirmPassword: ['', [Validators.required]],
      });

    }
  }

}
