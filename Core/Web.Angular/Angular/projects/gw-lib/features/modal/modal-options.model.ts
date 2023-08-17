import { CallbackButton, ICallbackButton, } from '@Growthware/shared/models';
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
    cancelButton: new CallbackButton('Cancel', 'cancelBtn', 'cancelBtn', false),
    closeButton: new CallbackButton('Close', 'closeBtn', 'closeBtn', false),
    okButton: new CallbackButton('OK', 'okBtn', 'okBtn', false)
  };

  constructor(
    public modalId: string,
    public headerText: string,
    public contentPayLoad: any,
    public windowSize: number | IWindowSize = 0
  ) {}
}
