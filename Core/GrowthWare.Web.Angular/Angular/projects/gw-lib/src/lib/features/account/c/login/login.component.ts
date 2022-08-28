import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ModalService } from '@Growthware/Lib';
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

  constructor(
    private _AccountSvc: AccountService,
    private _FormBuilder: FormBuilder,
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
    console.log(this.loginForm);
    this._ModalSvc.close(this._AccountSvc.loginModalId);
  }

}
