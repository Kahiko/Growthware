import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
	selector: 'gw-core-workflow-details',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		// Angular Material
		MatIconModule,
		MatTabsModule,
	],
	templateUrl: './workflow-details.component.html',
	styleUrls: ['./workflow-details.component.scss']
})
export class WorkflowDetailsComponent implements OnInit {

	private _FormBuilder = inject(FormBuilder);

	theForm: FormGroup = this._FormBuilder.group({});

	ngOnInit(): void {
		this.createForm();
	}

	createForm(): void {
		this.theForm = this._FormBuilder.group({

		});
	}

	get controls() {
		return this.theForm.controls;
	}

	onSubmit(form: FormGroup): void {
		console.log('TestLoggingComponent.onSubmit: form ', form);
	}
}
