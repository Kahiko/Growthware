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
// Library
import { GWCommon } from '@Growthware/common-code';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
// Feature
import { AccountService } from '../../account.service';
import { ClientChoices, IClientChoices } from '../../client-choices.model';
import { ISelectedableAction } from '../../selectedable-action.model';

@Component({
  selector: 'gw-lib-select-preferences',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    
    MatButtonModule,
    MatIconModule,
    MatRadioModule,
    MatSelectModule,
  ],
  templateUrl: './select-preferences.component.html',
  styleUrls: ['./select-preferences.component.scss']
})
export class SelectPreferencesComponent implements OnDestroy, OnInit {
  private _Subscription: Subscription = new Subscription();

  clientChoices: IClientChoices = new ClientChoices();
  frmSelectPreferences!: FormGroup;
  selectedColorScheme!: string;
  selectedAction!: string;

  validColorSchemes = [
    { color_Scheme: 'Blue'	  ,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,row_BackColor: '#b6cbeb'	,alternatingRow_BackColor: '#6699cc'	,sub_Head_Color: '#b6cbeb'	,back_Color: '#ffffff'	,left_Color: '#eeeeee' },
    { color_Scheme: 'Green'	  ,head_Color: '#808577'	,header_ForeColor: 'White'	,row_BackColor: '#879966'	,alternatingRow_BackColor: '#c5e095'	,sub_Head_Color: '#879966'	,back_Color: '#ffffff'	,left_Color: '#eeeeee' },
    { color_Scheme: 'Yellow'	,head_Color: '#CF9C00'	,header_ForeColor: 'Black'	,row_BackColor: '#f8bc03'	,alternatingRow_BackColor: '#f8e094'	,sub_Head_Color: '#f8bc03'	,back_Color: '#ffffff'	,left_Color: '#f8e094' },
    { color_Scheme: 'Purple'	,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,row_BackColor: '#be9cc5'	,alternatingRow_BackColor: '#91619b'	,sub_Head_Color: '#be9cc5'	,back_Color: '#ffffff'	,left_Color: '#eeeeee' },
    { color_Scheme: 'Red'	    ,head_Color: '#BA706A'	,header_ForeColor: 'White'	,row_BackColor: '#DE8587'	,alternatingRow_BackColor: '#A72A49'	,sub_Head_Color: '#df867f'	,back_Color: '#ffffff'	,left_Color: '#eeeeee' }
  ];

  validLinks = [
    { action: 'login', title: 'Login' },
  ]

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
    this.frmSelectPreferences = new FormGroup({
      recordsPerPage: new FormControl(10),
      selectedColorScheme: new FormControl('Blue'),
    })
    this.clientChoices = this._AccountSvc.clientChoices;
    console.log('clientChoices', this.clientChoices);
    this._AccountSvc.getSelectableActions().then((response: ISelectedableAction[]) => {
      this.validLinks = response;
      this.populateForm();
    });
  }

  get controls() {
    return this.frmSelectPreferences.controls;
  }
  
  onSubmit(form: FormGroup): void {
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
      this._AccountSvc.saveClientChoices(this.clientChoices).catch((error: any) => {
        this._LoggingSvc.toast('Unable to save client choices', 'Save client choices', LogLevel.Error);
      }).then((_) => {
        this._LoggingSvc.toast('Client choices saved', 'Save client choices', LogLevel.Success);
      })
    }
    console.log('clientChoices', this.clientChoices);
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
}
