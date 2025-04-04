import { TemplateRef, Type } from '@angular/core';
import { CallbackButton, ICallbackButton, } from '@growthware/common/interfaces';
import { IWindowSize } from './window-size.model';

export interface IModalOptions {
  initialData?: unknown;
  modalId: string;
  headerText: string;
  windowSize: number | IWindowSize;
  contentPayLoad: string | TemplateRef<unknown> | Type<unknown>;
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
    public contentPayLoad: string | TemplateRef<unknown> | Type<unknown>,
    public windowSize: number | IWindowSize = 0
  ) { }
}
