import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { ClientChoices, IClientChoices } from '@growthware/core/clientchoices';
import { GWCommon } from '@growthware/common/services';
import { LogLevel, LoggingService } from '@growthware/core/logging';
// Feature
import { AccountService } from '../../account.service';
import { ISelectedableAction } from '../../selectedable-action.model';

interface IColorSchemes {
	color_Scheme: string;
	even_row: string;
	even_font: string;
	odd_row: string;
	odd_font: string;
	background: string;
	header_row: string;
	header_font: string;
}

interface IColorSchemeColumns {
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
export class SelectPreferencesComponent implements OnInit {

	colorSchemeColumns: IColorSchemeColumns[] = [];
	clientChoices: IClientChoices = new ClientChoices();
	frmSelectPreferences!: FormGroup;
	selectedColorScheme!: string;
	selectedAction!: string;

	validColorSchemes: IColorSchemes[] = [
		{ color_Scheme: 'Blue', even_row: '#6699cc', even_font: 'White', odd_row: '#b6cbeb', odd_font: 'Black', background: '#ffffff', header_row: '#C7C7C7', header_font: 'Black' },
		{ color_Scheme: 'Green', even_row: '#c5e095', even_font: 'Black', odd_row: '#879966', odd_font: 'White', background: '#ffffff', header_row: '#808577', header_font: 'White' },
		{ color_Scheme: 'Yellow', even_row: '#f8e094', even_font: 'Black', odd_row: '#f8bc03', odd_font: 'Black', background: '#ffffff', header_row: '#CF9C00', header_font: 'Black' },
		{ color_Scheme: 'Purple', even_row: '#91619b', even_font: 'White', odd_row: '#be9cc5', odd_font: 'Black', background: '#ffffff', header_row: '#C7C7C7', header_font: '#91619b' },
		{ color_Scheme: 'Red', even_row: '#A72A49', even_font: 'White', odd_row: '#DE8587', odd_font: 'Black', background: '#ffffff', header_row: '#BA706A', header_font: 'WhiteSmoke' }
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

	getValue(colorSchemes: IColorSchemes, columnName: IColorSchemeColumns): string {
		type ObjectKey = keyof typeof colorSchemes;
		const mKey: ObjectKey = columnName.propertyName as ObjectKey;
		return colorSchemes[mKey];
	}

	onSubmit(): void {
		const mSelectedColor: string = this.controls['selectedColorScheme'].getRawValue();
		const mSelectedColorScheme = this.validColorSchemes.find(item => item.color_Scheme === mSelectedColor);
		if (mSelectedColorScheme) {
			this.populateProfile(mSelectedColorScheme)
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

	private populateProfile(selectedColorScheme: IColorSchemes): void {
		this.clientChoices.colorScheme = selectedColorScheme.color_Scheme;
		this.clientChoices.action = this.selectedAction;
		this.clientChoices.recordsPerPage = this.controls['recordsPerPage'].getRawValue();
		this.clientChoices.evenRow = selectedColorScheme.even_row;
		this.clientChoices.evenFont = selectedColorScheme.even_font;
		this.clientChoices.oddRow = selectedColorScheme.odd_row;
		this.clientChoices.oddFont = selectedColorScheme.odd_font;
		this.clientChoices.background = selectedColorScheme.background;
		this.clientChoices.headerRow = selectedColorScheme.header_row;
		this.clientChoices.headerFont = selectedColorScheme.header_font;
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
