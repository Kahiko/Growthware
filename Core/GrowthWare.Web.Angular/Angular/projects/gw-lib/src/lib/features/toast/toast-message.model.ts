import { EventType } from "./event-type.enum";

export interface IToastMessage {
  dateTime: string;
  eventType: EventType;
  message: string;
  title: string;
}

export class ToastMessage implements IToastMessage {
  public dateTime: string = new Date().toLocaleString();
  constructor(public message: string, public title: string, public eventType: EventType) { }

}
