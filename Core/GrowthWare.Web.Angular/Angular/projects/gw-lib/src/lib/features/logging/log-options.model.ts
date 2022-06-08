import { LogLevel } from "./log-level.enum";

export interface ILogOptions {
  "account"     : string;
  "className"   : string;
  "component"   : string;
  "level"       : LogLevel;
  "logConsole"  : boolean;
  "logDB"       : boolean;
  "logToast"    : boolean;
  "logUI"       : boolean;
  "methodName"  : string;
  "msg"         : string;
}

export class LogOptions implements ILogOptions {

  constructor(
    public className: string,
    public component: string,
    public methodName: string,
    public msg: string,
    public title: string,
    public account: string = 'System',
    public level: LogLevel = LogLevel.Debug,
    public logConsole: boolean = true,
    public logDB: boolean = false,
    public logToast: boolean = false,
    public logUI: boolean = false,
  ) {}
}
