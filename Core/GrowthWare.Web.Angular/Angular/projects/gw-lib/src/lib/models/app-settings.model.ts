export interface IAppSettings {
  logPriority?: string;
  name?: string;
  version?: string;
}

export class AppSettings implements IAppSettings {
  public logPriority?: string;
  public name?: string;
  public version?: string;
  constructor() {}
}
