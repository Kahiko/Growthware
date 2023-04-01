import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler } from '@angular/common/http';
import { HttpInterceptor } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
// Library
import { LoaderService } from '@Growthware/Lib/src/lib/features/loader';

/**
 * This class is for intercepting http requests. When a request starts, we set the loadingSub property
 * in the LoadingService to true. Once the request completes and we have a response, set the loadingSub
 * property to false. If an error occurs while servicing the request, set the loadingSub property to false.
 * @class {LoaderInterceptor}
 */
@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
    private _TotalRequests: number = 0;

    constructor(
        private _LoadingSvc: LoaderService
    ) { }

    intercept(request: HttpRequest<any>, next: HttpHandler) {
        this._TotalRequests++;
        this._LoadingSvc.setLoading(true);

        return next.handle(request).pipe(
            finalize(() => {
                this._TotalRequests--;
                if (this._TotalRequests === 0) {
                    this._LoadingSvc.setLoading(false);
                }
            })
        );
    }
}