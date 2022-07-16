import { Component, OnInit } from '@angular/core';

// Services
import {
  LoggingService,
  LogLevel,
} from '@Growthware/Lib/src/lib/features/logging';

// Interfaces / Common Code
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { CallbackButton, ICallbackButton } from '@Growthware/Lib/src/lib/models';
import { IModalOptions } from '../modal-options.model';
import { IWindowSize, WindowSize } from '../window-size.model';

@Component({
  selector: 'gw-lib-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
  public header: string = '';
  public height: number = 0;
  public modalId: string = '';
  public showFooter: boolean = false;
  public showCloseBtn: boolean = false;
  public showOkBtn: boolean = false;
  public width: number = 0;
  public returnData: any;
  // 'text', 'id', 'name', 'visible', className: string = 'btn btn-primary', color: string = 'primary'
  public cancelBtn: ICallbackButton = new CallbackButton('Cancel', this.modalId + '_CancelBtn', this.modalId + '_CancelBtn');
  public closeBtn: ICallbackButton = new CallbackButton('Close', this.modalId + '_CloseBtn', this.modalId + '_CloseBtn', true);
  public okBtn: ICallbackButton = new CallbackButton('OK', this.modalId + '_OkBtn', this.modalId + '_OkBtn', true);

  public cancelCallBackMethod?: (arg?: any) => void;
  public closeCallBackMethod?: (arg?: any) => void;
  public oKCallBackMethod?: (arg?: any) => void;

  constructor(private _LoggingSvc: LoggingService, private _GWCommon: GWCommon) { }

  private getWindowSize(options: IModalOptions): IWindowSize {
    const mRetVal = new WindowSize(0, 0);
    switch (options.windowSize) {
      case 0:
        mRetVal.pxHeight = 300;
        mRetVal.pxWidth = 400;
        break;
      case 1:
        mRetVal.pxHeight = 100;
        mRetVal.pxWidth = 200;
        break;
      case 2:
        mRetVal.pxHeight = 400;
        mRetVal.pxWidth = 500;
        break;
      case 3:
        mRetVal.pxHeight = 500;
        mRetVal.pxWidth = 600;
        break;
      default:
        const mWindowSize = options.windowSize as IWindowSize;
        mRetVal.pxHeight = mWindowSize.pxHeight;
        mRetVal.pxWidth = mWindowSize.pxWidth;
        break;
    }
    return mRetVal;
  }

  public setUp(options: IModalOptions): void {
    const mWindowSize: IWindowSize = this.getWindowSize(options);
    this.height = mWindowSize.pxHeight;
    this.width = mWindowSize.pxWidth;
    this.modalId = options.modalId;

    // set the buttons so we can use the id, name, class, text and visible properties in the modal component
    this.cancelBtn = options.buttons.cancelButton;
    this.closeBtn = options.buttons.closeButton;
    this.okBtn = options.buttons.okButton;

  }

  private keyDownHandler(e: any): void {
    // console.log(`Key "${e.key}" released [event: keyup]`);
    if (e.key === 'Escape') {
      this.onCancel();
    }
  }

  ngOnInit(): void {
    document.addEventListener('keyup', (e) => {
      this.keyDownHandler(e);
    });
  }

  onCancel(): void {
    if (this.cancelCallBackMethod) {
      document.removeEventListener('keydown', this.keyDownHandler);
      this.cancelCallBackMethod();
    }
  }

  onClose(): void {
    if (this.closeCallBackMethod) {
      this.closeCallBackMethod(this.returnData);
    }
  }

  onOk(): void {
    if (this.oKCallBackMethod) {
      this.oKCallBackMethod(this.returnData);
    }
  }
}
