import { ICallbackButton } from "@Growthware/Lib/src/lib/models";

export interface IWindowSize {
  "pxHeight": number;
  "pxWidth": number;
}

export interface IModalOptions {
  "modalId": string;
  "headerText": string;
  "windowSize": number | IWindowSize;
  "contentPayLoad": any;
  "buttons"?: ICallbackButton[]
}

export class ModalOptions implements IModalOptions {
  public buttons: ICallbackButton[];

  constructor(
    public modalId: string,
    public headerText: string,
    public contentPayLoad: any,
    public windowSize: number | IWindowSize = 0,
  ) {}
}
