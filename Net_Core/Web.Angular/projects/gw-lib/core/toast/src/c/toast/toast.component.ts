import { Component, inject, input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Feature
import { EventType } from '../../event-type.enum';
import { ToastService } from '../../toast.service';
import { IToastMessage } from '../../toast-message.model';

@Component({
	selector: 'gw-core-toast',
	standalone: true,
	imports: [
		CommonModule
	],
	templateUrl: './toast.component.html',
	styleUrls: ['./toast.component.scss']
})
export class ToastComponent implements OnInit {
	private _ToastSvc = inject(ToastService);

	public toastMessage = input.required<IToastMessage>();

	public typeClass: string = 'bg-success text-white';

	ngOnInit() {
		switch (EventType[this.toastMessage().eventType]) {
		case 'Info':
			this.typeClass = 'bg-info text-dark';
			break;
		case 'Warning':
			this.typeClass = 'bg-warning text-dark';
			break;
		case 'Success':
			this.typeClass = 'bg-success text-white';
			break;
		case 'Error':
			this.typeClass = 'bg-danger text-white';
			break;
		default:
			this.typeClass = 'bg-primary text-white';
			break;
		}
	}

	hide() {
		this._ToastSvc.removeToast(this.toastMessage().id);
	}
}
