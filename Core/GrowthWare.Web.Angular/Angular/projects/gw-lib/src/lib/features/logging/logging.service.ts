import { Injectable } from '@angular/core';
import { ILogOptions } from './log-options.model';

@Injectable({
  providedIn: 'root'
})
export class LoggingService {

  constructor() { }

  public log(options: ILogOptions): void {

    if(options.logConsole) {
      this.logConsole();
    }

    if(options.logDB) {
      this.logDB();
    }

    if(options.logToast) {
      this.logToast();
    }

    if(options.logUI) {
      this.logUI();
    }
  }

  private logConsole(): void {

  }

  private logDB(): void {

  }

  private logToast(): void {

  }

  private logUI(): void {

  }
}
