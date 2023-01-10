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

    if (token && !this._JwtHelper.isTokenExpired(token)) {
      // console.log(this._JwtHelper.decodeToken(token))
      switch (this._AccountSvc.authenticationResponse.status) {
        case 1:
          this._Router.navigate(['/home']);
          break;
        case 4:
          this._Router.navigate(['/change-password'], {
            queryParams: {
              return: state.url
            }
          });
          return false;
          break;
        case 5:
          this._Router.navigate(['/edit-my-account']);
          break;
        default:
          break;
      }
      return true;
    }

    this._AccountSvc.logout();
    return false;
  }
}
