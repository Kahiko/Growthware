import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
// Library
import { AccountService } from '@Growthware/Lib/src/lib/features/account';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  /**
   *
   */
  constructor(private _AccountSvc: AccountService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(err => {
      if (
        [401, 403].includes(err.status)
        && this._AccountSvc.account
        && this._AccountSvc.account !== this._AccountSvc.defaultAccount
      ) {
        // auto logout if 401 or 403 response returned from api
        this._AccountSvc.logout();
      }

      const error = (err && err.error && err.error.message) || err.statusText;
      console.error(err);
      // return throwError(error);
      return throwError(() => error);
    }));
  }
}
