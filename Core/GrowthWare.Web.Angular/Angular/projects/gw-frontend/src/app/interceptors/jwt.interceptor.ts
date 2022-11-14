import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
// Library
import { AccountService } from '@Growthware/Lib/src/lib/features/account';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  /**
   *
   */
  constructor(private _AccountSvc: AccountService) {


  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const mAccount = this._AccountSvc.authenticationResponse;
    const isLoggedIn = mAccount && mAccount.account != this._AccountSvc.defaultAccount;
    const isApiUrl = true;
    if (isLoggedIn && isApiUrl) {
      request = request.clone({
          setHeaders: { Authorization: `Bearer ${mAccount.jwtToken}` }
      });
  }
    return next.handle(request);
  }
}
