import { inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
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
class AuthGuardService {

  private _Router = inject(Router);
  private _JwtHelper = inject(JwtHelperService);

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const mTokenStr = localStorage.getItem("jwt");
    let mRedirectUrl: string = '';
    let mReturn: boolean = false;
    if (mTokenStr && !this._JwtHelper.isTokenExpired(mTokenStr)) {
      // console.log(this._JwtHelper.decodeToken(token))
      const mToken: IToken | null = this._JwtHelper.decodeToken(mTokenStr)
      if(mToken) {
        switch (parseInt(mToken.status)) {
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
      }
      this.handleRedirect(mRedirectUrl, state, mReturn);
      return true;
    }
    this.handleRedirect('/accounts/logout', state, false);
    return true; // b/c we are now navigating to logout we can return true
  }

  private handleRedirect(redirectUrl: string, state: RouterStateSnapshot, returnParameter: boolean): void {
    if (redirectUrl !== '') {
      if (!state.url.startsWith(redirectUrl)) {
        if (returnParameter) {
          this._Router.navigate([redirectUrl.toLocaleLowerCase()], {
            queryParams: {
              return: state.url
            }
          });
        } else {
          this._Router.navigate([redirectUrl.toLocaleLowerCase()]);
        }
      }
    }
  }
}

export const AuthGuard: CanActivateFn = (next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean => {
  return inject(AuthGuardService).canActivate(next, state);
}
