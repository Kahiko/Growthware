import { Injectable, inject } from '@angular/core';
import { HttpRequest, HttpEvent, HttpHandlerFn, HttpInterceptorFn } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
// Library
import { AccountService } from '@growthware/core/account';

@Injectable({
	providedIn: 'root'
})
class HandleError {

	constructor(private _AccountSvc: AccountService) { }

	intercept(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
		return next(request).pipe(catchError(err => {
			let errorMsg = '';
			if (err.error instanceof ErrorEvent) {
				// console.log('this is client side error');
				errorMsg = `Client Error: ${err.error.message}`;
			} else {
				// console.log('this is server side error');
				errorMsg = `Server Error Code: ${err.status}, Message: ${err.message}`;
			}
			console.error('ErrorInterceptor.intercept', errorMsg);
			// auto logout if 401 or 403 response returned from api
			if ([401, 403].includes(err.status) && this._AccountSvc.authenticationResponse.account !== this._AccountSvc.anonymous) {
				this._AccountSvc.logout();
			}
			return throwError(() => errorMsg);
		}));
	}
}

export const ErrorInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn,) => {
	return inject(HandleError).intercept(req, next);
};
