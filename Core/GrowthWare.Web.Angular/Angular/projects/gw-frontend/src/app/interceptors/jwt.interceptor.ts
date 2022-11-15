import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
// Library
import { AccountService } from '@Growthware/Lib/src/lib/features/account';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  /**
   *
   */
  constructor(private _AccountSvc: AccountService, private _GWCommon: GWCommon) {


  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const mAuthenticationResponse = this._AccountSvc.authenticationResponse;
    const mIsLoggedIn = mAuthenticationResponse && mAuthenticationResponse.account != this._AccountSvc.defaultAccount;
    // Here is where you would alter the code if you have more than on API
    const mRequestURL = this._GWCommon.baseURL + 'GrowthwareAPI';
    const mIsApiUrl = request.url.startsWith(mRequestURL);
    console.log(mRequestURL);
    if (mIsLoggedIn && mIsApiUrl) {
      request = request.clone({
          setHeaders: { Authorization: `Bearer ${mAuthenticationResponse.jwtToken}` }
      });
  }
    return next.handle(request);
  }
}
