import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
// Library
// Feature

@Component({
	selector: 'gw-core-test-logging',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		// Angular Material
		MatIconModule,
		MatTabsModule,
	],
	templateUrl: './page.component.html',
	styleUrl: './page.component.scss'
})
export class PageComponent implements OnInit {

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
		console.log('PageComponent.onSubmit: form ', form);
	}
}
