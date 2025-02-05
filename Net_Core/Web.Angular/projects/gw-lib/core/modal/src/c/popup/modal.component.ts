import { Component, OnDestroy, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
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
export class ModalComponent implements OnDestroy, OnInit {

  private _IsResizing: boolean = false;

  @ViewChild('modalDiv') modalElement!: ElementRef; // Reference to the modal element
  @ViewChild('bottomRightHandle') bottomRightHandle!: ElementRef; // Reference to the bottom right handle
  @ViewChild('rightHandle') rightHandle!: ElementRef; // Reference to the right handle
  @ViewChild('bottomHandle') bottomHandle!: ElementRef; // Reference to the bottom handle

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

  private currentDirection: string = ''; // Property to store current resize direction

  constructor() { }

  ngOnInit() {
    // do nothing atm
  }

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
    this.width = mWindowSize.pxWidth;
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
        event.target === this.bottomRightHandle.nativeElement ||
        event.target === this.rightHandle.nativeElement ||
        event.target === this.bottomHandle.nativeElement
      ) {
        event.preventDefault();
        event.stopPropagation(); // Prevent drag from firing
        this._IsResizing = true; // Set resizing flag
        this.currentDirection = this.getResizeDirection(event.target); // Set the current direction
        document.addEventListener('mousemove', this.resizeModal.bind(this)); // No direction passed here
        document.addEventListener('mouseup', this.stopResize.bind(this));
      }
    }
  }

  getResizeDirection(target: EventTarget) {
    if (target === this.bottomRightHandle.nativeElement) return 'bottom-right';
    if (target === this.rightHandle.nativeElement) return 'right';
    if (target === this.bottomHandle.nativeElement) return 'bottom';
    return '';
  }

  resizeModal(event: MouseEvent) {
    const modalRect = this.modalElement.nativeElement.getBoundingClientRect(); // Get current dimensions and position of the modal

    switch (this.currentDirection) { // Use the stored direction
      case 'left':
        const newWidthLeft = modalRect.right - event.clientX; // Calculate new width
        this.width = Math.max(newWidthLeft, 100); // Set minimum width
        this.modalElement.nativeElement.style.left = `${event.clientX}px`; // Adjust position
        break;
      case 'right':
        const newWidthRight = event.clientX - modalRect.left; // Calculate new width
        this.width = Math.max(newWidthRight, 100); // Set minimum width
        break;
      case 'top':
        const newHeightTop = modalRect.bottom - event.clientY; // Calculate new height
        this.height = Math.max(newHeightTop, 100); // Set minimum height
        this.modalElement.nativeElement.style.top = `${event.clientY}px`; // Adjust position
        break;
      case 'bottom':
        const newHeightBottom = event.clientY - modalRect.top; // Calculate new height
        this.height = Math.max(newHeightBottom, 100); // Set minimum height
        break;
      case 'bottom-right':
        const newWidthBR = event.clientX - modalRect.left; // Calculate new width
        const newHeightBR = event.clientY - modalRect.top; // Calculate new height
        this.width = Math.max(newWidthBR, 100); // Set minimum width
        this.height = Math.max(newHeightBR, 100); // Set minimum height
        break;
    }
  }

  stopResize() {
    this._IsResizing = false; // Reset resizing flag
    document.removeEventListener('mousemove', this.resizeModal.bind(this));
    document.removeEventListener('mouseup', this.stopResize.bind(this));
    this.currentDirection = ''; // Reset the direction
  }

  ngOnDestroy(): void {
    // do nothing atm
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