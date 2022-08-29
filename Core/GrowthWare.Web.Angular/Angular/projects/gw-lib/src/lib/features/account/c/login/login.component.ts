import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalService } from '@Growthware/Lib/src/lib/features/modal';
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  get getControls() {
    return this.loginForm.controls;
  }

  submitted: boolean = false;

  constructor(
    private _AccountSvc: AccountService,
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
  ) { }

  ngOnInit(): void {
    this.loginForm = this._FormBuilder.group({
      account: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4)]]
    })
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'account':
        if (this.loginForm.controls['account'].hasError('required')) {
          return 'You must enter a value';
        }
        break;
      case 'password':
        if (this.loginForm.controls['password'].hasError('required')) {
          return 'You must enter a value';
        }
        if (this.loginForm.controls['password'].hasError('minlength')) {
          return 'Value must be at least 4 character';
        }
        break;
      default:
        break;
    }
    return undefined;
  }

  onSubmit(){
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
        return;
    }
    console.log(this.loginForm);
    this._AccountSvc.authenticate(this.loginForm.value['account'], this.loginForm.value['password']).then((response: boolean | string) => {
      if(response === true) {
        this._ModalSvc.close(this._AccountSvc.loginModalId);
      } else {
        this._LoggingSvc.toast('The Account or Password are incorrect', 'Login Error', LogLevel.Error);
      }
    });
  }

}
