import { Component, ElementRef, input, OnInit, output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { fromEvent, take } from 'rxjs';
// Feature
import { EventType } from '../../event-type.enum';

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
	// @Output() disposeEvent = new EventEmitter();
	disposeEvent = output();

	@ViewChild('toastElement', { static: true })
	toastEl!: ElementRef;

	//   @Input() type!: EventType;
	type = input.required<EventType>();

	// @Input() dateTime: string = new Date().toLocaleString();
	dateTime = input<string>(new Date().toLocaleString());

	// @Input() title!: string;
	title = input.required<string>();

	// @Input() message!: string;
	message = input.required<string>();

	public typeClass: string = '';

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
		this.show();
	}

	private show() {
		if (this.type() !== EventType.Error) {
			setTimeout(() => {
				this.hide();
			}, 3000);
		}
		fromEvent(this.toastEl.nativeElement, 'hidden.bs.toast')
			.pipe(take(1))
			.subscribe(() => this.hide());
	}

	hide() {
		this.disposeEvent.emit();
	}
}
