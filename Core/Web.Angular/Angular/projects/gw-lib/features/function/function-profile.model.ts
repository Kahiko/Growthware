export interface IFunctionProfile {
  action: string;
  description: string;
  enableViewState: boolean;
  enableNotifications: boolean;
  id: number;
  isNavigable: boolean;
  linkBehavior: number;
  functionTypeSeqId: number;
  groups: string[];
  metaKeywords: string;
  navigationTypeSeqId: number;
  notes: string;
  noUI: boolean;
  parentId: number;
  redirectOnTimeout: boolean;
  roles: string[];
  source: string;
  controller: string;
}

export class FunctionProfile implements IFunctionProfile {
  public action: string = '';
  public description: string = '';
  public enableViewState: boolean = false;
  public enableNotifications: boolean = false;
  public id: number = -1;
  public isNavigable: boolean = false;
  public linkBehavior: number = 1;
  public functionTypeSeqId: number = 3;
  public groups: string[] = [];
  public metaKeywords: string = '';
  public navigationTypeSeqId: number = 3;
  public notes: string = '';
  public noUI: boolean = false;
  public parentId: number = -1;
  public redirectOnTimeout: boolean = false;
  public roles: string[] = [];
  public source: string = '';
  public controller: string = '';

  constructor() {}
}
