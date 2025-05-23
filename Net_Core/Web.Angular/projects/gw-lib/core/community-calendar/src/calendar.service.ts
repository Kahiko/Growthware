import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
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
	private _ApiDeleteEvent: string = '';
	private _Api_GetEvent: string = '';
	private _Api_GetEvents: string = '';
	private _Api_GetEventSecurity: string = '';
	private _Api_SaveEvent: string = '';

	public calendarData$ = signal<IMonth>(new Month());
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
		this._ApiDeleteEvent = this._GWCommon.baseURL + this._ApiName + 'DeleteEvent';
		this._Api_GetEvent = this._GWCommon.baseURL + this._ApiName + 'GetEvent';
		this._Api_GetEvents = this._GWCommon.baseURL + this._ApiName + 'GetEvents';
		this._Api_GetEventSecurity = this._GWCommon.baseURL + this._ApiName + 'GetEventSecurity';
		this._Api_SaveEvent = this._GWCommon.baseURL + this._ApiName + 'SaveEvent';
	}

	/**
	 * Delete an event by its ID.
	 *
	 * @param {number} eventId - The ID of the event to delete
	 * @param {string} action - The action for the calendar
	 * @return {Promise<boolean>} A promise that resolves with a boolean indicating the success of the deletion
	 */
	public deleteEvent(eventId: number, action: string): Promise<boolean> {
		if (!eventId || !action) {
			throw new Error('Event ID and action are required');
		}
		return new Promise<boolean>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.set('calendarEventSeqId', eventId)
				.set('action', action);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.get<boolean>(this._ApiDeleteEvent, mHttpOptions).subscribe({
				next: (response: boolean) => {
					if (response) {
						const mCalendarData = this.calendarData$();
						outerLoop: for (let mWeekIndex = 0; mWeekIndex < mCalendarData.weeks.length; mWeekIndex++) {
							const mDays = mCalendarData.weeks[mWeekIndex].days;
							for (let mDayIndex = 0; mDayIndex < mDays.length; mDayIndex++) {
								const mDay = mDays[mDayIndex];
								if (mDay.events) {
									const mEvents = mDay.events;
									for (let mEvenIndex = 0; mEvenIndex < mEvents.length; mEvenIndex++) {
										const mEvent = mEvents[mEvenIndex];
										if (mEvent && mEvent.id === eventId) {
											mDay.events = mDay.events.filter(obj => obj !== mEvent);
											break outerLoop;
										}
									}
								}
							}
						}
						resolve(response);
					} else {
						reject(response);
					}
				},
				error: (error: HttpErrorResponse | string) => {
					this._LoggingSvc.errorHandler(error, 'CalendarService', 'deleteEvent');
					reject(error);
				}
			});
		});
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
	 * Retrieves an event from the API.
	 * @param {string} action - The action for the calendar
	 * @param {number} id - The ID of the event to retrieve
	 * @return {Promise<ICalendarEvent>}
	 * @memberof CalendarService
	 */
	public getEvent(action: string, id: number): Promise<ICalendarEvent> {
		const mQueryParameter: HttpParams = new HttpParams()
			.set('action', action)
			.set('id', id.toString());
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		return new Promise<ICalendarEvent>((resolve, reject) => {
			this._HttpClient.get<ICalendarEvent>(this._Api_GetEvent, mHttpOptions).subscribe({
				next: (response: ICalendarEvent) => {
					// console.log('CalendarService.getEvent', response);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'CalendarService', 'getEvent');
					reject('Failed to call the API');
				},
				// complete: () => {}
			});
		});
	}

	/**
	 * @descriptionGenerate the data for a month given a selected date and the first day of the week.
	 * Calls mergeEvents padding the generated data.
	 *
	 * @param {Date} selectedDate - the selected date
	 * @return {IMonth} the month data
	 * @memberof CalendarService
	 */
	private getMonthData(action: string, selectedDate: Date): void {
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
		this.mergeEvents(action, mRetVal);
	}

	/**
	 * Retrieves the security status of a specific event.
	 *
	 * @param {number} eventId - the ID of the event to retrieve security status for
	 * @return {Promise<boolean>} a Promise that resolves to a boolean indicating the security status
	 */
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
	private mergeEvents(action: string, monthData: IMonth): void {
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
			this.calendarData$.update(() => monthData);
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'CalendarService', 'mergeEvents');
		});
	}

	/**
	 * Save an event with the provided action and calendar event data.
	 *
	 * @param {string} action - The action to be performed.
	 * @param {ICalendarEvent} calendarEvent - The calendar event data to be saved.
	 * @return {Promise<boolean>} A promise that resolves to a boolean indicating the success of the save operation.
	 */
	public saveEvent(action: string, calendarEvent: ICalendarEvent): Promise<boolean> {
		// TODO: 
		// 1.) Add logic checking for overlapping events
		// 2.) Ensure that the start time is less than the end time

		return new Promise<boolean>((resolve, reject) => {
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
			};
			// The parameters names match the property names in the UISaveEventParameters.cs model
			const mParameters = {
				action: action,
				calendarEvent: calendarEvent
			};
			// console.log('mParameters.calendarEvent.start Before :', mParameters.calendarEvent.start);
			// console.log('mParameters.calendarEvent.end Before :', mParameters.calendarEvent.end);
			mParameters.calendarEvent.start = new Date(mParameters.calendarEvent.start).toISOString();
			mParameters.calendarEvent.end = new Date(mParameters.calendarEvent.end).toISOString();
			// console.log('mParameters.calendarEvent.start After :', mParameters.calendarEvent.start);
			// console.log('mParameters.calendarEvent.end After :', mParameters.calendarEvent.end);
			this._HttpClient.post<ICalendarEvent>(this._Api_SaveEvent, mParameters, mHttpOptions).subscribe({
				next: (response: ICalendarEvent) => {
					// Get a working copy of the canendar data
					const mCalendarData = this.calendarData$();
					/**
					 * If the id is greater than 0, then it is an update and needs to be removed so it can be added again.
					 * Doing so accounts for when an event date changes.
					 */
					if (calendarEvent.id > 0) {
						// Update with response
						// 1.) Remove the Event from the current month it may have changed dates
						for (let mWeekIndex = 0; mWeekIndex < mCalendarData.weeks.length; mWeekIndex++) {
							mCalendarData.weeks[mWeekIndex].days.forEach(mDay => {
								if (mDay.events) {
									mDay.events = mDay.events.filter(obj => obj.id !== response.id);
								}
							})
						}
					}
					// Add response
					outerLoop: for (let mWeekIndex = 0; mWeekIndex < mCalendarData.weeks.length; mWeekIndex++) {
						const mDays = mCalendarData.weeks[mWeekIndex].days;
						for (let mDayIndex = 0; mDayIndex < mDays.length; mDayIndex++) {
							const mDay = mDays[mDayIndex];
							if (this._GWCommon.datesEqual(mDay.date, new Date(response.start))) {
								if (mDay.events) {
									mDay.events.push(response);
									mDay.events.sort((a, b) => new Date(a.start).getTime() - new Date(b.start).getTime());
								} else {
									mDay.events = [response];
								}
								break outerLoop;
							}
						}
					}
					// Update the calendar signal with the updated data
					this.calendarData$.update(() => mCalendarData);
					resolve(true);
				},
				error: (error) => {
					reject('Failed to call the API');
					this._LoggingSvc.errorHandler(error, 'CalendarService', 'saveEvent');
				}
			});
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
	public setSelectedDate(action: string, newSelectedDate: Date, force: boolean = false): void {
		const mMonthMatch = newSelectedDate.getMonth() === this._SelectedDate.getMonth();
		const mYearMatch = newSelectedDate.getFullYear() === this._SelectedDate.getFullYear();
		this._SelectedDate = newSelectedDate;
		if (!force && mMonthMatch && mYearMatch) {
			this.calendarData$().weeks.some((week: IWeek) => {
				week.days.some((day: IDay) => {
					if (day.isSelected) {
						day.isSelected = false;
					}
				});
			});
			this.calendarData$().weeks.some((week: IWeek) => {
				week.days.some((day: IDay) => {
					if (this._GWCommon.datesEqual(day.date, newSelectedDate)) {
						day.isSelected = true;
					}
				});
			});
			this.calendarData$.update(() => this.calendarData$());
		} else {
			this.getMonthData(action, newSelectedDate);
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
