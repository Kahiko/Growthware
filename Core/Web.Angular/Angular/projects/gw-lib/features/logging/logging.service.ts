import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { EventType, ToastService, ToastMessage } from '@Growthware/features/toast';
// Feature
import { ILogOptions, LogOptions } from './log-options.model';
import { LogDestination } from './log-destination.enum';
import { ILoggingProfile, LoggingProfile } from './logging-profile.model';
import { LogLevel } from './log-level.enum';

@Injectable({
  providedIn: 'root',
})
export class LoggingService {
  // https://adrianhall.github.io/cloud/2019/06/30/building-an-efficient-logger-in-typescript/

  private _ApiName: string = 'GrowthwareAPI/';
  private _Api_CurrentLogLevel = 'GetLogLevel';
  private _Api_Log: string = 'log';
  private _Api_SetLogLevel = 'SetLogLevel';

  constructor(
    private _HttpClient: HttpClient,
    private _GWCommon: GWCommon,
    private _ToastSvc: ToastService
  ) {
    this._Api_Log = _GWCommon.baseURL + this._ApiName + 'Log';
    this._Api_SetLogLevel = _GWCommon.baseURL + this._ApiName + 'SetLogLevel';
    this._Api_CurrentLogLevel = _GWCommon.baseURL + this._ApiName  + 'GetLogLevel';
  }

