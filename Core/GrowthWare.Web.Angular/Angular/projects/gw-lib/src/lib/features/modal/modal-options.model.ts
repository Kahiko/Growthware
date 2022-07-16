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
  public buttons =
    { cancelButton: new CallbackButton('cancelBtn', 'Cancel'),
    closeButton: new CallbackButton('closeBtn', 'Close'),
    okButton: new CallbackButton('okBtn', 'OK') };

  constructor(
    public modalId: string,
    public headerText: string,
    public contentPayLoad: any,
    public windowSize: number | IWindowSize = 0
  ) {}
}
