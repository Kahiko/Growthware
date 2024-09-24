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
  public id = this.message + this.dateTime;

  constructor(public message: string, public title: string, public eventType: EventType) { }

}
