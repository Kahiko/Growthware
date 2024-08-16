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
	alternatingRow_BackColor: string;
	back_Color: string;
	color_Scheme: string;
	head_Color: string;
	header_ForeColor: string;
	left_Color: string;
	row_BackColor: string;
	sub_Head_Color: string;
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
		{ alternatingRow_BackColor: '#6699cc'	,back_Color: '#ffffff'	,color_Scheme: 'Blue'	,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,left_Color: '#eeeeee'	,row_BackColor: '#b6cbeb'	, sub_Head_Color: '#b6cbeb' },
		{ alternatingRow_BackColor: '#c5e095'	,back_Color: '#ffffff'	,color_Scheme: 'Green'	,head_Color: '#808577'	,header_ForeColor: 'White'	,left_Color: '#eeeeee'	,row_BackColor: '#879966'	, sub_Head_Color: '#879966' },
		{ alternatingRow_BackColor: '#f8e094'	,back_Color: '#ffffff'	,color_Scheme: 'Yellow'	,head_Color: '#CF9C00'	,header_ForeColor: 'Black'	,left_Color: '#f8e094'	,row_BackColor: '#f8bc03'	, sub_Head_Color: '#f8bc03' },
		{ alternatingRow_BackColor: '#91619b'	,back_Color: '#ffffff'	,color_Scheme: 'Purple'	,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,left_Color: '#eeeeee'	,row_BackColor: '#be9cc5'	, sub_Head_Color: '#be9cc5' },
		{ alternatingRow_BackColor: '#A72A49'	,back_Color: '#ffffff'	,color_Scheme: 'Red'    ,head_Color: '#BA706A'	,header_ForeColor: 'White'	,left_Color: '#eeeeee'	,row_BackColor: '#DE8587'	, sub_Head_Color: '#df867f' }
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
		this.clientChoices = this._AccountSvc.clientChoices;
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
		if(mSelectedColorScheme) {
			this.clientChoices.alternatingRowBackColor = mSelectedColorScheme.alternatingRow_BackColor;
			this.clientChoices.backColor = mSelectedColorScheme.back_Color;
			this.clientChoices.colorScheme = mSelectedColorScheme.color_Scheme;
			this.clientChoices.headColor = mSelectedColorScheme.head_Color;
			this.clientChoices.headerForeColor = mSelectedColorScheme.header_ForeColor;
			this.clientChoices.leftColor = mSelectedColorScheme.left_Color;
			this.clientChoices.rowBackColor = mSelectedColorScheme.row_BackColor;
			this.clientChoices.subHeadColor = mSelectedColorScheme.sub_Head_Color;
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
		if(!this._GWCommon.isNullOrUndefined(this.clientChoices)) {
			this.selectedAction = 'home';
			if(!this._GWCommon.isNullOrEmpty(this.clientChoices.action)) {
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
