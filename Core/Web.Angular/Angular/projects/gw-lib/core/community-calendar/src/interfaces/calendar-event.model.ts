export interface ICalendarEvent {
    id: number;
    title: string;
    start: Date;
    end: Date;
    allDay: boolean;
    description: string;
    color: string;
    link: string;
    location: string;
}

export class CalendarEvent implements ICalendarEvent {
	public id: number = -1;
	public title: string = '';
	public start: Date = new Date();
	public end: Date = new Date();
	public allDay: boolean = false;
	public description: string = '';
	public color: string = '#6495ED';
	public link: string = '';
	public location: string = '';

	constructor() { 
		this.end.setMinutes(this.end.getMinutes() + 30);
	}
}
