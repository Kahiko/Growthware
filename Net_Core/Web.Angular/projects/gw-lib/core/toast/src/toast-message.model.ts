import { EventType } from './event-type.enum';

export interface IToastMessage {
  id: string;
  dateTime: string;
  eventType: EventType;
  message: string;
  title: string;
}

export class ToastMessage implements IToastMessage {
  public dateTime: string = new Date().toLocaleString();
  public id: string = '';
  public message: string = ''; 
  public title: string = ''; 
  public eventType!: EventType

  constructor(message: string, title: string, eventType: EventType) { 
    this.message = message;
    this.title = title;
    this.eventType = eventType;
    this.id = this.message + this.dateTime
  }

}
