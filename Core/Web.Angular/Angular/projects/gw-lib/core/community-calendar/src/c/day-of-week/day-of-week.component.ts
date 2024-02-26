import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService} from '@growthware/core/logging';
// Feature
import { CalendarService } from '../../calendar.service';
import { IDay } from '../../interfaces/day.model';
import { NamesOfMonths } from '../../interfaces/names-of-months.enum';
import { NamesOfDays } from '../../interfaces/names-of-days.enum';

@Component({
	selector: 'gw-core-day-of-week',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
	],
	templateUrl: './day-of-week.component.html',
	styleUrls: ['./day-of-week.component.scss']
})
export class DayOfWeekComponent {
	private _FirstDayOfWeek: NamesOfDays = NamesOfDays.Monday;

	public monthNames: Array<string> = [];
	
	@Input() calendarDay!: IDay;
	@Input() weekNumber?: number;

	constructor(
		private _CalendarSvc: CalendarService,
		private _GWCommon: GWCommon,
		private _LoggingService: LoggingService
	) { 
		this.monthNames = this._GWCommon.getEnumNames(NamesOfMonths);
	}

	public onDateClick(date: IDay): void {
		console.log('onDateClick', date);
		this._CalendarSvc.setSelectedDate(date.date);
	}
}
