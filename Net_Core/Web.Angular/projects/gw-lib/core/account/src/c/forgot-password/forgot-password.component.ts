import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
// Library
import { ModalService, ModalOptions, WindowSize } from '@growthware/core/modal';
import { LogLevel, LoggingService } from '@growthware/core/logging';
// Feature
import { AccountService } from '../../account.service';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'gw-core-forgot-password',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent implements AfterViewInit, OnInit {

  @ViewChild('account') accountInputElement!: ElementRef<HTMLInputElement>;
  canCancel: boolean = true;
  frmForgotPassword!: FormGroup;

  constructor(
    private _AccountSvc: AccountService,
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router,
  ) { 
    const mAction = this._Router.url.split('?')[0] .replace('/', '').replace('\\','');
    if (mAction === 'accounts/forgot-password') {
      this.canCancel = false;
    }
    // console.log('ForgotPasswordComponent.constructor.mAction', mAction);
  }

  ngAfterViewInit(): void {
    this.accountInputElement.nativeElement.focus();
  }

  ngOnInit(): void {
    this.createForm();
  }

  get controls() {
    return this.frmForgotPassword.controls;
  }

  getErrorMessage(fieldName: string): string | void {
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (fieldName) {
      case 'account': {
        if (this.controls['account'].hasError('required')) {
          return 'Required';
        }
      }
    }
  }

  onCancel(): void {
		const mWindowSize: WindowSize = new WindowSize(225, 450);
		const mModalOptions: ModalOptions = new ModalOptions(this._AccountSvc.logInModalId, 'Logon', LoginComponent, mWindowSize);
    this._ModalSvc.close(this._AccountSvc.forgotPasswordModalId);
		this._ModalSvc.open(mModalOptions);
  }

  onSubmit(form: FormGroup): void {
    if(form.valid) {
      // console.log('AccountProfile', this._AccountProfile);
      this._AccountSvc.forgotPassword(this.controls['account'].getRawValue()).then((response) => {
        console.log('ForgotPasswordComponent.onSubmit.response: ', response);
        this._LoggingSvc.toast(response, 'Forgot Password', LogLevel.Success);
        this._ModalSvc.close(this._AccountSvc.forgotPasswordModalId);
      }).catch(() => {
        this._LoggingSvc.toast('Error sending request', 'Forgot Password', LogLevel.Error);
      });
    }
  }

  createForm(): void {
    this.frmForgotPassword = this._FormBuilder.group({
      account: ['', [Validators.required]],
    });
  }
}
