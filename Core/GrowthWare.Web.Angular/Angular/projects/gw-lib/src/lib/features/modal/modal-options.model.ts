import { ICallbackButton } from "@Growthware/Lib/src/lib/models";
import { IWindowSize } from "./window-size.model";

export interface IModalOptions {
  "modalId": string;
  "headerText": string;
  "windowSize": number | IWindowSize;
  "contentPayLoad": any;
  "buttons"?: ICallbackButton | ICallbackButton[]
}

export class ModalOptions implements IModalOptions {
  public buttons?: ICallbackButton[];

  constructor(
    public modalId: string,
    public headerText: string,
    public contentPayLoad: any,
    public windowSize: number | IWindowSize = 0,
  ) { }
}
