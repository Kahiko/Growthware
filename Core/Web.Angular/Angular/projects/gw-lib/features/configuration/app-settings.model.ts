export interface IAppSettings {
  logPriority?: string;
  name?: string;
  securityEntityTranslation?: string;
  version?: string;
}

export class AppSettings implements IAppSettings {
  public logPriority?: string;
  public name?: string;
  public securityEntityTranslation?: string;
  public version?: string;
  constructor() {}
}
