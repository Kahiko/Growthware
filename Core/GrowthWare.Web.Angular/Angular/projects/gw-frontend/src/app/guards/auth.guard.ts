import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
// Library
import { AccountService } from '@Growthware/Lib/src/lib/features/account';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate  {

  constructor(
    private _AccountSvc: AccountService,
    private _Router: Router,
    private _JwtHelper: JwtHelperService
  ){}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = localStorage.getItem("jwt");
    let mRedirectUrl: string = '';
    if (token && !this._JwtHelper.isTokenExpired(token)) {
      // console.log(this._JwtHelper.decodeToken(token))
      switch (this._AccountSvc.authenticationResponse.status) {
        case 1:
          mRedirectUrl = '/home';
          break;
        case 4:
          console.log(state);
          mRedirectUrl = '/accounts/change-password';
          break;
        case 5:
          mRedirectUrl = '/accounts/edit-my-account';
          break;
        default:
          break;
      }
      if(mRedirectUrl !== '') {
        if(!state.url.startsWith(mRedirectUrl)) {
          this._Router.navigate([mRedirectUrl], {
            queryParams: {
              return: state.url
            }
          });
          return false;
        }
      }
      return true;
    }

    this._AccountSvc.logout();
    return false;
  }
}
