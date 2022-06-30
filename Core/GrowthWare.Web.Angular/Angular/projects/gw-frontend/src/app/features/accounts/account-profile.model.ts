export interface IAccountProfile {
  addedBy: number;
  addedDate: Date;
  id: number;
  idColumnName: string;
  name: string;
  nameColumnName: string;
  updatedBy: number;
  updatedDate: Date;
  assignedRoles: string[];
  groups: string[];
  derivedRoles: string[];
  account: string;
  email: string;
  enableNotifications: boolean;
  status: number;
  passwordLastSet: Date;
  password: string;
  failedAttempts: number;
  firstName: string;
  isSystemAdmin: boolean;
  lastName: string;
  middleName: string;
  preferredName: string;
  timeZone: number;
  location: string;
  lastLogOn: Date;
}

export class AccountProfile implements IAccountProfile {
  public addedBy: number;
  public addedDate: Date;
  public id: number;
  public idColumnName: string;
  public name: string;
  public nameColumnName: string;
  public updatedBy: number;
  public updatedDate: Date;
  public assignedRoles: string[];
  public groups: string[];
  public derivedRoles: string[];
  public account: string;
  public email: string;
  public enableNotifications: boolean;
  public status: number;
  public passwordLastSet: Date;
  public password: string;
  public failedAttempts: number;
  public firstName: string;
  public isSystemAdmin: boolean;
  public lastName: string;
  public middleName: string;
  public preferredName: string;
  public timeZone: number;
  public location: string;
  public lastLogOn: Date;

  constructor() {}
}
