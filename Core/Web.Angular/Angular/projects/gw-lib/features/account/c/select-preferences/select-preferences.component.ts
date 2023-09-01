import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/common-code';
// Feature
import { AccountService } from '../../account.service';
import { ClientChoices, IClientChoices } from '../../client-choices.model';
import { ISelectedableAction } from '../../selectedable-action.model';

@Component({
  selector: 'gw-lib-select-preferences',
  templateUrl: './select-preferences.component.html',
  styleUrls: ['./select-preferences.component.scss']
})
export class SelectPreferencesComponent implements OnDestroy, OnInit {
  private _Subscription: Subscription = new Subscription();

  clientChoices: IClientChoices = new ClientChoices();
  frmSelectPreferences!: FormGroup;
  selectedColorScheme!: string;
  selectedLink!: string;

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
    // console.log('clientChoices', this.clientChoices);
    this._AccountSvc.getSelectableActions().then((response: ISelectedableAction[]) => {
      this.validLinks = response;
      this.populateForm();
    });
  }

  get controls() {
    return this.frmSelectPreferences.controls;
  }
  
  onSubmit(form: FormGroup): void {
    // nothing atm
  }

  private populateForm(): void {
    if(!this._GWCommon.isNullOrUndefined(this.clientChoices)) {
      this.selectedLink = 'home';
      if(!this._GWCommon.isNullOrEmpty(this.clientChoices.favoriteAction)) {
        this.selectedLink = this.clientChoices.favoriteAction;
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

  onRecordsChanged(element: any): void {
    // nothing atm
  }

}
