import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
// Library
// import { LoggingService } from '@growthware/core/logging';
import { ConfigurationService } from '@growthware/core/configuration';
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { AccountService } from '../../account.service';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';

@Component({
	selector: 'gw-core-login',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatFormFieldModule,
		MatInputModule
	],
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit, OnDestroy, OnInit {
	private _Subscription: Subscription = new Subscription();

	@ViewChild('account') account!: ElementRef;

	loginForm!: FormGroup;

	get getControls() {
		return this.loginForm.controls;
	}

	submitted: boolean = false;

	constructor(
		private _AccountSvc: AccountService,
		private _ActivatedRoute: ActivatedRoute,
		private _ConfigurationSvc: ConfigurationService,
		private _FormBuilder: FormBuilder,
		// private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _Router: Router,
	) { }

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngAfterViewInit(): void {
		setTimeout(() => {
			this.account.nativeElement.focus();
		}, 100);
	}

	ngOnInit(): void {
		this._Subscription.add(this._ActivatedRoute.queryParams.subscribe((params) => {
			if (params['resetToken']) {
				console.log('LoginComponent.ngOnInit.params.resetToken', params['resetToken']);
			}
		}));
		this._Subscription.add(this._ConfigurationSvc.environment$.subscribe((environment) => {
			if (environment.toLocaleLowerCase() === 'development') {
				this.loginForm = this._FormBuilder.group({
					account: ['Developer', [Validators.required]],
					password: ['none', [Validators.required, Validators.minLength(4)]]
				});
			} else {
				this.loginForm = this._FormBuilder.group({
					account: ['', [Validators.required]],
					password: ['', [Validators.required, Validators.minLength(4)]]
				});
			}
		}));
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

	onForgotPassword(): void {
		this._ModalSvc.close(this._AccountSvc.logInModalId);
		const mWindowSize: WindowSize = new WindowSize(225, 450);
		const mModalOptions: ModalOptions = new ModalOptions(this._AccountSvc.forgotPasswordModalId, 'Forgot Password', ForgotPasswordComponent, mWindowSize);
		mModalOptions.buttons.okButton.callbackMethod = () => {
			// this.onModalOk;
			this._ModalSvc.close(this._AccountSvc.forgotPasswordModalId);
		};
		this._ModalSvc.open(mModalOptions);

	}

	onSubmit() {
		this.submitted = true;

		// stop here if form is invalid
		if (this.loginForm.invalid) {
			return;
		}
		this._AccountSvc.logIn(this.loginForm.value['account'], this.loginForm.value['password']).then(() => {
			this._ModalSvc.close(this._AccountSvc.logInModalId);
			this._Router.navigate(['favorite']);
		});
	}

}
