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
    // This will need to match any Api's you have in proxy.conf.js
    const mBaseUrl = this._GWCommon.baseURL;
    const mApiUrls = [
      mBaseUrl + "GrowthwareAccount",
      mBaseUrl + "GrowthwareAPI",
      mBaseUrl + "GrowthwareFile",
      mBaseUrl + "GrowthwareFunction",
      mBaseUrl + "GrowthwareGroup",
      mBaseUrl + "GrowthwareRole",
      mBaseUrl + "swagger"
    ];
    const mUrlIndex = mApiUrls.findIndex(item => request.url.toLowerCase().startsWith(item.toLowerCase()));
    const mIsApiUrl = mUrlIndex > -1;
    // console.log(mIsApiUrl + ' - ' + request.url);
    if (mIsLoggedIn && mIsApiUrl) {
      request = request.clone({
          setHeaders: { Authorization: `Bearer ${mAuthenticationResponse.jwtToken}` }
      });
  }
    return next.handle(request);
  }
}
