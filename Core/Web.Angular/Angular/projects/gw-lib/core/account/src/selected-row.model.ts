import { ISelectedRow } from '@growthware/core/base/interfaces';

export class SelectedRow implements ISelectedRow {
	AccountSeqId: number = -1;
	Added_Date: string = '';
	Email: string = '';
	First_Name: string = '';
	Last_Login: string = '';
	Last_Name: string = '';
	TotalRecords: number = -1;
}
