import { ITotalRecords } from '@growthware/common/interfaces';

export class SelectedRow implements ITotalRecords {
	Added_By: string = '';
	Added_Date: string = '';
	Description: string = '';
	MessageSeqId: number = -1;
	Name: string = '';
	Title: string = '';
	TotalRecords: number = -1;
	Updated_By: string = '';
	Updated_Date: string = '';

	constructor() {
        
	}
}
