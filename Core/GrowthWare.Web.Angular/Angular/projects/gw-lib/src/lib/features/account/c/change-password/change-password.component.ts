import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
// Feature
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  private _Return: string = '';

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
  }

  onSubmit(form: FormGroup): void {

  }

  changePassword() {
    // logic to change password
    // on success change routes
    if(!this._GWCommon.isNullOrEmpty(this._Return)) {
      this._Router.navigateByUrl(this._Return);
    }
  }

  get getControls() {
    return this.frmChangePassword.controls;
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'oldPassword':
        if(this._AccountSvc.authenticationResponse.status == 4) {
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
        } else if(this.getControls['confirmPassword'].value != this.getControls['newPassword']) {
          mRetVal = 'New and Confirm must match exactly!';
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
        oldPassword: [''],
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
