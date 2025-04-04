import { Injectable, inject } from '@angular/core';
import { HttpRequest, HttpEvent, HttpHandlerFn, HttpInterceptorFn } from '@angular/common/http';
import { Observable } from 'rxjs';
// Library
import { AccountService } from '@growthware/core/account';
import { GWCommon } from '@growthware/common/services';

@Injectable({
	providedIn: 'root'
})
class JwtHandler {
	private _BaseUrl: string = '';

	constructor(private _AccountSvc: AccountService, private _GWCommon: GWCommon) {
		this._BaseUrl = this._GWCommon.baseURL;
	}

	intercept(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
		const mAuthenticationResponse = this._AccountSvc.authenticationResponse();
		const mIsLoggedIn = mAuthenticationResponse && mAuthenticationResponse.account != this._AccountSvc.anonymous;
		if (mIsLoggedIn) {
			// This will need to match any Api's you have in proxy.conf.js
			const mApiUrls = [
				this._BaseUrl + 'GrowthwareAccount',
				this._BaseUrl + 'GrowthwareAPI',
				this._BaseUrl + 'GrowthwareCalendar',
				this._BaseUrl + 'GrowthwareFeedback',
				this._BaseUrl + 'GrowthwareFile',
				this._BaseUrl + 'GrowthwareFunction',
				this._BaseUrl + 'GrowthwareGroup',
				this._BaseUrl + 'GrowthwareMessage',
				this._BaseUrl + 'GrowthwareNameValuePair',
				this._BaseUrl + 'GrowthwareRole',
				this._BaseUrl + 'GrowthwareSecurityEntity',
				this._BaseUrl + 'GrowthwareState',
				this._BaseUrl + 'swagger'
			];
			const mUrlIndex = mApiUrls.findIndex(item => request.url.toLowerCase().startsWith(item.toLowerCase()));
			const mIsApiUrl = mUrlIndex > -1;
			// console.log(mIsApiUrl + ' - ' + request.url);
			if (mIsApiUrl) {
				request = request.clone({
					setHeaders: { Authorization: `Bearer ${mAuthenticationResponse.jwtToken}` }
				});
			}
		}
		return next(request);
	}
}

export const JwtInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn,) => {
	return inject(JwtHandler).intercept(req, next);
};