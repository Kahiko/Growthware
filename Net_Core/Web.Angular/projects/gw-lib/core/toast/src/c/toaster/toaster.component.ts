import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
// Library Imports
import { GWCommon } from '@growthware/common/services';
// Features
import { IToastMessage, ToastMessage } from '../../toast-message.model';
import { ToastService } from '../../toast.service';
import { ToastComponent } from '../toast/toast.component';

@Component({
	selector: 'gw-core-toaster',
	standalone: true,
	imports: [
		FormsModule,
		ToastComponent
	],
	templateUrl: './toaster.component.html',
	styleUrls: ['./toaster.component.scss'],
})
export class ToasterComponent implements OnDestroy, OnInit {
	public currentToasts: Array<IToastMessage> = [];
	private _ToastSub: Subscription = new Subscription();

	constructor(
		private _ChangeDetectorRef: ChangeDetectorRef,
		private _GWCommon: GWCommon,
		private _ToastSvc: ToastService,
	) { }

	ngOnDestroy(): void {
		this._ToastSub.unsubscribe();
	}

	ngOnInit(): void {
		this._ToastSub = this._ToastSvc.toastEvents$
			.subscribe((toast: ToastMessage) => {
				const mIncomingToast: IToastMessage = new ToastMessage(toast.message, toast.title, toast.eventType);
				const mExistingToast: IToastMessage = this.currentToasts.filter(x => x.message.toLocaleLowerCase() == mIncomingToast.message.toLocaleLowerCase())[0];
				// only add new messages to the current toast
				if (this._GWCommon.isNullOrUndefined(mExistingToast)) {
					this.currentToasts.push(mIncomingToast);
					this._ChangeDetectorRef.detectChanges();
				} else {
					// update toast with the same message so the time is updated
					this.currentToasts.forEach((element, index) => {
						if (element.message === mIncomingToast.message) {
							this.currentToasts[index] = mIncomingToast;
						}
					});
				}
			});
	}

	dispose(index: number) {
		this.currentToasts.splice(index, 1);
		this._ChangeDetectorRef.detectChanges();
	}
}
