import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

interface IToken {
  account: string;
  exp: number;
  iat: number;
  nbf: number;
  status: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate  {

  constructor(
    private _Router: Router,
    private _JwtHelper: JwtHelperService
  ){}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const mTokenStr = localStorage.getItem("jwt");
    let mRedirectUrl: string = '';
    let mReturn: boolean = false;
    if (mTokenStr && !this._JwtHelper.isTokenExpired(mTokenStr)) {
      // console.log(this._JwtHelper.decodeToken(token))
      const mToken: IToken = this._JwtHelper.decodeToken(mTokenStr)
      switch (parseInt(mToken.status)) {
        case 1:
          mRedirectUrl = '/home';
          mReturn = false;
          break;
        case 4:
          // console.log(state);
          mRedirectUrl = '/accounts/change-password';
          mReturn = true;
          break;
        case 5:
          mRedirectUrl = '/accounts/edit-my-account';
          mReturn = false;
          break;
        default:
          break;
      }
      this.handleRedirect(mRedirectUrl, state, mReturn);
      return true;
    }

    this.handleRedirect('/accounts/logout', state, false);
    return false;
  }

  private handleRedirect(redirectUrl: string, state: RouterStateSnapshot, returnParameter: boolean): void {
    if(redirectUrl !== '') {
      if(!state.url.startsWith(redirectUrl)) {
        if(returnParameter) {
          this._Router.navigate([redirectUrl], {
            queryParams: {
              return: state.url
            }
          });
        } else {
          this._Router.navigate([redirectUrl]);
        }
      }
    }
  }
}
