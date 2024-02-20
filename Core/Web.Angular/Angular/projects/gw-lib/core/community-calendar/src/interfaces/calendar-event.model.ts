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
	public id!: number;
	public title!: string;
	public start!: Date;
	public end!: Date;
	public allDay!: boolean;
	public description!: string;
	public color!: string;
	public link!: string;
	public location!: string;
}