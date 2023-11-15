import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
// Library
// import { LoggingService } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
// Feature
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit, OnInit {

  @ViewChild('account') account!: ElementRef;

  loginForm!: FormGroup;

  get getControls() {
    return this.loginForm.controls;
  }

  submitted: boolean = false;

  constructor(
    private _AccountSvc: AccountService,
    private _FormBuilder: FormBuilder,
    // private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
  ) { }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.account.nativeElement.focus();
    }, 100);
  }

  ngOnInit(): void {
    this.loginForm = this._FormBuilder.group({
      account: ['Developer', [Validators.required]],
      password: ['none', [Validators.required, Validators.minLength(4)]]
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
    this._AccountSvc.logIn(this.loginForm.value['account'], this.loginForm.value['password']).then((response: boolean | string) => {
      this._ModalSvc.close(this._AccountSvc.logInModalId);
    });
  }

}
