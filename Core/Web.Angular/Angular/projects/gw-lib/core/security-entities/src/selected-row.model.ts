import { ITotalRecords } from '@growthware/common/interfaces';

export class SelectedRow implements ITotalRecords {
	Description: string = '';
	Name: string = '';
	SecurityEntitySeqId: number = -1;
	Skin: string = '';
	TotalRecords: number = -1;
}
