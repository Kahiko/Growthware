import { Component, OnInit } from '@angular/core';

// Angular Material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
// Library
import { LoggingService } from '@growthware/core/logging';
import { INaturalSortResults } from '@growthware/common/interfaces';
// Feature
import { SysAdminService } from '../../sys-admin.service';


@Component({
	selector: 'gw-core-natural-sort',
	standalone: true,
	imports: [
		MatSelectModule,
		MatFormFieldModule,
		MatTableModule
	],
	templateUrl: './natural-sort.component.html',
	styleUrls: ['./natural-sort.component.scss']
})
export class NaturalSortComponent implements OnInit {

	dataTableSource: unknown[] = [];
	dataViewSource: unknown[] = [];
	displayedColumns: string[] = ['col1', 'col2'];

	startTime: string = '';
	stopTime: string = '';

	selectedSortDirection: string = 'ASC';

	totalMilliseconds: string = '';

	validSortDirections = [
		{ id: 'ASC', text: 'Asending' },
		{ id: 'DESC', text: 'Desending' },
	];

	constructor(
		private _SysAdminSvc: SysAdminService,
		private _LoggingSvc: LoggingService
	) { }

	ngOnInit(): void {
		this.getData();
	}

	getData(): void {
		this._SysAdminSvc.getTestNaturalSort(this.selectedSortDirection).then((response: INaturalSortResults) => {
			// console.log('NaturalSortComponent.ngOnInit',response.dataTable);
			this.dataTableSource = response.dataTable;
			this.dataViewSource = response.dataView;
			this.startTime = response.startTime;
			this.stopTime = response.stopTime;
			this.totalMilliseconds = response.totalMilliseconds;
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getTestNaturalSort');
		});
	}

}
