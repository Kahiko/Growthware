import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable } from 'rxjs';
// Library
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { SearchService } from '@growthware/core/search';
// Feature
import { IAccountInformation, AccountInformation } from './account-information.model';
import { IAccountProfile } from './account-profile.model';
import { IAuthenticationResponse, AuthenticationResponse } from './authentication-response.model';
import { IClientChoices, ClientChoices } from './client-choices.model';
import { ISelectedableAction } from './selectedable-action.model';
import { SelectedRow } from './selected-row.model';

@Injectable({
	providedIn: 'root'
})
export class AccountService extends BaseService {
	public clientChoicesSig = signal<IClientChoices>(new ClientChoices());
	override addEditModalId: string = 'addEditAccountModal';
	public authenticationResponse: IAuthenticationResponse = new AuthenticationResponse();
	public authenticationResponseSig = signal<IAuthenticationResponse>(new AuthenticationResponse());
	readonly anonymous = 'anonymous';
	public clientChoices: IClientChoices = new ClientChoices();
	readonly forgotPasswordModalId = 'forgotPasswordModal';
	readonly logInModalId = 'logInModal';
	override modalReason: string = '';
	override selectedRow: SelectedRow = new SelectedRow();
	public updateMenu$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);

	private _AccountInformation = new AccountInformation;
	private _ApiName: string = 'GrowthwareAccount/';
	private _Api_ChangePassword = '';
	private _Api_Authenticate: string = '';
	private _Api_ForgotPassword: string = '';
	private _Api_GetAccountForEdit: string = '';
	private _Api_Logoff: string = '';
	private _Api_RefreshToken: string = '';
	private _Api_RegisterAccount: string = '';
	private _Api_ResetPassword: string = '';
	private _Api_SaveAccount: string = '';
	private _Api_SaveClientChoices: string = '';
	private _Api_SelectableActions = '';
	private _Api_VerifyAccount: string = '';
	private _TimerId!: ReturnType<typeof setTimeout>;

	constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
		private _Router: Router,
		private _SearchSvc: SearchService,
	) {
		super();
		const mBaseUrl = this._GWCommon.baseURL;
		this._Api_Authenticate = mBaseUrl + this._ApiName + 'Authenticate';
		this._Api_ChangePassword = mBaseUrl + this._ApiName + 'ChangePassword';
		this._Api_ForgotPassword = mBaseUrl + this._ApiName + 'ForgotPassword';
		this._Api_GetAccountForEdit = mBaseUrl + this._ApiName + 'EditAccount';
		this._Api_Logoff = mBaseUrl + this._ApiName + 'Logoff';
		this._Api_RefreshToken = mBaseUrl + this._ApiName + 'RefreshToken';
		this._Api_RegisterAccount = mBaseUrl + this._ApiName + 'Register';
		this._Api_ResetPassword = mBaseUrl + this._ApiName + 'ResetPassword';
		this._Api_SaveAccount = mBaseUrl + this._ApiName + 'SaveAccount';
		this._Api_SaveClientChoices = mBaseUrl + this._ApiName + 'SaveClientChoices';
		this._Api_SelectableActions = mBaseUrl + this._ApiName + 'GetSelectableActions';
		this._Api_VerifyAccount = mBaseUrl + this._ApiName + 'VerifyAccount';
	}

	/**
	 * @description Performs any necessary actions once an account has been authenticated
	 * @param authenticationResponse 
	 */
	private afterAuthentication(accountInformation: IAccountInformation, forceMenuUpdate: boolean = false): void {
		// if the account is not anonymous and the account information is not null
		// 	1.) Set _AuthenticationResponse
		// 	2.) Make client choices avalible VIA sessionStorage to avoid injecting the AccountService
		// 	3.) Trigger menu update subject
		// 	4.) Start refresh token timer
		// if account information is not null
		// 	1.) Make the JWT token available VIA sessionStorage to avoid injecting the AccountService
		let mTriggerMenuUpdates = false;
		if (this._AccountInformation.authenticationResponse.account.toLowerCase() !== accountInformation.authenticationResponse.account.toLowerCase()) {
			mTriggerMenuUpdates = true;
		}
		this._AccountInformation = JSON.parse(JSON.stringify(accountInformation));
		this.authenticationResponse = this._AccountInformation.authenticationResponse;
		this.clientChoices = JSON.parse(JSON.stringify(this._AccountInformation.clientChoices));
		const mClientChoicesString: string = JSON.stringify(this._AccountInformation.clientChoices);
		sessionStorage.setItem('clientChoices', mClientChoicesString);
		// this.accountInformationChanged$.next(this._AccountInformation);
		this.authenticationResponseSig.set(JSON.parse(JSON.stringify(accountInformation.authenticationResponse)));
		this.clientChoicesSig.set(JSON.parse(mClientChoicesString));
		if (mTriggerMenuUpdates || forceMenuUpdate) {
			this.triggerMenuUpdates();
		}
		if (this._AccountInformation.authenticationResponse.jwtToken !== null) {
			sessionStorage.setItem('jwt', this._AccountInformation.authenticationResponse.jwtToken);
		}
		if (this._AccountInformation !== null && this._AccountInformation.authenticationResponse.account.toLowerCase() !== this.anonymous) {
			this.setRefreshTokenTimer();
		} else {
			this.stopRefreshTokenTimer();
		}
	};

	/**
	 * @description Authenticates an account VIA the API using the provided account and password
	 * @param {string} account - The client's account.
	 * @param {string} password - The client's password.
	 * @return {Promise<boolean | string>} A promise that resolves to true if the authentication is successful, or a string with an error message if the authentication fails.
	 */
	private authenticate(account: string, password: string): Promise<boolean> {
		return new Promise<boolean>((resolve, reject) => {
			if (this._GWCommon.isNullOrEmpty(account)) {
				throw new Error('account can not be blank!');
			}
			if (this._GWCommon.isNullOrEmpty(password)) {
				throw new Error('password can not be blank!');
			}
			const mQueryParameter: HttpParams = new HttpParams()
				.set('account', account)
				.set('password', password);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.post<{ item1: IAuthenticationResponse, item2: IClientChoices }>(this._Api_Authenticate, null, mHttpOptions).subscribe({
				next: (response) => {
					const mAccountInformation: IAccountInformation = { authenticationResponse: response.item1, clientChoices: response.item2 };
					this.afterAuthentication(mAccountInformation);
					resolve(true);
				},
				error: (error) => {
					if (error.status && error.status === 403) {
						this._LoggingSvc.toast('The Account or Password is incorrect', 'Login Error', LogLevel.Warn);
						reject(error.error);
					} else {
						this._LoggingSvc.errorHandler(error, 'AccountService', 'authenticate');
						reject('Failed to call the API');
					}
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * Change the client's password.
	 *
	 * @param {string} oldPassword - The client's current password.
	 * @param {string} newPassword - The new password to set for the client.
	 * @return {Promise<boolean>} A promise that resolves to true if the password change was successful, or false otherwise.
	 */
	public async changePassword(oldPassword: string, newPassword: string): Promise<boolean> {
		if (this._GWCommon.isNullOrEmpty(newPassword)) {
			throw new Error('newPassword can not be blank!');
		}
		if (this._GWCommon.isNullOrEmpty(oldPassword)) {
			throw new Error('oldPassword can not be blank!');
		}
		return new Promise<boolean>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('oldPassword', oldPassword)
				.set('newPassword', newPassword);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'text/plain',
				}),
				responseType: 'text' as 'json',
				params: mQueryParameter,
			};
			this._HttpClient.post<{ item1: string; item2: IAuthenticationResponse }>(this._Api_ChangePassword, null, mHttpOptions).subscribe({
				next: (response) => {
					// const mAccountInformation: IAccountInformation = { authenticationResponse: response.item1, clientChoices: response.item2 };
					if (response.item1.startsWith('Your password has been changed')) {
						const mAccountInformation = { authenticationResponse: response.item2, clientChoices: this._AccountInformation.clientChoices };
						this.afterAuthentication(mAccountInformation);
						this._LoggingSvc.toast(response.item1, 'Change password', LogLevel.Success);
						resolve(true);
					} else {
						this._LoggingSvc.toast(response.item1, 'Change password', LogLevel.Error);
						reject(false);
					}
				},
				error: (error) => {
					if (error.status && error.status === 403) {
						this._LoggingSvc.toast('Unable to change password', 'Change password', LogLevel.Error);
						reject(error.error);
					} else {
						this._LoggingSvc.errorHandler(error, 'AccountService', 'authenticate');
						reject(false);
					}
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * Sends a password reset request to the server.
	 *
	 * @param {string} account - The account to send the password reset request to.
	 * @return {Promise<string>} A promise that resolves to a string with the response from the server.
	 */
	forgotPassword(account: string): Promise<string> {
		// console.log('AccountService.forgotPassword: ', account);
		return new Promise<string>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('account', account);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
				responseType: 'text' as 'json',
			};
			this._HttpClient.post<string>(this._Api_ForgotPassword, null, mHttpOptions).subscribe({
				next: (response: string) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'AccountService', 'forgotPassword');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * Retrieves the account profile for the given account.
	 *
	 * @param {string} account - The account to retrieve the profile for.
	 * @return {Promise<IAccountProfile>} - A promise that resolves to the account profile.
	 */
	public async getAccountForEdit(account: string): Promise<IAccountProfile> {
		const mAccount: string = account;
		if (this._GWCommon.isNullOrEmpty(mAccount)) {
			throw new Error('account can not be blank!');
		}
		const mQueryParameter: HttpParams = new HttpParams()
			.set('account', mAccount);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<IAccountProfile>((resolve, reject) => {
			this._HttpClient.get<IAccountProfile>(this._Api_GetAccountForEdit, mHttpOptions).subscribe({
				next: (response: IAccountProfile) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'AccountService', 'getAccount');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * Retrieves a list of selectable actions.
	 *
	 * @return {Promise<ISelectedableAction[]>} A promise that resolves to an array of selectable actions.
	 */
	public async getSelectableActions(): Promise<ISelectedableAction[]> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		return new Promise<ISelectedableAction[]>((resolve, reject) => {
			this._HttpClient.get<ISelectedableAction[]>(this._Api_SelectableActions, mHttpOptions).subscribe({
				next: (response: ISelectedableAction[]) => {
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'AccountService', 'getSelectableActions');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * Logs in the client with the provided account and password.
	 *
	 * @param {string} account - The client's account.
	 * @param {string} password - The client's password.
	 * @param {boolean} silent - (Optional) Set to true to suppress any notifications or toasts. Defaults to false.
	 * @returns {Promise<boolean>} A promise that resolves to true if the login is successful, and false otherwise.
	 */
	public async logIn(account: string, password: string, silent: boolean = false): Promise<boolean> {
		/**
		 * 1.) Authenticate the account
		 * on Success:
		 *  1.) Notify subscribers that the AuthenticationResponse and ClientChoices have changed
		 *  2.) Start the refresh token timer
		 *  3.) Navigate to the appropriate page
		 * on Failure:
		 *  1.) Navigate to the appropriate page
		 */
		return new Promise<boolean>((resolve, reject) => {
			this.authenticate(account, password).then((response: boolean) => {
				let mNavigationUrl: string = this._AccountInformation.clientChoices.action;
				if (this._AccountInformation.authenticationResponse.status == 4) {
					mNavigationUrl = '/accounts/change-password';
				}
				if (response === true) {
					this._Router.navigate([mNavigationUrl.toLocaleLowerCase()]);
				}
				if (!silent) {
					this._LoggingSvc.toast('You are now logged in', 'Login Successful', LogLevel.Info);
				}
				resolve(response);
			}).catch((error) => {
				this._LoggingSvc.errorHandler(error, 'AccountService', 'logIn');
				this._Router.navigate(['/accounts/logout']);
				reject(error);
			});
		});
	}

	/**
	 * Logout the client and perform necessary cleanup actions.
	 *
	 * @return {void} This function does not return anything.
	 */
	public logout(slient: boolean = false, navigate: boolean = true): void {
		sessionStorage.removeItem('jwt');
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		this._HttpClient.post<{ item1: IAuthenticationResponse, item2: IClientChoices }>(this._Api_Logoff, mHttpOptions).subscribe({
			next: (response) => {
				// console.log('logout.authenticationResponse', authenticationResponse);
				const mAccountInformation: IAccountInformation = { authenticationResponse: response.item1, clientChoices: response.item2 };
				this.afterAuthentication(mAccountInformation);
				if (!slient) {
					this._LoggingSvc.toast('Logout successful', 'Logout', LogLevel.Success);
				}
				if (navigate) {
					this._Router.navigate(['generic_home']);
				}
				this.stopRefreshTokenTimer();
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'AccountService', 'logout');
			},
			// complete: () => {}
		});
	}

	/**
	 * Refreshes the authentication token.
	 *
	 * @return {Observable<IAuthenticationResponse>} The authentication response
	 */
	refreshToken(): Observable<IAuthenticationResponse> {
		// 1.) get the refresh token response
		return this._HttpClient.post<{ item1: IAuthenticationResponse, item2: IClientChoices }>(this._Api_RefreshToken, {}, { withCredentials: true })
			.pipe(map((response) => {
				// 2.) update information from the response
				const mAccountInformation: IAccountInformation = { authenticationResponse: response.item1, clientChoices: response.item2 };
				this.afterAuthentication(mAccountInformation);
				return mAccountInformation.authenticationResponse;
			}));
	}

	/**
	 * Registers a new account.
	 * @param accountProfile 
	 * @returns Promise<string>
	 */
	async registerAccount(accountProfile: IAccountProfile): Promise<string> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		return new Promise<string>((resolve, reject) => {
			// Format the first name, last name, middle name, and preferred name
			// Create an copy of the accountProfile
			const mAccountToSave = JSON.parse(JSON.stringify(accountProfile));
			mAccountToSave.firstName = this._GWCommon.capitalizeFirstLetter(mAccountToSave.firstName);
			mAccountToSave.lastName = this._GWCommon.capitalizeFirstLetter(mAccountToSave.lastName);
			mAccountToSave.middleName = this._GWCommon.capitalizeFirstLetter(mAccountToSave.middleName);
			mAccountToSave.preferredName = this._GWCommon.capitalizeFirstLetter(mAccountToSave.preferredName);
			this._HttpClient.post<{ message: string }>(this._Api_RegisterAccount, mAccountToSave, mHttpOptions).subscribe({
				next: (mReturnString: { message: string }) => {
					console.log('AccountService.registerAccount', mReturnString);
					if (mReturnString.message.toLowerCase().indexOf('failed') > -1) {
						reject(mReturnString.message);
					}
					resolve(mReturnString.message);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'AccountService', 'registerAccount');
					reject(error);
				}
			});
		});
	}

	/**
	 * Resets the password.
	 * @param resetToken 
	 * @param newPassword 
	 * @returns Promise<boolean>
	 */
	public async resetPassword(resetToken: string, newPassword: string): Promise<boolean> {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('resetToken', resetToken)
			.set('newPassword', newPassword);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.put<{ item1: IAuthenticationResponse, item2: IClientChoices }>(this._Api_ResetPassword, null, mHttpOptions).subscribe({
				next: (response: { item1: IAuthenticationResponse, item2: IClientChoices }) => {
					if (response.item1.account.toLowerCase() !== this.anonymous.toLowerCase()) {
						const mAccountInformation: IAccountInformation = { authenticationResponse: response.item1, clientChoices: response.item2 };
						this.afterAuthentication(mAccountInformation);
						this._Router.navigate(['home']);
					} else {
						resolve(false);
					}
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'AccountService', 'resetPassword');
					reject(false);
				}
			});
		});
	}

	/**
	 * Sends a request to save an account.
	 * 
	 * @param {IAccountProfile} accountProfile the account profile to save
	 * @returns {Promise<boolean>} A promise that resolves to true if the save is successful, and false otherwise.
	 */
	async saveAccount(accountProfile: IAccountProfile): Promise<boolean> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<string>(this._Api_SaveAccount, accountProfile, mHttpOptions).subscribe({
				next: () => {
					const mSearchCriteria = this._SearchSvc.getSearchCriteria('Accounts'); // from SearchAccountsComponent (this.configurationName)
					if (mSearchCriteria != null) {
						this._SearchSvc.setSearchCriteria('Accounts', mSearchCriteria);
					}
					resolve(true);
				}
				, error: (error) => {
					this._LoggingSvc.errorHandler(error, 'AccountService', 'saveAccount');
					reject(error);
				}
				//, complete: () => {}
			});
		});
	}

	/**
	 * Saves the client choices in the database.
	 *
	 * @param {IClientChoices} clientChoices - The client choices to be saved.
	 * @return {Promise<boolean>} A promise that resolves to true if the client choices are saved successfully, otherwise false.
	 */
	public async saveClientChoices(clientChoices: IClientChoices): Promise<boolean> {
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			})
		};
		return new Promise<boolean>((resolve, reject) => {
			this._HttpClient.post<IClientChoices>(this._Api_SaveClientChoices, clientChoices, mHttpOptions).subscribe({
				next: (response: IClientChoices) => {
					const mAccountInformation: IAccountInformation = { authenticationResponse: this.authenticationResponse, clientChoices: response };
					this.afterAuthentication(mAccountInformation, true);
					resolve(true);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'AccountService', 'saveClientChoices');
					reject(error);
				}
				//, complete: () => {}
			});
		});
	}

	/**
	 * Starts the refresh token timer.
	 */
	private setRefreshTokenTimer() {
		// parse json object from base64 encoded jwt token
		const mJasonWebToken = sessionStorage.getItem('jwt');
		let mJwtBase64 = null;
		if (mJasonWebToken != null) {
			mJwtBase64 = mJasonWebToken.split('.')[1];
		}
		if (mJwtBase64) {
			const mJwtToken = JSON.parse(window.atob(mJwtBase64));

			// set a timeout to refresh the token a minute before it expires
			const mExpires = new Date(mJwtToken.exp * 1000);
			const mTimeout = mExpires.getTime() - Date.now() - (60 * 1000);
			// console.log('mExpires', mExpires);
			// console.log('mTimeout',  new Date(Date.now() + mTimeout));
			this._TimerId = setTimeout(() => this.refreshToken().subscribe(), mTimeout);
		}
	}

	/**
	 * Stops the refresh token timer.
	 *
	 * @return {void} - This function does not return anything.
	 */
	private stopRefreshTokenTimer(): void {
		clearTimeout(this._TimerId);
	}

	/**
	 * Notifies account information subscribers that the account information has changed, triggering
	 * menu components to re-render.
	 *
	 * @return {void} 
	 */
	public triggerMenuUpdates(): void {
		this.updateMenu$.next(true);
	}

	/**
	 * Verifies an account using the verification token and email.
	 * @param verificationToken 
	 * @param email 
	 * @returns 
	 */
	public verifyAccount(verificationToken: string, email: string): void {
		if (this._GWCommon.isNullOrEmpty(verificationToken)) {
			this._LoggingSvc.console('verificationToken can not be blank!', LogLevel.Error);
			this._LoggingSvc.toast('Unable to verify account.', 'Verify Account', LogLevel.Error);
			return;
		}
		// console.log('AccountService.verifyAccount verificationToken: ', verificationToken);
		const mQueryParameter: HttpParams = new HttpParams()
			.set('verificationToken', verificationToken)
			.set('email', email);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		this._HttpClient.post<{ item1: IAuthenticationResponse, item2: IClientChoices }>(this._Api_VerifyAccount, null, mHttpOptions).subscribe({
			next: (response) => {
				// console.log('AccountService.verifyAccount response: ', response);
				const mAccountInformation: IAccountInformation = { authenticationResponse: response.item1, clientChoices: response.item2 };
				this.afterAuthentication(mAccountInformation);
				this._Router.navigate(['/accounts/change-password']);
				this._LoggingSvc.toast('Account has been verified, Please change your password', 'Verify Account', LogLevel.Success);
			},
			error: (error) => {
				// console.log('AccountService.verifyAccount.error: ', error);
				this._LoggingSvc.errorHandler(error, 'AccountService', 'verifyAccount');
				this._LoggingSvc.toast('Account failed verification!', 'Verify Account', LogLevel.Fatal);
			},
			// complete: () => {}
		});
	}
}