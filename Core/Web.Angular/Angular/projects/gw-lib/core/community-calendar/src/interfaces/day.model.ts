import { IEvent } from './event.model';
import { NamesOfDays } from './names-of-days.enum';
import { NamesOfMonths } from './names-of-months.enum';

export interface IDay {
	day: NamesOfDays;
	date: Date;
	events?: IEvent[];
	isFutureMonth: boolean;
	isPreviousMonth: boolean;
	isSelected: boolean;
	isToday: boolean;
	month: NamesOfMonths;
	monthName: string;
}

export class Day implements IDay {
	public day!: NamesOfDays;
	public date: Date;
	public events?: IEvent[];
	public isFutureMonth: boolean = false;
	public isPreviousMonth: boolean = false;
	public isSelected: boolean = false;
	public isToday: boolean = false;
	public month: NamesOfMonths;
	public monthName: string = '';

	constructor(date: Date) {
		this.day = date.getDay() as NamesOfDays;
		this.date = date;
		this.isFutureMonth = date.getMonth() > new Date().getMonth();
		this.isPreviousMonth = date.getMonth() < new Date().getMonth();
		this.month = date.getMonth() as NamesOfMonths;
		this.monthName = NamesOfDays[this.day];
	}
}