import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
// Library
import { LogLevel, LoggingService } from '@Growthware/Lib/src/lib/features/logging';

@Injectable({
  providedIn: 'root'
})
export class UtilityService {

  constructor(private _LoggingSvc: LoggingService) { }


  /**
   * A common function to handle an HttpClient error
   *
   * @param {HttpErrorResponse} errorResponse
   * @param {string} className
   * @param {string} methodName
   * @memberof GWCommon
   */
   public errorHandler(errorResponse: HttpErrorResponse, className: string, methodName: string) {
    let errorMessage = '';
    if (errorResponse.error instanceof ErrorEvent) {
        // Get client-side error
        errorMessage = errorResponse.error.message;
    } else {
        // Get server-side error
        errorMessage = `Error Code: ${errorResponse.status}\nMessage: ${errorResponse.message}`;
    }
    this._LoggingSvc.console(`${className}.${methodName}:`, LogLevel.Error);
    this._LoggingSvc.console(errorMessage, LogLevel.Error);
  }
}
