export interface IAppSettings {
  environment?: string;
  chunkSize?: number;
  logPriority?: string;
  name?: string;
  securityEntityTranslation?: string;
  version?: string;
}

export class AppSettings implements IAppSettings {
	public environment?: string;
	public chunkSize?: number;
	public logPriority?: string;
	public name?: string;
	public securityEntityTranslation?: string;
	public version?: string;
	constructor() {}
}
