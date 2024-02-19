export interface IEvent {
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
