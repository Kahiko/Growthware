export interface ISideNavLink {
  "icon": string;
  "link": string;
  "linkText": string;
  "isRouterLink": boolean;
  "styleClass": string;
  "routerLinkActive": string;
}

export class SideNavLink implements ISideNavLink {
  constructor(
    public icon: string,
    public link: string,
    public linkText: string,
    public isRouterLink: boolean = true,
    public styleClass: string = '',
    public routerLinkActive: string = '',
  ) {}
}
