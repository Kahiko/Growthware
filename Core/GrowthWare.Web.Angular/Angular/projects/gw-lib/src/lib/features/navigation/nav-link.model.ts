export interface INavLink {
  "disabled": boolean
  "icon": string;
  "link": string;
  "linkText": string;
  "isRouterLink": boolean;
  "styleClass": string;
  "routerLinkActive": string;
  "children": INavLink[];
}

export class NavLink implements INavLink {
  public disabled = false;
  // public children = new Array<INavLink>;

  constructor(
    public icon: string,
    public link: string,
    public linkText: string,
    public isRouterLink: boolean = true,
    public styleClass: string = '',
    public routerLinkActive: string = '',
    public children:INavLink[] = [],
  ) {}
}
