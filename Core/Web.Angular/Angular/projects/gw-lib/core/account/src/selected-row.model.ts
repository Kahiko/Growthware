import { ITotalRecords } from '@growthware/common/interfaces';

export class SelectedRow implements ITotalRecords {
	Account: string = '';
	AccountSeqId: number = -1;
	Added_Date: string = '';
	Email: string = '';
	First_Name: string = '';
	Last_Login: string = '';
	Last_Name: string = '';
	TotalRecords: number = -1;
}
