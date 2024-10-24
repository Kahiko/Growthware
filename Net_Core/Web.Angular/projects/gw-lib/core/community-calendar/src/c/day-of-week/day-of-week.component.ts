import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService} from '@growthware/core/logging';
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { CalendarService } from '../../calendar.service';
import { EventDetailsComponent } from '../event-details/event-details.component';
import { IDay } from '../../interfaces/day.model';
import { NamesOfMonths } from '../../interfaces/names-of-months.enum';
import { NamesOfDays } from '../../interfaces/names-of-days.enum';
import { CalendarEvent, ICalendarEvent } from '../../interfaces/calendar-event.model';

@Component({
	selector: 'gw-core-day-of-week',
	standalone: true,
	imports: [
		CommonModule,
		// Feature
		EventDetailsComponent,
		// Angular Material
		MatButtonModule,
		MatIconModule,
		MatToolbarModule,
		MatTooltipModule,
	],
	templateUrl: './day-of-week.component.html',
	styleUrls: ['./day-of-week.component.scss']
})
export class DayOfWeekComponent {
	private _Action: string = '';
	private _FirstDayOfWeek: NamesOfDays = NamesOfDays.Monday;

	public monthNames: Array<string> = [];
	
	calendarDay = input.required<IDay>();
	weekNumber = input<number>();

	constructor(
		private _CalendarSvc: CalendarService,
		private _GWCommon: GWCommon,
		private _LoggingService: LoggingService,
		private _ModalSvc: ModalService,
		private _Router: Router
	) { 
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this.monthNames = this._GWCommon.getEnumNames(NamesOfMonths);
	}

	/**
	 * Returns the header class for the day.
	 * 
	 * @param {IDay} date - The date to be selected.
	 * @returns Array<string>
	 */
	public headerClass(calendarDay: IDay): Array<string> {
		const mRetVal = ['header'];
		const mCurrentMonth = calendarDay.date.toLocaleString('default', { month: 'long' });
		const mSelectedtMonth = this._CalendarSvc.selectedDate.toLocaleString('default', { month: 'long' });
		if (mCurrentMonth !== mSelectedtMonth) {
			mRetVal.push('grey-header');
		} else  if (calendarDay.isSelected) {
			mRetVal.push('selected-header');
		}
		return mRetVal;
	}

	/**
	 * Handles the date header click event.
	 *
	 * @param {IDay} date - The date to be selected.
	 * @return {void} - This function does not return anything.
	 */
	public onDateClick(date: IDay): void {
		// console.log('onDateClick', date);
		this._CalendarSvc.setSelectedDate(this._Action, date.date);
	}

	/**
	 * Handles the Add Event button click.
	 *
	 * @param {IDay} date - The date the event will be added to.
	 * @return {void} - This function does not return anything.
	 */
	public onAddEventClick(date: IDay) {
		// console.log('onDateClick', date);
		this._CalendarSvc.setSelectedDate(this._Action, date.date);
		const mEvent = new CalendarEvent();
		const mEndDate: Date = new Date(date.date);
		mEndDate.setMinutes(mEndDate.getMinutes() + 15);
		mEvent.end = mEndDate.toString();
		mEvent.start = date.date.toString();
		this._CalendarSvc.selectedEvent = mEvent;
		this.openEventDetails();
	}

	public onEventClick(date: IDay, event: ICalendarEvent) {
		// console.log('DayOfWeekComponent.onEditEventClick', event);
		this._CalendarSvc.getEvent(this._Action, event.id).then((response: ICalendarEvent) => {
			this._CalendarSvc.setSelectedDate(this._Action, date.date);
			this._CalendarSvc.selectedEvent = response;
			this.openEventDetails();				
		}).catch(() => {
			this._LoggingService.errorHandler('Error attempting to get an event from CalendarService', 'DayOfWeekComponent', 'onEventClick');
		});
	}

	private openEventDetails() {
		let mTitle: string = 'Add Event';
		if(this._CalendarSvc.selectedEvent.id > 0) {
			mTitle = 'Edit Event';
		}
		const mWindowSize: WindowSize = new WindowSize(575, 450);
		const mModalOptions: ModalOptions = new ModalOptions(this._CalendarSvc.addEditModalId, mTitle, EventDetailsComponent, mWindowSize);
		// mModalOptions.buttons.okButton.callbackMethod = () => {
		// 	this.onModalOk;
		// };
		this._ModalSvc.open(mModalOptions);		
	}

	private onModalOk() {
		this._ModalSvc.close(this._CalendarSvc.addEditModalId);
	}
}
