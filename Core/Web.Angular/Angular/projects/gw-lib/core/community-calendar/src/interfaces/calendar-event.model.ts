export interface ICalendarEvent {
    allDay: boolean;
    color: string;
    description: string;
    end: string;
    id: number;
	owner: string;
    title: string;
    start: string;
    link: string;
    location: string;
}

export class CalendarEvent implements ICalendarEvent {
	public allDay: boolean = false;
	public color: string = '#6495ED';
	public description: string = '';
	public end: string = '';
	public id: number = -1;
	public owner: string = '';
	public title: string = '';
	public start: string = '';
	public link: string = '';
	public location: string = '';

	constructor() { 
		// this.end.setMinutes(this.end.getMinutes() + 30);
	}
}
