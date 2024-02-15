import { ITotalRecords } from '@growthware/common/interfaces';

export class SelectedRow implements ITotalRecords {
	Added_By: string = '';
	Added_Date: string = '';
	Description: string = '';
	Is_System: boolean = false;
	Is_System_Only: boolean = false;
	Name: string = '';
	RoleSeqId: number = -1;
	TotalRecords: number = -1;
	Updated_By: string = '';
	Updated_Date: string = '';    
}
