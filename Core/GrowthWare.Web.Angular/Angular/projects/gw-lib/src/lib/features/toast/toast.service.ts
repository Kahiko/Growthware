import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { EventType } from './event-type.enum';
import { ToastMessage } from './toast-message.model';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  toastEvents: Observable<ToastMessage>;
  private _toastMessages = new Subject<ToastMessage>();

  constructor() {
    this.toastEvents = this._toastMessages.asObservable();
  }

  /**
   * Show success toast notification.
   * @param {ToastMessage} toastMessage
   * @memberof ToastService
   */
  showToast(toastMessage: ToastMessage): void {
    this._toastMessages.next(toastMessage);
  }

  /**
   * Show success toast notification.
   * @param title Toast title
   * @param message Toast message
   */
  showSuccessToast(title: string, message: string) {
    this.showToast({message,title,eventType: EventType.Success});
  }

  /**
   * Show info toast notification.
   * @param title Toast title
   * @param message Toast message
   */
  showInfoToast(title: string, message: string) {
    this.showToast({message,title,eventType: EventType.Info});
  }

  /**
   * Show warning toast notification.
   * @param title Toast title
   * @param message Toast message
   */
  showWarningToast(title: string, message: string) {
    this.showToast({message,title,eventType: EventType.Warning});
  }

  /**
   * Show error toast notification.
   * @param title Toast title
   * @param message Toast message
   */
  showErrorToast(title: string, message: string) {
    this.showToast({message,title,eventType: EventType.Error});
  }
}
