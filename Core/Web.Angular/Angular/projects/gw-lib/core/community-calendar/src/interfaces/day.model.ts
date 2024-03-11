import { ICalendarEvent } from './calendar-event.model';
import { NamesOfDays } from './names-of-days.enum';
import { NamesOfMonths } from './names-of-months.enum';

export interface IDay {
	day: NamesOfDays;
	date: Date;
	events?: ICalendarEvent[];
	isSelected: boolean;
	isToday: boolean;
	month: NamesOfMonths;
	monthName: string;
}

export class Day implements IDay {
	public day!: NamesOfDays;
	public date: Date;
	public events?: ICalendarEvent[];
	public isSelected: boolean = false;
	public isToday: boolean = false;
	public month: NamesOfMonths;
	public monthName: string = '';

	constructor(date: Date) {
		this.day = date.getDay() as NamesOfDays;
		this.date = date;
		this.month = date.getMonth() as NamesOfMonths;
		this.monthName = NamesOfMonths[date.getMonth()];
	}
}