import { Injectable, inject } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpEvent, HttpHandlerFn } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
// Library
import { LoaderService } from './loader.service';

/**
 * This class is for intercepting all http requests. When a request starts, we set the loadingSub property
 * in the LoadingService to true. Once the request completes and we have a response, set the loadingSub
 * property to false. If an error occurs while servicing the request, set the loadingSub property to false.
 * @class {LoaderInterceptor}
 */
@Injectable({
	providedIn: 'root'
})
class Loader {

	constructor(
		private _LoadingSvc: LoaderService
	) { }

	intercept(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
		this._LoadingSvc.setLoading(true);
		return next(request).pipe(
			finalize(() => this._LoadingSvc.setLoading(false))
		);
	}
}

export const LoaderInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn,) => {
	return inject(Loader).intercept(req, next);
};