import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable, Subject } from 'rxjs';
import { catchError, of } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { SearchService } from '@Growthware/Lib/src/lib/features/search';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { INavLink } from '@Growthware/Lib/src/lib/features/navigation';
import { MenuType } from '@Growthware/Lib/src/lib/models';
// Feature
import { IAccountProfile } from './account-profile.model';
import { IClientChoices } from './client-choices.model';
import { AuthenticationResponse, IAuthenticationResponse } from './authentication-response.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private _ApiName: string = 'GrowthwareAccount/';
  private _Api_Authenticate = '';
  private _Api_ChangePassword = '';
  private _Api_GetLinks: string = '';
  private _Api_GetMenuItems: string = '';
  private _Api_Logoff: string = '';
  private _Api_RefreshToken: string = '';
  private _Api_SaveAccount: string = '';
  private _AuthenticationResponse = new AuthenticationResponse();
  private _AuthenticationResponseSubject: BehaviorSubject<IAuthenticationResponse> = new BehaviorSubject<IAuthenticationResponse>(this._AuthenticationResponse);
  private _BaseURL: string = '';
  private _ClientChoices: Subject<IClientChoices> = new Subject<IClientChoices>();
  private _DefaultAccount: string = 'Anonymous';
  private _RefreshTokenTimeout?: NodeJS.Timeout;
  private _SideNavSubject = new Subject<INavLink[]>();

  public get addModalId(): string {
    return 'addAccount'
  }

  public get editModalId(): string {
    return 'editAccount'
  }

  public get loginModalId(): string {
    return 'login';
  }

  public get authenticationResponse(): IAuthenticationResponse {
    return this._AuthenticationResponseSubject.getValue();
  }

  editAccount: string = '';
  editReason: string = '';

  readonly authenticationResponse$ = this._AuthenticationResponseSubject.asObservable();
  readonly clientChoices$ = this._ClientChoices.asObservable();
  
  public get defaultAccount(): string {
    return this._DefaultAccount;
  }

  readonly sideNav$ = this._SideNavSubject.asObservable();

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private _Router: Router,
    private _SearchSvc: SearchService,
  ) {
    this._BaseURL = this._GWCommon.baseURL;
    this._Api_Authenticate = this._BaseURL + this._ApiName + 'Authenticate';
    this._Api_ChangePassword = this._BaseURL + this._ApiName + 'ChangePassword';
    this._Api_GetLinks = this._BaseURL + this._ApiName + 'GetLinks';
    this._Api_GetMenuItems = this._BaseURL + this._ApiName + 'GetMenuItems';
    this._Api_Logoff = this._BaseURL + this._ApiName + 'Logoff';
    this._Api_RefreshToken = this._BaseURL + this._ApiName + 'RefreshToken';
    this._Api_SaveAccount = this._BaseURL + this._ApiName + 'SaveAccount';
  }

  public async authenticate(account: string, password: string, silent: boolean = false): Promise<boolean | string> {
    return new Promise<boolean>((resolve, reject) => {
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
        next: (response: IAuthenticationResponse) => {
          localStorage.setItem("jwt", response.jwtToken);
          this._AuthenticationResponseSubject.next(response);
          if(!silent && account.toLowerCase() === response.account.toLowerCase()) {
            this._LoggingSvc.toast('Successfully logged in', 'Login Success', LogLevel.Success);
          }
          if(account.toLowerCase() !== response.account.toLowerCase()) {
            this._LoggingSvc.toast('The Account or Password is incorrect', 'Login Error', LogLevel.Error);
            resolve(false);
          } else {
            if(response.status == 4) {
              this._Router.navigate(['/accounts/change-password']);
            }
            resolve(true);
          }
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
            this.authenticate(this._AuthenticationResponseSubject.getValue().account, newPassword, true);
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

  public logout(): void {
    localStorage.removeItem("jwt")
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    this._HttpClient.get<IAuthenticationResponse>(this._Api_Logoff, mHttpOptions).subscribe({
      next: (response: any) => {
        localStorage.setItem("jwt", response.jwtToken);
        this._AuthenticationResponseSubject.next(response);
        this._LoggingSvc.toast('Logout successful', 'Logout', LogLevel.Success);
        this._Router.navigate(['generic_home']);
        this.stopRefreshTokenTimer();
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'AccountService', 'logout');
      },
      // complete: () => {}
    });
  }

  async saveAccount(accountProfile: IAccountProfile): Promise<boolean> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    return new Promise<boolean>((resolve, reject) => {
      this._HttpClient.post<string>(this._Api_SaveAccount, accountProfile, mHttpOptions).subscribe({
        next: (response: any) => {
          var mSearchCriteria = this._SearchSvc.getSearchCriteria("Accounts"); // from SearchAccountsComponent line 25
          if(mSearchCriteria != null) {
            this._SearchSvc.setSearchCriteria("Accounts", mSearchCriteria);
          }
          resolve(response);
        }
        , error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'AccountService', 'saveAccount');
          reject(error);
        }
        //, complete: () => {}
      });
    });
  }

  private startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    const jwtBase64 = this._AuthenticationResponseSubject.getValue().jwtToken!.split('.')[1];
    if(jwtBase64) {
      const jwtToken = JSON.parse(window.atob(jwtBase64));
      // set a timeout to refresh the token a minute before it expires
      const expires = new Date(jwtToken.exp * 1000);
      const timeout = expires.getTime() - Date.now() - (60 * 1000);
      this._RefreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
    }
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this._RefreshTokenTimeout);
}

  public refreshToken(): Observable<IAuthenticationResponse> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true,
    };
    return this._HttpClient.post<IAuthenticationResponse>(this._Api_RefreshToken, null, mHttpOptions)
    .pipe(
      map((response) => {
        this._AuthenticationResponseSubject.next(response);
        this.startRefreshTokenTimer();
        return this._AuthenticationResponseSubject.getValue();
      }),
      catchError((err) => {
        // console.log(err);
        this.logout();
        // return nothing
        return of();
      })
    );
  }

  public async getAccount(account: string): Promise<IAccountProfile> {
    let mAccount: string = account;
    if(this._GWCommon.isNullOrEmpty(mAccount)) {
      mAccount = this._DefaultAccount;
    }
    const mQueryParameter: HttpParams = new HttpParams().append('account', mAccount);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    const mUrl = this._BaseURL + this._ApiName + this.editReason;
    return new Promise<IAccountProfile>((resolve, reject) => {
      this._HttpClient.get<IAccountProfile>(mUrl, mHttpOptions).subscribe({
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
   * Gets an array if NavLinks from the API
   *
   * @return {*}  {Promise<INavLink[]>}
   * @memberof AccountService
   */
  public getNavLinks(menuType: MenuType): void {
    const mQueryParameter: HttpParams = new HttpParams()
      .set('menuType', menuType);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter
    };
    this._HttpClient.get<INavLink[]>(this._Api_GetMenuItems, mHttpOptions).subscribe({
      next: (response) => {
        this._SideNavSubject.next(response);
      },
      error: (error) => {
        this._LoggingSvc.errorHandler(error, 'AccountService', 'getNavLinks');
      },
      complete: () => {
        // here as example
      }
    })
  }
}
