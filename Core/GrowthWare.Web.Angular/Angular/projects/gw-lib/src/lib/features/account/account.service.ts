import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';

import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { INavLink } from '@Growthware/Lib/src/lib/features/navigation';

import { IAccountProfile } from './account-profile.model';
import { IAuthenticationResponse } from './authentication-response.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private _Account: string = '';
  private _AuthenticationResponse!: IAuthenticationResponse;
  private _ApiName: string = 'GrowthwareAPI/';
  private _Api_Authenticate = '';
  private _Api_GetAccount: string = '';
  private _Api_GetLinks: string = '';
  private _Api_Logoff: string = '';
  private _Api_RefreshToken: string = '';
  private _CurrentAccount: string = '';
  private _DefaultAccount: string = 'Anonymous'
  private _IsAuthenticated = new Subject<boolean>();
  private _Reason: string = '';
  private _RefreshTokenTimeout: any;
  private _SideNavSubject = new Subject<INavLink[]>();

  public get account(): string {
    return this._Account;
  }
  public set account(value: string) {
    this._Account = value;
  }

  public get authenticationResponse(): IAuthenticationResponse {
    return this._AuthenticationResponse;
  }

  public get addModalId(): string {
    return 'addAccount'
  }

  public get currentAccount(): string {
    return this._CurrentAccount;
  }

  public get defaultAccount(): string {
    return this._DefaultAccount;
  }

  public get editModalId(): string {
    return 'editAccount'
  }

  public get loginModalId(): string {
    return 'login';
  }

  public get reason(): string {
    return this._Reason;
  }
  public set reason(value: string) {
    this._Reason = value;
  }

  readonly isAuthenticated = this._IsAuthenticated.asObservable();
  readonly sideNavSubject = this._SideNavSubject.asObservable();

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private _Router: Router
  ) {
    this._Api_GetAccount = this._GWCommon.baseURL + this._ApiName + 'GetAccount';
    this._Api_GetLinks = this._GWCommon.baseURL + this._ApiName + 'GetLinks';
    this._Api_Authenticate = this._GWCommon.baseURL + this._ApiName + 'Authenticate';
    this._Api_Logoff = this._GWCommon.baseURL + this._ApiName + 'Logoff';
    this._Api_RefreshToken = this._GWCommon.baseURL + this._ApiName + 'RefreshToken';
  }

  public async authenticate(account: string, password: string): Promise<boolean | string> {
    return new Promise<boolean>((resolve, reject) => {
      if(this._GWCommon.isNullOrEmpty(account)) {
        throw new Error("account can not be blank!");
      }
      if(this._GWCommon.isNullOrEmpty(password)) {
        throw new Error("password can not be blank!");
      }
      let mQueryParameter: HttpParams = new HttpParams().append('account', account);
      mQueryParameter=mQueryParameter.append('password', password);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.post<IAuthenticationResponse>(this._Api_Authenticate, null, mHttpOptions).subscribe({
        next: (response: any) => {
          localStorage.setItem("jwt", response.jwtToken);
          this._Account = response.account;
          this._CurrentAccount = this._Account;
          this._AuthenticationResponse = response;
          this._LoggingSvc.toast('Successfully logged in', 'Login Success', LogLevel.Success);
          this.getNavLinks();
          this._Router.navigate(['home']);
          this._IsAuthenticated.next(true);
          resolve(true);
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

  public logout(): void {
    localStorage.removeItem("jwt")
    this.account = this._DefaultAccount;
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    this._HttpClient.get<IAuthenticationResponse>(this._Api_Logoff, mHttpOptions).subscribe({
      next: (response: any) => {
        localStorage.setItem("jwt", response.jwtToken);
        this._Account = response.account;
        this._CurrentAccount = this._Account;
        this._AuthenticationResponse = response;
        this._LoggingSvc.toast('Logout successful', 'Logout', LogLevel.Success);
        this.getNavLinks();
        this._Router.navigate(['generic_home']);
        this._IsAuthenticated.next(false);
        this.stopRefreshTokenTimer();
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'AccountService', 'logout');
      },
      // complete: () => {}
    });
  }

  private startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    const jwtToken = JSON.parse(atob(this._AuthenticationResponse.jwtToken.split('.')[1]));

    // set a timeout to refresh the token a minute before it expires
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    this._RefreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this._RefreshTokenTimeout);
}

  public refreshToken(): Observable<any> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true,
    };
    // return this._HttpClient.post<any>(this._Api_RefreshToken, {}, { withCredentials: true })
    // .pipe(map((account) => {
    //     // this.accountSubject.next(account);
    //     this._AuthenticationResponse = account
    //     this.startRefreshTokenTimer();
    //     return account;
    // }));
    return this._HttpClient.post<any>(this._Api_RefreshToken, null, mHttpOptions)
    .pipe(map((response) => {
        // this.accountSubject.next(account);
        this._AuthenticationResponse = response;
        this.startRefreshTokenTimer();
        this._Account = response.account;
        this._IsAuthenticated.next(true);
        return this._AuthenticationResponse;
    }));
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
    return new Promise<IAccountProfile>((resolve, reject) => {
      this._HttpClient.get<IAccountProfile>(this._Api_GetAccount, mHttpOptions).subscribe({
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
  public getNavLinks(): void {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    this._HttpClient.get<INavLink[]>(this._Api_GetLinks, mHttpOptions).subscribe({
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
