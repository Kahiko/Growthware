export interface ICalendarEvent {
    allDay: boolean;
    color: string;
    description: string;
    end: Date;
    id: number;
	owner: string;
    title: string;
    start: Date;
    link: string;
    location: string;
}

export class CalendarEvent implements ICalendarEvent {
	public allDay: boolean = false;
	public color: string = '#6495ED';
	public description: string = '';
	public end: Date = new Date();
	public id: number = -1;
	public owner: string = '';
	public title: string = '';
	public start: Date = new Date();
	public link: string = '';
	public location: string = '';

	constructor() { 
		this.end.setMinutes(this.end.getMinutes() + 30);
	}
}
