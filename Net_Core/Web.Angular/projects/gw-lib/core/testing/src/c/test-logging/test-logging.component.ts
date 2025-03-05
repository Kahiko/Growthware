import { Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { IModalOptions, ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { ILogOptions, LogDestination, LogOptions, LoggingService } from '@growthware/core/logging';

@Component({
	selector: 'gw-core-test-logging',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		// Angular Material
		MatButtonModule,
		MatCheckboxModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
		MatListModule,
		MatSelectModule,
		MatTabsModule,
	],
	templateUrl: './test-logging.component.html',
	styleUrl: './test-logging.component.scss'
})
export class TestLoggingComponent implements OnInit {

	private _FormBuilder = inject(FormBuilder);
	private _HelpOptions: IModalOptions = new ModalOptions('help', 'Help', '', 1);
	@ViewChild('File') private _HelpFile!: TemplateRef<unknown>;
	@ViewChild('Console') private _HelpConsole!: TemplateRef<unknown>;
	@ViewChild('DB') private _HelpDB!: TemplateRef<unknown>;
	@ViewChild('Toast') private _HelpToast!: TemplateRef<unknown>;
	private _LoggingSvc = inject(LoggingService);
	private _ModalSvc = inject(ModalService);

	theForm: FormGroup = this._FormBuilder.group({});
	validDestinations = [
		{ id: 0, name: 'File' },    // Used by the API
		{ id: 1, name: 'Console' }, // Used by the Fronend
		{ id: 2, name: 'DB' },      // Used by the API
		{ id: 3, name: 'Toast' },   // Used by the Fronend
	];

	validLogLevels = [
		{ id: 0, name: 'Debug' },
		{ id: 1, name: 'Error' },
		{ id: 2, name: 'Fatal' },
		{ id: 3, name: 'Info' },
		{ id: 4, name: 'Warn' },
		{ id: 5, name: 'Success' },
		{ id: 6, name: 'Trace' },
	];

	ngOnInit(): void {
		this.createForm();
	}

	createForm(): void {
		this.theForm = this._FormBuilder.group({
			msg: ['Just a message to test with', Validators.required],
			selectedLogLevel: [0, Validators.required],
			title: ['My Title', Validators.required],
			componentName: ['componentName', Validators.required],
			className: ['className', Validators.required],
			methodName: ['methodName', Validators.required],
			account: ['account', Validators.required],
		});
		this.validDestinations.forEach((item) => {
			if (item.name.toLocaleLowerCase() !== 'toast') {
				this.theForm.addControl(item.name, new FormControl<boolean>(false));
			} else {
				this.theForm.addControl(item.name, new FormControl<boolean>(true));
			}
		});
		// console.log('TestLoggingComponent.createForm: theForm ', this.theForm);
	}

	get controls() {
		return this.theForm.controls;
	}

	onHelp(controleName: string): void {
		switch (controleName) {
			case 'File':
				// this._HelpOptions.windowSize = 1;
				this._HelpOptions.windowSize = new WindowSize(250, 600);
				this._HelpOptions.contentPayLoad = this._HelpFile;
				break;
			case 'Console':
				this._HelpOptions.windowSize = 1;
				this._HelpOptions.contentPayLoad = this._HelpConsole;
				break;
			case 'DB':
				this._HelpOptions.windowSize = 1;
				this._HelpOptions.contentPayLoad = this._HelpDB;
				break;
			case 'Toast':
				this._HelpOptions.windowSize = new WindowSize(250, 600);
				this._HelpOptions.contentPayLoad = this._HelpToast;
				break;
			default:
				break;
		}
		this._ModalSvc.open(this._HelpOptions);
	}

	onSubmit(): void {
		// 
		const mLogDestination: LogDestination[] = [];
		this.validDestinations.forEach((item) => {
			const mCheckbox = this.controls[item.name].value;
			if (mCheckbox && mCheckbox === true) {
				mLogDestination.push(item.id);
			}
		});
		// Create the Object to send to the API
		const mLogOptions: ILogOptions = new LogOptions(
			this.controls['msg'].getRawValue(),
			this.controls['selectedLogLevel'].getRawValue(),
			mLogDestination,
			this.controls['componentName'].getRawValue(),
			this.controls['className'].getRawValue(),
			this.controls['methodName'].getRawValue(),
			this.controls['account'].getRawValue(),
			this.controls['title'].getRawValue()
		);
		this._LoggingSvc.log(mLogOptions);
	}

}
