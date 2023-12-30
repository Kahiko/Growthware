import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, map, Observable, of } from "rxjs";
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
// Feature
import { IAccountInformation, AccountInformation } from './account-information.model';
import { IAccountProfile } from './account-profile.model';
import { IAuthenticationResponse, AuthenticationResponse } from './authentication-response.model';
import { IClientChoices, ClientChoices } from './client-choices.model';
import { ISelectedableAction } from './selectedable-action.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private _AccountInformationSubject: BehaviorSubject<IAccountInformation> = new BehaviorSubject<IAccountInformation>(new AccountInformation());
  private _ApiName: string = 'GrowthwareAccount/';
  private _Api_GetAccountForEdit: string = '';
  private _Api_Authenticate: string = '';
  private _Api_ChangePassword = '';
  private _Api_ClientChoices: string = '';
  private _Api_Logoff: string = '';
  private _Api_RefreshToken: string = '';
  private _Api_SaveClientChoices: string = '';
  private _Api_SelectableActions: string = '';
  private _AuthenticationResponse = new AuthenticationResponse;
  private _BaseURL: string = '';
  private _ClientChoices: IClientChoices = new ClientChoices();
  private _TimerId!: any;
  private _TriggerMenuUpdateSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);

  readonly accountInformation = new AccountInformation();
  readonly accountInformationChanged$ = this._AccountInformationSubject.asObservable();
  readonly addEditModalId: string = 'addEditAccountModal';
  get authenticationResponse() { return this._AuthenticationResponse;}
  readonly anonymous: string = 'Anonymous';
  get clientChoices() { return this._ClientChoices; }
  readonly logInModalId = 'logInModal';
  public modalReason: string = '';
  public selectedRow: any;
  readonly triggerMenuUpdate$ = this._TriggerMenuUpdateSubject.asObservable();

  constructor(
    private _GWCommon: GWCommon, 
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private _Router: Router,
  ) {
    this._BaseURL = this._GWCommon.baseURL;
    this._Api_GetAccountForEdit = this._BaseURL + this._ApiName + 'EditAccount';
    this._Api_Authenticate = this._BaseURL + this._ApiName + 'Authenticate';
    this._Api_ChangePassword = this._BaseURL + this._ApiName + 'ChangePassword';
    this._Api_ClientChoices = this._BaseURL + this._ApiName + 'GetPreferences';
    this._Api_Logoff = this._BaseURL + this._ApiName + 'Logoff';
    this._Api_RefreshToken = this._BaseURL + this._ApiName + 'RefreshToken';
    this._Api_SaveClientChoices = this._BaseURL + this._ApiName + 'SaveClientChoices';
    this._Api_SelectableActions = this._BaseURL + this._ApiName + 'GetSelectableActions';
  }

  /**
   * Authenticates the client with the provided account and password.
   *
   * @param {string} account - The client's account.
   * @param {string} password - The client's password.
   * @param {boolean} silent - (Optional) Indicates whether to log toast messages or not.
   * @return {Promise<boolean|string>} A promise that resolves to true if the authentication is successful, or a string with an error message if the authentication fails.
   */
  private async authenticate(account: string, password: string, silent: boolean = false): Promise<IAuthenticationResponse> {
    return new Promise<IAuthenticationResponse>((resolve, reject) => {
      if(this._GWCommon.isNullOrEmpty(account)) {
        throw new Error("account can not be blank!");
      }
      if(this._GWCommon.isNullOrEmpty(password)) {
        throw new Error("password can not be blank!");
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
      this._HttpClient.post<IAuthenticationResponse>(this._Api_Authenticate, null, mHttpOptions).subscribe({
        next: (authenticationResponse: IAuthenticationResponse) => {
          resolve(authenticationResponse);
        },
        error: (error: any) => {
          if(error.status && error.status === 403) {
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
    if(this._GWCommon.isNullOrEmpty(newPassword)) {
      throw new Error("newPassword can not be blank!");
    }
    if(this._GWCommon.isNullOrEmpty(oldPassword)) {
      throw new Error("oldPassword can not be blank!");
    }
    // console.log(this._Api_ChangePassword);
    return new Promise<boolean>((resolve, reject) => {
      const mQueryParameter: HttpParams = new HttpParams()
        .set('oldPassword', oldPassword)
        .set('newPassword', newPassword);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'text/plain',
        }),
        responseType: "text" as "json",
        params: mQueryParameter,
      };
      this._HttpClient.post<any>(this._Api_ChangePassword, null, mHttpOptions).subscribe({
        next: (response: string) => {
          if(response.startsWith('Your password has been changed')) {
            this._LoggingSvc.toast(response, 'Change password', LogLevel.Success);
            this.authenticate(this.authenticationResponse.account, newPassword, true);
            resolve(true);
          } else {
            this._LoggingSvc.toast(response, 'Change password', LogLevel.Error);
            reject(false);
          }
        },
        error: (error: any) => {
          if(error.status && error.status === 403) {
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
   * Retrieves the client choices asynchronously.
   *
   * @return {Promise<IClientChoices>} A Promise that resolves to an IClientChoices object.
   */
  private async getClientChoices(): Promise<IClientChoices> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return new Promise<IClientChoices>((resolve, reject) => {
      this._HttpClient.get<IClientChoices>(this._Api_ClientChoices, mHttpOptions).subscribe({
        next: (response: IClientChoices) => {
          resolve(response);
        },
        error: (error: any) => {
          reject(false);
          this._LoggingSvc.errorHandler(error, 'AccountService', 'getClientChoices');
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
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'AccountService', 'getSelectableActions');
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
    if(this._GWCommon.isNullOrEmpty(mAccount)) {
      throw new Error("account can not be blank!");
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
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'AccountService', 'getAccount');
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
     *  1.) Get the client choices
     *  2.) Notify subscribers that the AuthenticationResponse and ClientChoices have changed
     *  3.) Start the refresh token timer
     *  4.) Navigate to the appropriate page
     * on Failure:
     *  1.) Navigate to the appropriate page
     */
    return new Promise<boolean>((resolve, reject) => {
      // TODO: Consider haveing authenticae return both the authentication response and the client choices for
      // improved performance.
      this.authenticate(account, password, silent).then((authenticationResponse: IAuthenticationResponse) => {
        this.getClientChoices().then((clientChoices: IClientChoices) => {
          let mNavigationUrl: string = clientChoices.action;
          if(authenticationResponse.status == 4) {
            mNavigationUrl = '/account/change-password';
          }
          sessionStorage.setItem("jwt", authenticationResponse.jwtToken);
          const mAccountInformation: IAccountInformation = { authenticationResponse: authenticationResponse, clientChoices: clientChoices };
          this._AuthenticationResponse = authenticationResponse;
          this._ClientChoices = clientChoices;
          this._AccountInformationSubject.next(mAccountInformation);
          this.triggerMenuUpdate();
          this.startRefreshTokenTimer();
          this._Router.navigate([mNavigationUrl.toLocaleLowerCase()]);
          if(!silent) {
            this._LoggingSvc.toast('You are now logged in', 'Login Successful', LogLevel.Info);
          }
          resolve(true);
        })        
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
  public logout(slient: boolean = false): void {
    sessionStorage.removeItem("jwt")
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    this._HttpClient.get<IAuthenticationResponse>(this._Api_Logoff, mHttpOptions).subscribe({
      next: (authenticationResponse: any) => {
        // console.log('logout.authenticationResponse', authenticationResponse);
        sessionStorage.setItem("jwt", authenticationResponse.jwtToken);
        this.getClientChoices().then((clientChoices: IClientChoices) => {
          const mAccountInformation: IAccountInformation = { authenticationResponse: authenticationResponse, clientChoices: clientChoices };
          this._AuthenticationResponse = authenticationResponse;
          this._ClientChoices = clientChoices;
          this._AccountInformationSubject.next(mAccountInformation);
          this.triggerMenuUpdate();
          if(!slient) {
            this._LoggingSvc.toast('Logout successful', 'Logout', LogLevel.Success);
          }
          this._Router.navigate(['generic_home']);
          this.stopRefreshTokenTimer();
          });
      },
      error: (error: any) => {
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
  public refreshFromToken(): Observable<IAuthenticationResponse> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true,
    };
    // console.log('refreshFromToken', new Date());
    return this._HttpClient.post<IAuthenticationResponse>(this._Api_RefreshToken, null, mHttpOptions)
    .pipe(
      map((authenticationResponse) => {
        this.getClientChoices().then((clientChoices: IClientChoices) => {
          // console.log('AccountService.refreshFromToken.authenticationResponse', authenticationResponse);
          this.startRefreshTokenTimer();
          if(this.authenticationResponse.account.toLocaleLowerCase() != authenticationResponse.account.toLocaleLowerCase()) {
            const mAccountInformation: IAccountInformation = { authenticationResponse: authenticationResponse, clientChoices: clientChoices };
            this._AuthenticationResponse = authenticationResponse;
            this._ClientChoices = clientChoices;
            this._AccountInformationSubject.next(mAccountInformation);
            this.triggerMenuUpdate();
          }
        });
        return authenticationResponse;
      }),
      catchError((err) => {
        // console.log(err);
        this.logout(true);
        // return nothing
        return of();
      })
    );
  }

  /**
   * Starts the refresh token timer.
   */
  public startRefreshTokenTimer(): void {
    // console.log('AccountService.startRefreshTokenTimer', 'Called');
    // parse json object from base64 encoded jwt token
    const mJwtBase64 = this.authenticationResponse.jwtToken!.split('.')[1];
    if(mJwtBase64) {
      // parse json object from base64 encoded jwt token
      const mJwtToken = JSON.parse(window.atob(mJwtBase64));
      // set a timeout to refresh the token a minute before it expires
      const mExpires = new Date(mJwtToken.exp * 1000);
      const mTimeout = mExpires.getTime() - Date.now() - (60 * 1000);
      // console.log('mExpires', mExpires);
      // console.log('mTimeoutDate',  new Date(Date.now() + mTimeout));
      this._TimerId = setTimeout(() => this.refreshFromToken().subscribe(), mTimeout);
    }
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
          this._ClientChoices = response;
          this._AccountInformationSubject.next(mAccountInformation);
          this.triggerMenuUpdate();
          resolve(true);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'AccountService', 'saveClientChoices');
          reject(error);
        }
        //, complete: () => {}
      });
    });
  }

  /**
   * Stops the refresh token timer.
   *
   * @return {void} - This function does not return anything.
   */
  public stopRefreshTokenTimer(): void {
    // console.log('AccountService.stopRefreshTokenTimer', 'Called');
    // release our intervalID from the variable
    clearInterval(this._TimerId);
    // this._TimerId = null;
  }

  /**
   * Notifies account information subscribers that the account information has changed, triggering
   * menu components to re-render.
   *
   * @return {void} 
   */
  public triggerMenuUpdate(): void {
    this._TriggerMenuUpdateSubject.next(true);
  }
}