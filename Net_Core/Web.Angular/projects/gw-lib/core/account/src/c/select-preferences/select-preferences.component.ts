import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { GWCommon } from '@growthware/common/services';
import { LogLevel, LoggingService } from '@growthware/core/logging';
// Feature
import { AccountService } from '../../account.service';
import { ClientChoices, IClientChoices } from '../../client-choices.model';
import { ISelectedableAction } from '../../selectedable-action.model';

export interface IColorSchemes {
	color_Scheme: string;
	even_row: string;
	odd_row: string;
	row_font: string;
	background: string;
	header_Color: string;
	// left_Color: string; // not used
	header_font: string;
}

export interface IColorSchemeColumns {
	displayedName: string;
	propertyName: string;
}

@Component({
	selector: 'gw-core-select-preferences',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,

		MatButtonModule,
		MatIconModule,
		MatRadioModule,
		MatSelectModule,
		MatTabsModule,
	],
	templateUrl: './select-preferences.component.html',
	styleUrls: ['./select-preferences.component.scss']
})
export class SelectPreferencesComponent implements OnDestroy, OnInit {
	private _Subscription: Subscription = new Subscription();

	colorSchemeColumns: IColorSchemeColumns[] = [];
	clientChoices: IClientChoices = new ClientChoices();
	frmSelectPreferences!: FormGroup;
	selectedColorScheme!: string;
	selectedAction!: string;

	validColorSchemes: IColorSchemes[] = [
		{ color_Scheme: 'Blue',   even_row: '#6699cc', odd_row: '#b6cbeb', row_font: 'Black', background: '#ffffff', header_Color: '#C7C7C7', header_font: '#b6cbeb' },
		{ color_Scheme: 'Green',  even_row: '#c5e095', odd_row: '#879966', row_font: 'White', background: '#ffffff', header_Color: '#808577', header_font: '#879966' },
		{ color_Scheme: 'Yellow', even_row: '#f8e094', odd_row: '#f8bc03', row_font: 'Black', background: '#ffffff', header_Color: '#CF9C00', header_font: '#f8bc03' },
		{ color_Scheme: 'Purple', even_row: '#91619b', odd_row: '#be9cc5', row_font: 'Black', background: '#ffffff', header_Color: '#C7C7C7', header_font: '#be9cc5' },
		{ color_Scheme: 'Red',    even_row: '#A72A49', odd_row: '#DE8587', row_font: 'White', background: '#ffffff', header_Color: '#BA706A', header_font: '#df867f' }
	];

	validLinks = [
		{ action: 'login', title: 'Login' },
	];

	constructor(
		private _AccountSvc: AccountService,
		private _FormBuilder: FormBuilder,
		private _GWCommon: GWCommon,
		private _LoggingSvc: LoggingService,
	) {
		this.selectedColorScheme = 'Blue';
	}

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngOnInit(): void {
		this.setHeaders();
		this.frmSelectPreferences = new FormGroup({
			recordsPerPage: new FormControl(10),
			selectedColorScheme: new FormControl('Blue'),
		});
		this.clientChoices = this._AccountSvc.clientChoices();
		// console.log('clientChoices', this.clientChoices);
		this._AccountSvc.getSelectableActions().then((response: ISelectedableAction[]) => {
			this.validLinks = response;
			this.populateForm();
		});
	}

	get controls() {
		return this.frmSelectPreferences.controls;
	}

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	getValue(colorSchemes: IColorSchemes, columnName: IColorSchemeColumns): string {
		type ObjectKey = keyof typeof colorSchemes;
		const mKey: ObjectKey = columnName.propertyName as ObjectKey;
		return colorSchemes[mKey];
	}

	onSubmit(): void {
		const mSelectedColor: string = this.controls['selectedColorScheme'].getRawValue();
		const mSelectedColorScheme = this.validColorSchemes.find(item => item.color_Scheme === mSelectedColor);
		if (mSelectedColorScheme) {
			this.clientChoices.alternatingRowBackColor = mSelectedColorScheme.even_row;
			this.clientChoices.backColor = mSelectedColorScheme.background;
			this.clientChoices.colorScheme = mSelectedColorScheme.color_Scheme;
			this.clientChoices.headColor = mSelectedColorScheme.header_Color;
			this.clientChoices.headerForeColor = mSelectedColorScheme.row_font;
			this.clientChoices.leftColor = '#eeeeee'; // mSelectedColorScheme.left_Color;
			this.clientChoices.rowBackColor = mSelectedColorScheme.odd_row;
			this.clientChoices.subHeadColor = mSelectedColorScheme.header_font;
			this.clientChoices.action = this.selectedAction;
			this.clientChoices.recordsPerPage = this.controls['recordsPerPage'].getRawValue();
			this._AccountSvc.saveClientChoices(this.clientChoices).catch(() => {
				this._LoggingSvc.toast('Unable to save your preferences', 'Save Preferences', LogLevel.Error);
			}).then(() => {
				this._LoggingSvc.toast('Your preferences have been saved', 'Save Preferences', LogLevel.Success);
			});
		}
		// console.log('clientChoices', this.clientChoices);
	}

	private populateForm(): void {
		if (!this._GWCommon.isNullOrUndefined(this.clientChoices)) {
			this.selectedAction = 'home';
			if (!this._GWCommon.isNullOrEmpty(this.clientChoices.action)) {
				this.selectedAction = this.clientChoices.action.toLocaleLowerCase();
			}
			this.frmSelectPreferences = this._FormBuilder.group({
				recordsPerPage: [this.clientChoices.recordsPerPage, [Validators.required]],
				selectedColorScheme: [this.clientChoices.colorScheme, [Validators.required]],
			});
		} else {
			this.frmSelectPreferences = this._FormBuilder.group({
				recordsPerPage: [10, [Validators.required]],
				selectedColorScheme: ['Blue', [Validators.required]],
			});
		}
	}

	setHeaders(): void {
		// loop through IColorSchemes
		for (const [key] of Object.entries(this.validColorSchemes[0])) {
			const mWords = key.split('_');
			const mDisplayedName = mWords.map((w) => w.charAt(0).toUpperCase() + w.slice(1)).join(' ');
			const mColorSchemeColumn = {
				displayedName: mDisplayedName,
				propertyName: key
			};
			this.colorSchemeColumns.push(mColorSchemeColumn);
		}
		//   console.log('xx.setHeaders', this.colorSchemeColumns);
	}
}
