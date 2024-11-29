/**
 * Note this matches C# object GrowthWare.Framework.Models.MNavLink
 * any changes needed here will also need to be made there
 */

export interface INavLink {
  'action': string
  'description': string
  'disabled': boolean
  'icon': string;
  'id': number;
  'isActive': boolean;
  'label': string;
  'link': string;
  'linkBehavior': number;
  'linkText': string;
  'isRouterLink': boolean;
  'styleClass': string;
  'parentId': number;
  'routerLinkActive': string;
  'children': INavLink[];
}

export class NavLink implements INavLink {
	public disabled = false;
  public id: number = -1;
	public isActive = false;
	// public children = new Array<INavLink>;

	constructor(
    public action: string,
    public description: string,
    public icon: string,
    public label: string,
    public link: string,
    public linkBehavior: number,
    public linkText: string,
    public isRouterLink: boolean = true,
    public styleClass: string = '',
    public parentId: number = -1,
    public routerLinkActive: string = '',
    public children:INavLink[] = [],
	) {}
}
