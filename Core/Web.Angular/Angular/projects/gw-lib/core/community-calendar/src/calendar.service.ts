import { Injectable } from '@angular/core';
// Library
import { GWCommon } from '@growthware/common/services';
// Featuer
import { IMonth, Month } from './interfaces/month.model';
import { IWeek, Week } from './interfaces/week.model';
import { Day } from './interfaces/day.model';
import { NamesOfDays } from './interfaces/names-of-days.enum';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class CalendarService {

	private _CalendarData: BehaviorSubject<IMonth> = new BehaviorSubject<IMonth>(new Month());
	public calendarData$ = this._CalendarData.asObservable();

	constructor(
		private _GWCommon: GWCommon
	) { }

	
	/**
	 * Generate the data for a month given a selected date and the first day of the week.
	 *
	 * @param {Date} selectedDate - the selected date
	 * @param {NamesOfDays} firstDayOfWeek - the first day of the week IE NamesOfDays.Sunday
	 * @return {IMonth} the month data
	 */
	getMonthData(selectedDate: Date, firstDayOfWeek: NamesOfDays): void {
		const mRetVal: IMonth = new Month();
		const mWorkingDate = new Date(selectedDate);
		const mCurrentMonth = selectedDate.getMonth();
		let mPreviousMonth = selectedDate.getMonth() -1;

		// Adjust for the previous month being in the previous year
		if(mPreviousMonth === -1) {
			mPreviousMonth = 11;
		}
		// Set to the first day of the month
		mWorkingDate.setDate(1);
		// Move to the first day to display in the calendar
		while (mWorkingDate.getDay() !== firstDayOfWeek) {
			mWorkingDate.setDate(mWorkingDate.getDate() - 1);
		}
		// Interate for the working date creating IDay objects and IWeek objects, then
		// adding them to the month data.
		while (mWorkingDate.getMonth() === mPreviousMonth || mWorkingDate.getMonth() === mCurrentMonth) {
			const week: IWeek = new Week();
			// Iterating through 7 days ensures the last week will have necessary days of
			// the next month.
			for (let i = 0; i < 7; i++) {
				// const mNewWorkingDate = new Date(mWorkingDate);
				const mDay = new Day(new Date(mWorkingDate));
				week.days.push(mDay);
				mWorkingDate.setDate(mWorkingDate.getDate() + 1);
			}
			mRetVal.weeks.push(week);
		}
		this._CalendarData.next(mRetVal);
	}
}
