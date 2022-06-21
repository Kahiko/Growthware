import { Injectable } from '@angular/core';
import { ILogOptions, LogOptions } from './log-options.model';
import { LogDestinition } from './log-destinition.enum';
import { LogLevel } from './log-level.enum';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  // https://adrianhall.github.io/cloud/2019/06/30/building-an-efficient-logger-in-typescript/
  constructor(private _GWCommon: GWCommon) { }

  private getStackTrace(): string {
    const mStackLines = (new Error("")).stack?.split('\n') ?? [];
    if(this._GWCommon.isNullOrUndefined(mStackLines) || mStackLines.length === 0) {
      return '';
    }
    const mOurCallStack: any[] = [];
    let mRetVal: string = '';
    try {
      for(let x = 0; x<=mStackLines.length; x++) {
        const mLine = mStackLines[x];
        if(mLine != 'Error' && !this._GWCommon.isNullOrUndefined(mLine)) { // Don't need the first line
          const mParts = mLine.split(' ');
          if(mParts.length = 7) {
            const mCaller = mParts[5];
            if(mCaller.indexOf('_next') === -1 && mCaller.indexOf('callH') === -1) { // we can stop b/c we have gotten all of our codes stack
              if(mCaller.indexOf('LoggingService') === -1) { // Don't include this class in the stack
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
      console.error('Error in LoggingService.getStackTrace:\n')
      console.error(error);
    }
    if(!this._GWCommon.isNullOrUndefined(mOurCallStack) && mOurCallStack.length !== 0) {
      mOurCallStack.forEach(element => {
        mRetVal += element.caller + ' => ';
      });
    }
    return mRetVal;
  }

  public log(options: ILogOptions): void {
    options.destination.forEach(element => {
      switch (LogDestinition[element]) {
        case 'Console':
          this.logConsole(options);
          break;
        case 'DB':
          this.logDB(options);
          break;
        case 'Toast':
          this.logToast(options);
          break;
        case 'UI':
          this.logUI(options);
          break;
        default:
          break;
      }

    });
  }

  public console(msg: string, level: LogLevel): void {
    const mLogOptions: LogOptions = new LogOptions(msg, level);
    this.logConsole(mLogOptions)
  }

  private logConsole(options: ILogOptions): void {

    const mMsg = this.getStackTrace().replace(new RegExp(' => ' + '$'), ':') + '\n  ' + options.msg;
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
        break;
      case 'Trace':
        console.trace(mMsg);
        break;
      case 'Success':
      default:
        console.log(mMsg)
        break;
    }

  }

  private logDB(options: ILogOptions): void {

  }

  private logToast(options: ILogOptions): void {

  }

  private logUI(options: ILogOptions): void {

  }
}
