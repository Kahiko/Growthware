export interface IAppSettings {
  name: string;
}

export class AppSettings implements IAppSettings {

  constructor(public name: string) {}
}
