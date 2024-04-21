export interface INavItem {
    action: string;
    description: string;
    items: INavItem[];
    label: string;
    parentId: number;
}

export class NavItem implements INavItem {
	public action: string = '';
	public description: string = '';
	public items: INavItem[] = [new NavItem()];
	public label: string = '';
	public parentId: number = -1;

	constructor( ) {}

}