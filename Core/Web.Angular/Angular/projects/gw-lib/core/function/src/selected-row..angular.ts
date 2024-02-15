import { ITotalRecords } from '@growthware/common/interfaces';

export class SelectedRow implements ITotalRecords {
	Action: string = '';
	Added_By: string = '';
	Added_Date: string = '';
	Description: string = '';
	FunctionSeqId: number = -1;
	Name: string = '';
	TotalRecords: number = -1;
	Updated_By: string = '';
	Updated_Date: string = '';
}
