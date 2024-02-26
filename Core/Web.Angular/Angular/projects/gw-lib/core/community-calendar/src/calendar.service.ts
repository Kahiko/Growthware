import { Injectable } from '@angular/core';
// Library
import { GWCommon } from '@growthware/common/services';
// Featuer
import { IMonth, Month } from './interfaces/month.model';
import { IWeek, Week } from './interfaces/week.model';
import { IDay, Day } from './interfaces/day.model';
import { NamesOfDays } from './interfaces/names-of-days.enum';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class CalendarService {

	private _CalendarData: BehaviorSubject<IMonth> = new BehaviorSubject<IMonth>(new Month());
	public calendarData$ = this._CalendarData.asObservable();
	private _FirstDayOfWeek: NamesOfDays = NamesOfDays.Monday;
	public get firstDayOfWeek(): NamesOfDays { return this._FirstDayOfWeek; }
	private _SelectedDate: Date = new Date();
	public get selectedDate(): Date { return this._SelectedDate; }

	constructor(
		private _GWCommon: GWCommon
	) { }


	/**
	 * Generate the data for a month given a selected date and the first day of the week.
	 *
	 * @param {Date} selectedDate - the selected date
	 * @return {IMonth} the month data
	 * @memberof CalendarService
	 */
	private getMonthData(selectedDate: Date): void {
		const mRetVal: IMonth = new Month();
		const mWorkingDate = new Date(selectedDate);
		const mCurrentMonth = selectedDate.getMonth();
		let mPreviousMonth = selectedDate.getMonth() - 1;

		// Adjust for the previous month being in the previous year
		if (mPreviousMonth === -1) {
			mPreviousMonth = 11;
		}
		// Set to the first day of the month
		mWorkingDate.setDate(1);
		// Move to the first day to display in the calendar
		while (mWorkingDate.getDay() !== this._FirstDayOfWeek) {
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
				mDay.isToday = this._GWCommon.datesEqual(new Date(), mDay.date);
				mDay.isSelected = this._GWCommon.datesEqual(selectedDate, mDay.date);
				week.days.push(mDay);
				mWorkingDate.setDate(mWorkingDate.getDate() + 1);
			}
			mRetVal.weeks.push(week);
		}
		this._CalendarData.next(mRetVal);
	}

	/**
	 * Sets the selected date and updates the calendar data if necessary.
	 *
	 * @param {Date} newSelectedDate - the new selected date
	 * @param {boolean} force - optional parameter to force the update
	 * @return {void} 
	 * @memberof CalendarService
	 */
	public setSelectedDate(newSelectedDate: Date, force: boolean = false): void {
		const mMonthMatch = newSelectedDate.getMonth() === this._SelectedDate.getMonth();
		const mYearMatch = newSelectedDate.getFullYear() === this._SelectedDate.getFullYear();
		this._SelectedDate = newSelectedDate;
		if (!force && mMonthMatch && mYearMatch) {
			this._CalendarData.getValue().weeks.some((week: IWeek) => {
				week.days.some((day: IDay) => { 
					if (day.isSelected) {
						day.isSelected = false; 
					}
				}); 
			});
			this._CalendarData.getValue().weeks.some((week: IWeek) => { 
				week.days.some((day: IDay) => { 
					if (this._GWCommon.datesEqual(day.date, newSelectedDate)) { 
						day.isSelected = true; 
					}
				}); 
			});
			this._CalendarData.next(this._CalendarData.getValue());
		} else {
			this.getMonthData(newSelectedDate);
		}
	}

	/**
	 * Sets the first day of the week.
	 *
	 * @param {NamesOfDays} firstDayOfWeek - the name of the first day of the week
	 * @return {void} 
	 * @memberof CalendarService
	 */
	public setFirstDayOfWeek(firstDayOfWeek: NamesOfDays): void {
		// typically 0 = Sunday or 1 = Monday and set by the calendar.component
		this._FirstDayOfWeek = firstDayOfWeek;
	}
}
