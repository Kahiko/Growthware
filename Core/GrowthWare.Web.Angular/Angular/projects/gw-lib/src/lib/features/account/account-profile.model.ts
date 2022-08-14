export interface IAccountProfile {
  addedBy: number;
  addedDate: string;
  id: number;
  idColumnName?: string;
  name?: string;
  nameColumnName?: string;
  updatedBy: number;
  updatedDate: string;
  assignedRoles?: string[];
  groups?: string[];
  derivedRoles: string[];
  account?: string;
  email?: string;
  enableNotifications: boolean;
  status: number;
  passwordLastSet: string;
  password?: string;
  failedAttempts: number;
  firstName?: string;
  isSystemAdmin: boolean;
  lastName?: string;
  middleName?: string;
  preferredName?: string;
  timeZone: number;
  location?: string;
  lastLogOn: string;
  getCommaSeparatedAssignedRoles?: string;
  getCommaSeparatedAssignedGroups?: string;
  getCommaSeparatedDerivedRoles?: string;
}

export class AccountProfile implements IAccountProfile {
  public addedBy: number = -1;
  public addedDate: string = '';
  public idColumnName?: string;
  public name?: string;
  public nameColumnName?: string;
  public updatedBy: number = -1;
  public updatedDate: string = '';
  public assignedRoles?: string[];
  public groups?: string[];
  public derivedRoles: string[] = ['Authenticated'];
  public email?: string;
  public enableNotifications: boolean = false;
  public status: number = 4;
  public passwordLastSet: string = '';
  public password?: string;
  public failedAttempts: number = 0;
  public firstName?: string;
  public isSystemAdmin: boolean = false;
  public lastName?: string;
  public middleName?: string;
  public preferredName?: string;
  public timeZone: number = -10;
  public location?: string;
  public lastLogOn: string = '';
  public getCommaSeparatedAssignedRoles?: string;
  public getCommaSeparatedAssignedGroups?: string;
  public getCommaSeparatedDerivedRoles?: string;

  constructor(
    public id: number,
    public account: string,
  ) {}
}
