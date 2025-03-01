import { inject, Injectable, signal } from '@angular/core';
import { EventType } from './event-type.enum';
// Feature
import { IToastMessage, ToastMessage } from './toast-message.model';

@Injectable({
	providedIn: 'root',
})
export class ToastService {

	private _Timer: ReturnType<typeof setTimeout> | undefined;

	public currentToasts = signal<Array<IToastMessage>>([]);

	/**
	 * Starts the timer for refreshing the currentToasts signal.
	 */
	refreshToasts(): void {
		const mRemovableToasts = this.currentToasts().filter((item) => {
			return item.eventType !== EventType.Error
		}).reduce((acc, item) => acc + 1, 0);
		if (mRemovableToasts > 0) {
			const mCurrentDateTime: Date = new Date();
			// console.log('ToastService.autoUpdateToasts.mTestDateTime', mCurrentDateTime);
			this.currentToasts.update((items) => {
				const mRetVal = items.filter((item) => {
					const mItemDateTime: Date = new Date(item.dateTime);
					mItemDateTime.setSeconds(mItemDateTime.getSeconds() + 3);
					return mItemDateTime >= mCurrentDateTime && item.eventType !== EventType.Error;
				});
				return mRetVal;
			});
			// restart the timer
			this._Timer = setTimeout(() => { this.refreshToasts(); }, 3000);
		} else {
			// since there are no more times in the currentToasts signal, stop the timer
			clearTimeout(this._Timer);
			this._Timer = undefined;
		}
	}

	/**
	 * Removes a toast message in the currentToasts signal given the id.
	 * @param id 
	 */
	removeToast(id: string): void {
		this.currentToasts.update((items) => items.filter(item => item.id !== id));
	}

	/**
	 * Adds or updates a toast message in the currentToasts signal.
	 * @param {ToastMessage} toastMessage
	 * @memberof ToastService
	 */
	addOrUpdateToasts(toastMessage: ToastMessage): void {

		const mExistingToast: IToastMessage | undefined = this.currentToasts().find(item => item.message === toastMessage.message);

		if (mExistingToast === undefined) {
			// Added the incomming toast message
			this.currentToasts.update((items) => [...items, toastMessage]);
		} else {
			// update the existing toast message
			this.currentToasts.update((items) => items.map(item => item.message === toastMessage.message ? toastMessage : item));
		}
		this.refreshToasts();
	}

	/**
	 * Show success toast notification.
	 * @param title Toast title
	 * @param message Toast message
	 */
	showSuccessToast(title: string, message: string) {
		this.addOrUpdateToasts(new ToastMessage(message, title, EventType.Success));
	}

	/**
	 * Show info toast notification.
	 * @param title Toast title
	 * @param message Toast message
	 */
	showInfoToast(title: string, message: string) {
		this.addOrUpdateToasts(new ToastMessage(message, title, EventType.Info));
	}

	/**
	 * Show warning toast notification.
	 * @param title Toast title
	 * @param message Toast message
	 */
	showWarningToast(title: string, message: string) {
		this.addOrUpdateToasts(new ToastMessage(message, title, EventType.Warning));
	}

	/**
	 * Show error toast notification.
	 * @param title Toast title
	 * @param message Toast message
	 */
	showErrorToast(title: string, message: string) {
		this.addOrUpdateToasts(new ToastMessage(message, title, EventType.Error));
	}
}
