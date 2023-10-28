import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
// Library
import { AccountService } from '@Growthware/features/account';
import { GWCommon } from '@Growthware/common-code';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private _BaseUrl: string = '';

  /**
   *
   */
  constructor(private _AccountSvc: AccountService, private _GWCommon: GWCommon) { 
    this._BaseUrl = this._GWCommon.baseURL;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const mAuthenticationResponse = this._AccountSvc.authenticationResponse;
    const mIsLoggedIn = mAuthenticationResponse && mAuthenticationResponse.account != this._AccountSvc.defaultAccount;
    // This will need to match any Api's you have in proxy.conf.js
    const mApiUrls = [
      this._BaseUrl + "GrowthwareAccount",
      this._BaseUrl + "GrowthwareAPI",
      this._BaseUrl + "GrowthwareFile",
      this._BaseUrl + "GrowthwareFunction",
      this._BaseUrl + "GrowthwareGroup",
      this._BaseUrl + "GrowthwareMessage",
      this._BaseUrl + "GrowthwareNameValuePair",
      this._BaseUrl + "GrowthwareRole",
      this._BaseUrl + "GrowthwareSecurityEntity",
      this._BaseUrl + "GrowthwareState",
      this._BaseUrl + "swagger"
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
