import { ITotalRecords } from '@growthware/common/interfaces';

export class SelectedRow implements ITotalRecords {
	Added_By: string = '';
	Added_Date: string = '';
	Description: string = '';
	GroupSeqId: number = -1;
	Name: string = '';
	TotalRecords: number = -1;
	Updated_By: string = '';
	Updated_Date: string = '';    
}
