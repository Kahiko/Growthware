import { ISelectedRow } from '@growthware/core/base/interfaces';

export class SelectedRow implements ISelectedRow {
	Added_By: string;
	Added_Date: string;
	Description: string;
	State: string;
	Status: string;
	TotalRecords: number;
	Updated_By: string;
	Updated_Date: string;

	constructor() {
		this.Added_By = '';
		this.Added_Date = '';
		this.Description = '';
		this.State = '';
		this.Status = '';
		this.TotalRecords = -1;
		this.Updated_By = '';
		this.Updated_Date = '';
	}
}
