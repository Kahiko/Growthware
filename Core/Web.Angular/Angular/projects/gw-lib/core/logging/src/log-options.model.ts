import { LogDestination } from './log-destination.enum';
import { LogLevel } from './log-level.enum';

export interface ILogOptions {
  'account'       : string;
  'componentName' : string;
  'className'     : string;
  'destination'   : LogDestination[];
  'level'         : LogLevel;
  'methodName'    : string;
  'msg'           : string;
  'title'         : string;
}

export class LogOptions implements ILogOptions {

	constructor(
    public msg: string,
    public level: LogLevel = LogLevel.Debug,
    public destination: LogDestination[] = [LogDestination.Console],
    public componentName: string = '',
    public className: string = '',
    public methodName: string = '',
    public account: string = 'System',
    public title: string = ''
	) {}
}
