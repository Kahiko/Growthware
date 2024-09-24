import { Component, inject, input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Feature
import { EventType } from '../../event-type.enum';
import { ToastService } from '../../toast.service';

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

	public dateTime = input<string>(new Date().toLocaleString());
	public id = input.required<string>();
	public message = input.required<string>();
	public title = input.required<string>();
	public type = input.required<EventType>();
	public typeClass: string = 'bg-success text-white';

	ngOnInit() {
		switch (EventType[this.type()]) {
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
		this._ToastSvc.removeToast(this.id());
	}
}
