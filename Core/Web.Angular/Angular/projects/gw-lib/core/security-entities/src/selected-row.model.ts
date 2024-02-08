import { ISelectedRow } from '@growthware/core/base/interfaces';

export class SelectedRow implements ISelectedRow {
	Description: string = '';
	Name: string = '';
	SecurityEntitySeqId: number = -1;
	Skin: string = '';
	TotalRecords: number = -1;
}
