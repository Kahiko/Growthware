import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService} from '@growthware/core/logging';
// Feature
import { IDay } from '../../interfaces/day.model';

@Component({
	selector: 'gw-core-day-of-week',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
		MatButtonModule,
	],
	templateUrl: './day-of-week.component.html',
	styleUrls: ['./day-of-week.component.css']
})
export class DayOfWeekComponent {

	public monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
		'July', 'August', 'September', 'October', 'November', 'December'
	];
	
	@Input() calendarDay!: IDay;
	@Input() weekNumber?: number;

	constructor(
		private _GWCommon: GWCommon,
		private _LoggingService: LoggingService
	) { }
}
