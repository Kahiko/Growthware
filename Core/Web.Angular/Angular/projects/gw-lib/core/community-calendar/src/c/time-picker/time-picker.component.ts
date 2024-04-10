import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// import { DatePipe } from '@angular/common';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
// Library
import { GWCommon } from '@growthware/common/services';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
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
		MatButtonModule,
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
	@Output() timeRangeSelected = new EventEmitter<{ startDate: Date, endDate: Date }>();

	get controls() {
		return this.frmProfile.controls;
	}

	constructor(
		private _GWCommon: GWCommon,
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
		this.startDate = new Date(this.startDate);
		this.endDate = new Date(this.endDate);
		const mStartTime = this.startDate.toTimeString().slice(0, 5);
		const mEndTime = this.endDate.toTimeString().slice(0, 5);
		this.frmProfile = this._FormBuilder.group({
			startTime: [mStartTime, [Validators.required]],
			endTime: [mEndTime, [Validators.required]],
		});
	}

	emitTimeRange(): void {
		const mStartTime = this.controls['startTime'].getRawValue();
		const mEndTime = this.controls['endTime'].getRawValue();
		console.log('emitTimeRange - timezoneOffset_UtcOffset', this._GWCommon.timezoneOffset_UtcOffset(new Date().getTimezoneOffset()));
		this.startDate.setHours(mStartTime.split(':')[0]);
		this.startDate.setMinutes(mStartTime.split(':')[1]);
		this.endDate.setHours(mEndTime.split(':')[0]);
		this.endDate.setMinutes(mEndTime.split(':')[1]);
		this.timeRangeSelected.emit({
			startDate: new Date(this.startDate),
			endDate: new Date(this.endDate)
		});
	}

	selectText(event: FocusEvent): void {
		if (event !== null && event.target !== null) {
			const mFleInput = event.target as HTMLInputElement;
			mFleInput.select();
		}
	}

	setToCurrentTime(elementId: string): void {
		const mNow = new Date();
		const mHour = mNow.getHours().toString().padStart(2, '0');
		const mMinute = mNow.getMinutes().toString().padStart(2, '0');
		const mTimeString = `${mHour}:${mMinute}`;
		this.controls[elementId].setValue(mTimeString);
		this.timeChanged$.next(0);
	}
}
