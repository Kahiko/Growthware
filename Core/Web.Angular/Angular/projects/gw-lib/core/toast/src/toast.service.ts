import { Injectable } from '@angular/core';
import { distinct, Observable, Subject } from 'rxjs';
import { EventType } from './event-type.enum';
import { ToastMessage } from './toast-message.model';

@Injectable({
	providedIn: 'root',
})
export class ToastService {
	// https://betterprogramming.pub/how-to-create-a-toast-service-using-angular-13-and-bootstrap-5-494e5c66627
	// https://www.tutorialrepublic.com/codelab.php?topic=bootstrap&file=toast-with-different-color-schemes

	private _toastMessages = new Subject<ToastMessage>();

	public toastEvents$: Observable<ToastMessage>;

	constructor() {
		this.toastEvents$ = this._toastMessages.asObservable();
	}

	/**
   * Show success toast notification.
   * @param {ToastMessage} toastMessage
   * @memberof ToastService
   */
	showToast(toastMessage: ToastMessage): void {
		this._toastMessages
			.pipe(distinct(({ message }) => message));
		this._toastMessages.next(toastMessage);
	}

	/**
   * Show success toast notification.
   * @param title Toast title
   * @param message Toast message
   */
	showSuccessToast(title: string, message: string) {
		this.showToast(new ToastMessage(message, title, EventType.Success));
	}

	/**
   * Show info toast notification.
   * @param title Toast title
   * @param message Toast message
   */
	showInfoToast(title: string, message: string) {
		this.showToast(new ToastMessage(message, title, EventType.Info));
	}

	/**
   * Show warning toast notification.
   * @param title Toast title
   * @param message Toast message
   */
	showWarningToast(title: string, message: string) {
		this.showToast(new ToastMessage(message, title, EventType.Warning));
	}

	/**
   * Show error toast notification.
   * @param title Toast title
   * @param message Toast message
   */
	showErrorToast(title: string, message: string) {
		this.showToast(new ToastMessage(message, title, EventType.Error));
	}
}
