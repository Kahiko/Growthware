export interface IGroupProfile {
    id: number;
    name: string;
    description: string;
    rolesInGroup: string[];
    rolesNotInGroup: string[];
}

export class GroupProfile implements IGroupProfile {
	id: number = -1;
	name: string = '';
	description: string = '';
	rolesInGroup: string[] = [];
	rolesNotInGroup: string[] = [];  
}
