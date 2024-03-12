import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
// Angular Material
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatInputModule } from '@angular/material/input';

@Component({
	selector: 'gw-core-time-picker',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		// Angular Material
		MatDatepickerModule,
		MatFormFieldModule,
		MatGridListModule,
		MatInputModule,
	],
	templateUrl: './time-picker.component.html',
	styleUrl: './time-picker.component.scss'
})
export class TimePickerComponent implements OnDestroy, OnInit {

	private _Subscriptions: Subscription = new Subscription();

	hour: number = 8;
	minute: number = 0;

	timeChanged$: Subject<number> = new Subject<number>();
	frmProfile!: FormGroup;
	numberOfHours: number = 12;

	@Input() endDate!: Date;
	@Input() military: boolean = false;
	@Input() startDate!: Date;
	@Output() timeRangeSelected = new EventEmitter<{ startTime: Date, endTime: Date }>();

	constructor(
		private _FormBuilder: FormBuilder,
	) {
		if (this.military) {
			this.numberOfHours = 24;
		}
	}

	ngOnInit(): void {
		this.createForm();		
		this._Subscriptions.add(
			this.timeChanged$
				.pipe(debounceTime(500))
				.subscribe(() => {
					this.emitTimeRange();
				})
		);
	}

	ngOnDestroy(): void {
		this._Subscriptions.unsubscribe();
	}

	private createForm(): void {
		// convert the @Input dates to Date objects
		this.startDate = new Date(this.startDate);
		this.endDate = new Date(this.endDate);
		const mStartDatePipe: DatePipe = new DatePipe('en-US');
		const mEndDatePipe: DatePipe = new DatePipe('en-US');
		this.frmProfile = this._FormBuilder.group({
			fromHour: [this.startDate.getHours(), [Validators.required]],
			fromMinute: [mStartDatePipe.transform(this.startDate, 'mm'), [Validators.required]],
			toHour: [this.endDate.getHours(), [Validators.required]],
			toMinute: [mEndDatePipe.transform(this.endDate, 'mm'), [Validators.required]],
		});
	}

	emitTimeRange(): void {
		this.startDate.setHours(this.frmProfile.value.fromHour);
		this.startDate.setMinutes(this.frmProfile.value.fromMinute);
		this.endDate.setHours(this.frmProfile.value.toHour);
		this.endDate.setMinutes(this.frmProfile.value.toMinute);
		this.timeRangeSelected.emit({
			startTime: this.startDate,
			endTime: this.endDate
		});
	}
	
	selectText(event: FocusEvent): void {
		if(event !== null && event.target !== null) {
			const mFleInput = event.target as HTMLInputElement;
			mFleInput.select();
		}
	}

}
