import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
// Library
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
// Featuer
import { IMonth, Month } from './interfaces/month.model';
import { IWeek, Week } from './interfaces/week.model';
import { IDay, Day } from './interfaces/day.model';
import { NamesOfDays } from './interfaces/names-of-days.enum';
import { ICalendarEvent } from './interfaces/calendar-event.model';
import { ITotalRecords } from '@growthware/common/interfaces';

@Injectable({
	providedIn: 'root'
})
export class CalendarService extends BaseService {
	override addEditModalId: string = 'addEditEvent';
	override modalReason: string = '';
	public selectedEvent: ICalendarEvent = {} as ICalendarEvent;
	override selectedRow: ITotalRecords = { TotalRecords: 0 } as ITotalRecords;

	private _ApiName: string = 'GrowthwareCalendar/';
	private _Api_GetEvents: string = '';
	private _Api_GetEventSecurity: string = '';
	private _Api_SaveEvent: string = '';

	private _CalendarData: BehaviorSubject<IMonth> = new BehaviorSubject<IMonth>(new Month());
	public calendarData$ = this._CalendarData.asObservable();
	private _FirstDayOfWeek: NamesOfDays = NamesOfDays.Monday;
	public get firstDayOfWeek(): NamesOfDays { return this._FirstDayOfWeek; }
	private _SelectedDate: Date = new Date();
	public get selectedDate(): Date { return this._SelectedDate; }

	constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
	) {
		super();
		this._Api_GetEvents = this._GWCommon.baseURL + this._ApiName + 'GetEvents';
		this._Api_GetEventSecurity = this._GWCommon.baseURL + this._ApiName + 'GetEventSecurity';
		this._Api_SaveEvent = this._GWCommon.baseURL + this._ApiName + 'SaveEvent';
	}

	/**
	 * Gets events from the API.
	 *
	 * @param {string} action - The action for the calendar
	 * @param {Date} startDate - The start date or first date for the calendar
	 * @param {Date} endDate - The end date or last date for the calendar
	 * @return {Promise<ICalendarEvent[]>} description of return value
	 */
	private getEvents(action: string, startDate: Date, endDate: Date): Promise<ICalendarEvent[]> {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('action', action)
			.set('startDate', startDate.toLocaleDateString())
			.set('endDate', endDate.toLocaleDateString());
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<ICalendarEvent[]>((resolve, reject) => {
			this._HttpClient.get<ICalendarEvent[]>(this._Api_GetEvents, mHttpOptions).subscribe({
				next: (response: ICalendarEvent[]) => {
					// console.log('CalendarService.getEvents', response);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'CalendarService', 'getEvents');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * Generate the data for a month given a selected date and the first day of the week.
	 * Calls mergeEvents padding the generated data.
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
				mDay.isSelected = this._GWCommon.datesEqual(selectedDate, mDay.date);
				week.days.push(mDay);
				mWorkingDate.setDate(mWorkingDate.getDate() + 1);
			}
			mRetVal.weeks.push(week);
		}
		this.mergeEvents(mRetVal);
	}

	public getEventSecurity(eventId: number): Promise<boolean> {
		return new Promise<boolean>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('calendarEventSeqId', eventId);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.get<boolean>(this._Api_GetEventSecurity, mHttpOptions).subscribe({
				next: (response: boolean) => {
					resolve(response);
				},
				error: (error) => {
					reject('Failed to call the API');
					this._LoggingSvc.errorHandler(error, 'CalendarService', 'getEventSecurity');
				}
			});
		});
	}

	/**
	 * Merge events into the month data and update the calendar data.
	 *
	 * @param {IMonth} monthData - the month data to merge events into
	 * @return {void} 
	 */
	private mergeEvents(monthData: IMonth): void {
		// console.log('mergeEvents.monthData', monthData);
		const mFirstDay = monthData.weeks[0].days[0];
		const mLastDay = monthData.weeks[monthData.weeks.length - 1].days[6];
		this.getEvents('CommunityCalendar', mFirstDay.date, mLastDay.date).then((response) => {
			monthData.weeks.forEach((week) => {
				week.days.forEach((day) => {
					// console.log('mergeEvents.getEvents.day.date', day.date);
					const mFoundEvents = response.filter((event) => {
						return this._GWCommon.datesEqual(new Date(event.start), new Date(day.date));
					});
					if (mFoundEvents.length > 0) {
						day.events = mFoundEvents;
					}
				});
			});
			this._CalendarData.next(monthData);
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'CalendarService', 'mergeEvents');
		});
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
