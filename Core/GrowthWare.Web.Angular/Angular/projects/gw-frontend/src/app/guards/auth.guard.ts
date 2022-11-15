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
    private _JwtHelper: JwtHelperService
  ){}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = localStorage.getItem("jwt");

    if (token && !this._JwtHelper.isTokenExpired(token)){
      // console.log(this._JwtHelper.decodeToken(token))
      return true;
    }

    this._AccountSvc.logout();
    return false;
  }
}
