type CallbackMethod = (arg: any | any[]) => void;

export interface ICallbackButton {
  "id": string,
  "name": string,
  "class": string,
  "text": string,
  "visible": boolean,
  "callbackMethod"?: CallbackMethod,
}

export class CallbackButton implements ICallbackButton {
  public id: string;
  public name: string;
  public class: string;
  public text: string;
  public visible: boolean;
  public callbackMethod?: CallbackMethod;

  constructor(id: string, name: string, className: string = 'btn btn-primary', text: string = 'Add', visible: boolean = false) {
    this.id = id;
    this.name = name;
    this.class = className;
    this.text = text;
    this.visible = visible;
  }
}
