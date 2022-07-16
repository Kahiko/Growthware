import { CallbackButton, ICallbackButton, } from '@Growthware/Lib/src/lib/models';
import { IWindowSize } from './window-size.model';

export interface IModalOptions {
  modalId: string;
  headerText: string;
  windowSize: number | IWindowSize;
  contentPayLoad: any;
  buttons: {
    cancelButton: ICallbackButton;
    closeButton: ICallbackButton;
    okButton: ICallbackButton;
  };
}

export class ModalOptions implements IModalOptions {
  public buttons = {
    cancelButton: new CallbackButton('Cancel', 'cancelBtn', 'cancelBtn'),
    closeButton: new CallbackButton('Close', 'closeBtn', 'closeBtn'),
    okButton: new CallbackButton('OK', 'okBtn', 'okBtn')
  };

  constructor(
    public modalId: string,
    public headerText: string,
    public contentPayLoad: any,
    public windowSize: number | IWindowSize = 0
  ) {}
}
