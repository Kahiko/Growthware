import { IDirectoryData, DirectoryData } from "./directory-data.model";
import { IFunctionMenuOrder } from "./function-menu-order.model";

export interface IFunctionProfile {
  action: string;
  assignedViewRoles: string[];
  assignedAddRoles: string[];
  assignedEditRoles: string[];
  assignedDeleteRoles: string[];

  addGroups: string[];

  canSaveGroups: boolean;
  canSaveRoles: boolean;

  deleteGroups: string[];
  editGroups: string[];
  viewGroups: string[];

  derivedViewRoles: string[];
  derivedAddRoles: string[];
  derivedEditRoles: string[];
  derivedDeleteRoles: string[];

  directoryData: IDirectoryData;

  description: string;
  enableViewState: boolean;
  enableNotifications: boolean;
  id: number;
  functionMenuOrders: IFunctionMenuOrder[];
  isNavigable: boolean;
  linkBehavior: number;
  functionTypeSeqId: number;
  groups: string[];
  name: string;
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
  public assignedViewRoles: string[] = [];
  public assignedAddRoles: string[] = [];
  public assignedEditRoles: string[] = [];
  public assignedDeleteRoles: string[] = [];
  public directoryData: IDirectoryData = new DirectoryData();
  public addGroups: string[] = [];
  public deleteGroups: string[] = [];
  public editGroups: string[] = [];
  public viewGroups: string[] = [];

  public canSaveGroups: boolean = false;
  public canSaveRoles: boolean = false;

  public derivedViewRoles: string[] = [];
  public derivedAddRoles: string[] = [];
  public derivedEditRoles: string[] = [];
  public derivedDeleteRoles: string[] = [];
  public description: string = '';

  public enableViewState: boolean = false;
  public enableNotifications: boolean = false;
  public id: number = -1;
  public isNavigable: boolean = false;
  public linkBehavior: number = 1;
  public functionMenuOrders: IFunctionMenuOrder[] = [];
  public functionTypeSeqId: number = 3;
  public groups: string[] = [];
  public metaKeywords: string = '';
  public name: string = '';
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
