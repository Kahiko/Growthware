import { Injectable } from '@angular/core';
// Library
import { GWCommon } from '@growthware/common/services';
// Featuer
import { IMonth, Month } from './interfaces/month.model';
import { IWeek, Week } from './interfaces/week.model';
import { Day } from './interfaces/day.model';
import { NamesOfDays } from './interfaces/names-of-days.enum';

@Injectable({
	providedIn: 'root'
})
export class CalendarService {

	constructor(
		private _GWCommon: GWCommon
	) { }

	getMonthData(selectedDate: Date, firstDayOfWeek: NamesOfDays): IMonth {
		const monthData: IMonth = new Month();
		const mWorkingDate = new Date(selectedDate);
		const mCurrentMonth = selectedDate.getMonth();
		let mPreviousMonth = selectedDate.getMonth() -1;

		// Adjust for the previous month being in the previous year
		if(mPreviousMonth === -1) {
			mPreviousMonth = 11;
		}

		mWorkingDate.setDate(1); // Set to the first day of the month
	
		// Move to the first day to display in the calendar
		while (mWorkingDate.getDay() !== firstDayOfWeek) {
			mWorkingDate.setDate(mWorkingDate.getDate() - 1);
		}

		// Interate through the days creating and adding them to the month as long
		// as they fall in the previous or current months
		while (mWorkingDate.getMonth() === mPreviousMonth || mWorkingDate.getMonth() === mCurrentMonth) {
			const week: IWeek = new Week();
			// Iterate through days of the week
			// looping through 7 days ensures the last week will have necessary days for
			// the next month.
			for (let i = 0; i < 7; i++) {
				// const mNewWorkingDate = new Date(mWorkingDate);
				const mDay = new Day(new Date(mWorkingDate));
				week.days.push(mDay);
				mWorkingDate.setDate(mWorkingDate.getDate() + 1);
			}
			monthData.weeks.push(week);
		}

		return monthData;
	}
}