  public async getLogLevel(): Promise<number> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      responseType: "text" as "json",
    };
    return new Promise<number>((resolve, reject) => {
      this._HttpClient.get<any>(this._Api_CurrentLogLevel, mHttpOptions).subscribe({
        next: (response: number) => {
          resolve(response);
        },
        error: (error: any) => {
          if(error.status && error.status === 403) {
            this.toast('Unable to get log level', 'Get Log Level', LogLevel.Error);
            reject(error.error);
          } else {
            this.errorHandler(error, 'LoggingService', 'getLogLevel');
            reject(false);
          }
        },
        // complete: () => {}
      });      
    })
  }

  private getStackTrace(): string {
    const mStackLines = new Error('').stack?.split('\n') ?? [];
    if (
      this._GWCommon.isNullOrUndefined(mStackLines) ||
      mStackLines.length === 0
    ) {
      return '';
    }
    const mOurCallStack: any[] = [];
    let mRetVal: string = '';
    try {
      for (let x = 0; x <= mStackLines.length; x++) {
        const mLine = mStackLines[x];
        if (mLine != 'Error' && !this._GWCommon.isNullOrUndefined(mLine)) {
          // Don't need the first line
          const mParts = mLine.split(' ');
          if ((mParts.length = 7)) {
            const mCaller = mParts[5];
            if (
              mCaller.indexOf('_next') === -1 &&
              mCaller.indexOf('callH') === -1
            ) {
              // we can stop b/c we have gotten all of our codes stack
              if (mCaller.indexOf('LoggingService') === -1) {
                // Don't include this class in the stack
                const mCallStackObj = { caller: mCaller, file: mParts[6] };
                mOurCallStack.push(mCallStackObj);
              }
            } else {
              break;
            }
          }
        }
      }
    } catch (error) {
      console.error('Error in LoggingService.getStackTrace:\n');
      console.error(error);
    }
    if (
      !this._GWCommon.isNullOrUndefined(mOurCallStack) &&
      mOurCallStack.length !== 0
    ) {
      mOurCallStack.forEach((element) => {
        mRetVal += element.caller + ' => ';
      });
    }
    return mRetVal;
  }

  /**
   * Practical when multi-destinations are desired.
   *
   * @param {ILogOptions} options
   * @memberof LoggingService
   */
  public log(options: ILogOptions): void {
    options.destination.forEach((element) => {
      switch (LogDestination[element]) {
        case 'Console':
          this._LogConsole(options);
          break;
        case 'DB':
          this._LogDB(options);
          break;
        case 'Toast':
          this._LogToast(options);
          break;
        default:
          this.toast(`Unsupported LogDestination: "$LogDestination[element]"`, 'Logging Error', LogLevel.Error);
          break;
      }
    });
  }

  public async setLogLevel(logLevel: number): Promise<boolean> {
    const mQueryParameter: HttpParams = new HttpParams()
    .set('logLevel', logLevel);    
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      responseType: "text" as "json",
      params: mQueryParameter,
    };
    return new Promise<boolean>((resolve, reject) => {
      this._HttpClient.post<any>(this._Api_SetLogLevel, mQueryParameter, mHttpOptions).subscribe({
        next: (response: string) => {
          this.toast('Successfully set the log level', 'Set Log Level', LogLevel.Success);
          resolve(true);
        },
        error: (error: any) => {
          if(error.status && error.status === 403) {
            this.toast('Unable to set log level', 'Set Log Level', LogLevel.Error);
            reject(error.error);
          } else {
            this.errorHandler(error, 'LoggingService', 'setLogLevel');
            reject(false);
          }
        },
        // complete: () => {}
      });      
    })
  }

  /**
   * Use to log a message to the console.
   *
   * @param {string} msg
   * @param {LogLevel} level
   * @memberof LoggingService
   */
  public console(msg: string, level: LogLevel): void {
    const mLogOptions: LogOptions = new LogOptions(msg, level);
    this._LogConsole(mLogOptions);
  }

  private _LogConsole(options: ILogOptions): void {
    const mMsg =
      this.getStackTrace().replace(new RegExp(' => ' + '$'), ':') +
      '\n  ' +
      options.msg;
    switch (LogLevel[options.level]) {
      case 'Debug':
        console.debug(mMsg);
        break;
      case 'Error':
      case 'Fatal':
        console.error(mMsg);
        break;
      case 'Info':
        console.info(mMsg);
        break;
      case 'Warn':
        console.warn(mMsg);
        break;
      case 'Trace':
        console.trace(mMsg);
        break;
      case 'Success':
      default:
        console.log(mMsg);
        break;
    }
  }

  /**
   * Use to log a message to the Database.
   *
   * @param {string} msg
   * @param {LogLevel} [level=LogLevel.Debug]
   * @param {string} componentName
   * @param {string} className
   * @param {string} methodName
   * @param {string} [account='System']
   * @memberof LoggingService
   */
  public dataBase(
    msg: string,
    level: LogLevel = LogLevel.Debug,
    componentName: string,
    className: string,
    methodName: string,
    account: string = 'System'
  ): void {
    const mLogOptions: LogOptions = new LogOptions(msg, level);
    mLogOptions.account = account;
    mLogOptions.className = className;
    mLogOptions.componentName = componentName;
    mLogOptions.level = level;
    mLogOptions.methodName = methodName;
    mLogOptions.msg = msg;
    this._LogDB(mLogOptions);
  }

  private _LogDB(options: ILogOptions): void {
    // this.console(this._LoggingURL, LogLevel.Info);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    const mData: ILoggingProfile = new LoggingProfile(
      options.account,
      options.className,
      options.componentName,
      LogLevel[options.level],
      options.methodName,
      options.msg,
    );
    mData.logDate = new Date().toISOString();
    this._HttpClient
      .post<any>(this. _Api_Log, mData, mHttpOptions)
      .subscribe({
        next: (response: any) => {
          this._LogConsole(response);
        },
        error: (errorResponse: any) => {
          this.errorHandler(errorResponse, 'LoggingService', 'logDB');
        },
      });
  }

  /**
   * Use to log a 'toast' message.
   *
   * @param {string} msg
   * @param {string} title
   * @param {LogLevel} level
   * @memberof LoggingService
   */
  public toast(msg: string, title: string, level: LogLevel): void {
    const mLogOptions: LogOptions = new LogOptions(msg, level);
    mLogOptions.title = title;
    this._LogToast(mLogOptions);
  }

  private _LogToast(options: ILogOptions): void {
    const mToastMessage = new ToastMessage(
      options.msg,
      options.title,
      EventType.Info
    );
    switch (LogLevel[options.level]) {
      case 'Error':
      case 'Fatal':
        mToastMessage.eventType = EventType.Error;
        break;
      case 'Debug':
      case 'Info':
      case 'Trace':
        mToastMessage.eventType = EventType.Info;
        break;
      case 'Warn':
        mToastMessage.eventType = EventType.Warning;
        break;
      case 'Success':
        mToastMessage.eventType = EventType.Success;
        break;
      default:
        mToastMessage.eventType = EventType.Info;
        break;
    }
    this._ToastSvc.showToast(mToastMessage);
  }

  /**
   * Handles an HttpClient error
   *
   * @private
   * @param {HttpErrorResponse} errorResponse
   * @param {string} methodName
   * @memberof LoggingService
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
    console.log(`${className}.${methodName}:`);
    console.log('\t' + errorMessage);
  }
}
