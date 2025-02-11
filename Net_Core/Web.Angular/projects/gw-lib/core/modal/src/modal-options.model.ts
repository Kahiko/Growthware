import { CallbackButton, ICallbackButton, } from '@growthware/common/interfaces';
import { IWindowSize } from './window-size.model';

export interface IModalOptions {
  initialData?: any;
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
	public initialData = undefined;
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
