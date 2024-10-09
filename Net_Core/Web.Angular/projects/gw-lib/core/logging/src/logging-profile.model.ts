import { LogDestination } from "./log-destination.enum";

export interface ILoggingProfile {
  account: string;
  className: string;
  component: string;
  destination: LogDestination[];
  level: string;
  logDate: string;
  logSeqId: number;
  methodName: string;
  msg: string;
}

export class LoggingProfile implements ILoggingProfile {
	public logDate: string = '';
	public logSeqId: number = 0;
  public destination: LogDestination[] = [];

	constructor(
    public account: string,
    public className: string,
    public component: string,
    public level: string,
    public methodName: string,
    public msg: string
	){}
}
