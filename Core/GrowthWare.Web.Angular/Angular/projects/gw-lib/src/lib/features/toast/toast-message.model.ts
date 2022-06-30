import { EventType } from "./event-type.enum";

export interface IToastMessage {
  eventType: EventType;
  message: string;
  title: string;
}

export class ToastMessage implements IToastMessage {

  constructor(public message: string, public title: string, public eventType: EventType) { }

}
