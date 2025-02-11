import { Component, OnDestroy, ViewChild, ElementRef, HostListener, Renderer2 } from '@angular/core';
// Angular Material cdk
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Interfaces / Common Code
import { CallbackButton, ICallbackButton } from '@growthware/common/interfaces';
import { IModalOptions } from '../../modal-options.model';
import { IWindowSize, WindowSize } from '../../window-size.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'gw-core-modal',
  standalone: true,
  imports: [
    CdkDrag,
    CdkDragHandle,
    CommonModule, 
    MatButtonModule, 
    MatIconModule
  ],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnDestroy {

  @ViewChild('modalDiv') _ModalElement!: ElementRef; // Reference to the modal element
  @ViewChild('bottomRightHandle') _BottomRightHandle!: ElementRef; // Reference to the bottom right handle
  @ViewChild('rightHandle') _RightHandle!: ElementRef; // Reference to the right handle
  @ViewChild('bottomHandle') _BottomHandle!: ElementRef; // Reference to the bottom handle

  public isModalBodyScrollable: boolean = false;
  public header: string = '';
  public height: number = 0; // Default height
  public width: number = 0; // Default width
  public modalId: string = '';
  public showFooter: boolean = false;
  public showCloseBtn: boolean = false;
  public showOkBtn: boolean = false;
  public returnData: unknown;

  public cancelBtn: ICallbackButton = new CallbackButton('Cancel', this.modalId + '_CancelBtn', this.modalId + '_CancelBtn');
  public closeBtn: ICallbackButton = new CallbackButton('Close', this.modalId + '_CloseBtn', this.modalId + '_CloseBtn', true);
  public okBtn: ICallbackButton = new CallbackButton('OK', this.modalId + '_OkBtn', this.modalId + '_OkBtn', true);

  public cancelCallBackMethod?: (arg?: unknown) => void;
  public closeCallBackMethod?: (arg?: unknown) => void;
  public oKCallBackMethod?: (arg?: unknown) => void;

  private _BoundResizeModal = this.resizeModal.bind(this); // class properties to maintain consistent function references
  private _BoundStopResize = this.stopResize.bind(this);   // class properties to maintain consistent function references
  private _CurrentDirection: string = ''; // Property to store current resize direction
  private _RemoveMouseMoveListener: (() => void) | undefined;
  private _RemoveMouseUpListener: (() => void) | undefined;
  private _OriginalHeight: number = 0;
  private _OriginalWidth: number = 0;

  constructor(
    private _Renderer: Renderer2
  ) { }

  private getWindowSize(options: IModalOptions): IWindowSize {
    /**
     * 3 - ExtraLarge = 95,
     * 2 - Large = 60,
     * 0 - Normal = 40,
     * 1 - Small = 20,
     * Custom = -1,
     */

    let mPercentage = .40;
    const mRetVal = new WindowSize(0, 0);
    let mWindowSize!: IWindowSize;
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (options.windowSize) {
      case 0:
        mRetVal.pxHeight = window.innerHeight * (mPercentage -.05);
        mRetVal.pxWidth = window.innerWidth * mPercentage;
        break;
      case 1:
        mPercentage = .20;
        mRetVal.pxHeight = window.innerHeight * (mPercentage -.05);
        mRetVal.pxWidth = window.innerWidth * mPercentage;
        break;
      case 2:
        mPercentage = .60;
        mRetVal.pxHeight = window.innerHeight * (mPercentage -.05);
        mRetVal.pxWidth = window.innerWidth * mPercentage;
        break;
      case 3:
        mPercentage = .95;
        mRetVal.pxHeight = window.innerHeight * (mPercentage -.05);
        mRetVal.pxWidth = window.innerWidth * mPercentage;
        break;
      default:
        mWindowSize = options.windowSize as IWindowSize;
        mRetVal.pxHeight = mWindowSize.pxHeight;
        mRetVal.pxWidth = mWindowSize.pxWidth;
        break;
    }
    return mRetVal;
  }

  public setUp(options: IModalOptions): void {
    const mWindowSize: IWindowSize = this.getWindowSize(options);
    this.header = options.headerText;
    this.height = mWindowSize.pxHeight;
    this._OriginalHeight = mWindowSize.pxHeight;
    this.width = mWindowSize.pxWidth;
    this._OriginalWidth = mWindowSize.pxWidth;
    this.modalId = options.modalId;

    // set the buttons so we can use the id, name, class, text and visible properties in the modal component
    this.cancelBtn = options.buttons.cancelButton;
    this.closeBtn = options.buttons.closeButton;
    this.okBtn = options.buttons.okButton;
    if(this.closeBtn.visible || this.okBtn.visible) {
      this.showFooter = true;
    }
  }

  @HostListener('mousedown', ['$event'])
  onMouseDown(event: MouseEvent) {
    if (event && event.target) {
      if (
        event.target === this._BottomRightHandle.nativeElement ||
        event.target === this._RightHandle.nativeElement ||
        event.target === this._BottomHandle.nativeElement
      ) {
        event.preventDefault();
        event.stopPropagation(); // Prevent drag from firing
        this._CurrentDirection = this.getResizeDirection(event.target); // Set the current direction
        this._RemoveMouseMoveListener = this._Renderer.listen(document, 'mousemove', (moveEvent: MouseEvent) => {
          this.resizeModal(moveEvent);
        });
        
        this._RemoveMouseUpListener = this._Renderer.listen(document, 'mouseup', () => {
          this.stopResize();
        });
      }
    }
  }

  getResizeDirection(target: EventTarget) {
    if (target === this._BottomRightHandle.nativeElement) return 'bottom-right';
    if (target === this._RightHandle.nativeElement) return 'right';
    if (target === this._BottomHandle.nativeElement) return 'bottom';
    return '';
  }

  removeListeners() {
    if (this._RemoveMouseMoveListener) {
      this._RemoveMouseMoveListener();
      this._RemoveMouseMoveListener = undefined;
    }
    if (this._RemoveMouseUpListener) {
      this._RemoveMouseUpListener();
      this._RemoveMouseUpListener = undefined;
    }
  }

  resizeModal(event: MouseEvent) {
    const mModalRect = this._ModalElement.nativeElement.getBoundingClientRect(); // Get current dimensions and position of the modal

    switch (this._CurrentDirection) { // Use the stored direction
      case 'left':
        const newWidthLeft = mModalRect.right - event.clientX; // Calculate new width
        this.width = Math.max(newWidthLeft, 100); // Set minimum width
        this._ModalElement.nativeElement.style.left = `${event.clientX}px`; // Adjust position
        break;
      case 'right':
        const newWidthRight = event.clientX - mModalRect.left; // Calculate new width
        this.width = Math.max(newWidthRight, 100); // Set minimum width
        break;
      case 'top':
        const newHeightTop = mModalRect.bottom - event.clientY; // Calculate new height
        this.height = Math.max(newHeightTop, 100); // Set minimum height
        this._ModalElement.nativeElement.style.top = `${event.clientY}px`; // Adjust position
        break;
      case 'bottom':
        const newHeightBottom = event.clientY - mModalRect.top; // Calculate new height
        this.height = Math.max(newHeightBottom, 100); // Set minimum height
        break;
      case 'bottom-right':
        const newWidthBR = event.clientX - mModalRect.left; // Calculate new width
        const newHeightBR = event.clientY - mModalRect.top; // Calculate new height
        this.width = Math.max(newWidthBR, 100); // Set minimum width
        this.height = Math.max(newHeightBR, 100); // Set minimum height
        break;
    }
    
    // Check if modal body needs scrollbars
    const bodyElement = this._ModalElement.nativeElement.querySelector('.modal-body__div');
    if (bodyElement) {
      const contentHeight = bodyElement.scrollHeight;
      const contentWidth = bodyElement.scrollWidth;

      // Set scrollable state based on current size compared to original
      this.isModalBodyScrollable = 
        (this.height < this._OriginalHeight && contentHeight > this.height) || 
        (this.width < this._OriginalWidth && contentWidth > this.width);
    }
  }

  stopResize() {
    this._CurrentDirection = ''; // Reset the direction
    this.removeListeners();
  }

  ngOnDestroy(): void {
    // Clean up event listeners is a precautionary measure
    // in case this.stopResize() was not called for some reason (better to avoid memory leaks!)
    this.removeListeners();
  }

  onCancel(): void {
    if (this.cancelCallBackMethod) {
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