import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Library
import { GWCommon } from '@growthware/common/services';
// Feature
// import { CalendarDay } from '../../calendar-day.model';
import { CalendarService } from '../../calendar.service';
import { DayOfWeekComponent } from '../day-of-week/day-of-week.component';
import { IMonth, Month } from '../../interfaces/month.model';
import { NamesOfDays } from '../../interfaces/names-of-days.enum';

@Component({
	selector: 'gw-core-calendar',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
		MatButtonModule,
		MatIconModule,
		// Feature
		DayOfWeekComponent
	],
	templateUrl: './calendar.component.html',
	styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
	private _FirstDayOfWeek: NamesOfDays = NamesOfDays.Monday;
	private _SelectedDate: Date = new Date();
	// public calendar: CalendarDay[] = [];
	public calendar: IMonth = new Month();
	public weekDayNames: string[] = [];
	public monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
		'July', 'August', 'September', 'October', 'November', 'December'
	];
	public displayMonth: string = '';

	constructor(
		private _CalendarSvc: CalendarService,
    	private _GWCommon: GWCommon
	){}

	ngOnInit(): void {
		this._FirstDayOfWeek = NamesOfDays.Sunday;
		this.getCalendarData();
		// console.log('CalendarComponent.ngOnInit.calendar', this.calendar);
		this.getWeekDayNames();
		// console.log('CalendarComponent.ngOnInit.weekDayNames', this.weekDayNames);
	}

	public increaseMonth() {
		this._SelectedDate.setMonth(this._SelectedDate.getMonth() + 1);
		this.getCalendarData();
	}

	public decreaseMonth() {
		this._SelectedDate.setMonth(this._SelectedDate.getMonth() - 1);
		this.getCalendarData();
	}

	public setCurrentMonth() {
		this._SelectedDate = new Date();
		this.getCalendarData();
	}

	private getCalendarData(): void {
		this.calendar = this._CalendarSvc.getMonthData(this._SelectedDate, this._FirstDayOfWeek);
	}

	private getWeekDayNames(): void {
		this.weekDayNames.length = 0;
		let mDay: number = this._FirstDayOfWeek;
		for (let i = 0; i < 7; i++) {
			this.weekDayNames.push(NamesOfDays[mDay]);
			if (mDay === 6) {
				mDay = 0;
			} else {
				mDay++;
			}
		}
	}
}
