import { ISelectedRow } from '@growthware/core/base/interfaces';

export class SelectedRow implements ISelectedRow {
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
