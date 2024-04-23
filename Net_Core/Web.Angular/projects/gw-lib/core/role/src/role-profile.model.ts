export interface IRoleProfile {
    accountsInRole: string[];
    accountsNotInRole: string[];
    id: number;
    isSystem: boolean;
    isSystemOnly: boolean;
    name: string;
    description: string;
}
export class RoleProfile implements IRoleProfile {
	accountsInRole: string[] = [];
	accountsNotInRole: string[] = [];
	id: number = -1;
	isSystem: boolean = false;
	isSystemOnly: boolean = false;
	name: string = '';
	description: string = '';
	constructor() {
        
	}
}
