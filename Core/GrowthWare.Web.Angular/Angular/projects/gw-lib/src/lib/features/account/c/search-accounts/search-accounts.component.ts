import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent, DynamicTableService } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { SearchService, SearchCriteriaNVP } from '@Growthware/Lib/src/lib/features/search';
import { CallbackButton, DynamicTableBtnMethods, INameValuePare } from '@Growthware/Lib/src/lib/models';
import { LoggingService, LogLevel, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { ModalService, IModalOptions, ModalOptions, ModalSize } from '@Growthware/Lib/src/lib/features/modal';

import { AccountDetailsComponent } from '../account-details/account-details.component';

@Component({
  selector: 'gw-lib-search-accounts',
  templateUrl: './search-accounts.component.html',
  styleUrls: ['./search-accounts.component.scss']
})
export class SearchAccountsComponent implements OnDestroy, OnInit {
  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  public configurationName = 'Accounts';
  public results: any;

  constructor(
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _SearchSvc: SearchService,
  ) { }

  ngAfterViewInit(): void {
    // Setup the dynamic table button methods
    const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
    mDynamicTableBtnMethods.btnTopLeftCallBackMethod = () => { this.onBtnTopLeft(); }
    mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => { this.onBtnTopRight(); }
    mDynamicTableBtnMethods.btnBottomLeftCallBackMethod = () => { this.onBtnBottomLeft(); }
    mDynamicTableBtnMethods.btnBottomRightCallBackMethod = () => { this.onBtnBottomRight(); }
    this.dynamicTable.setButtonMethods(mDynamicTableBtnMethods);
    // setup the dynamic table "single" and "double" row clicks
    this.dynamicTable.rowClickBackMethod = (rowNumber: number) => { this.onRowClick(rowNumber); };
    this.dynamicTable.rowDoubleClickBackMethod = (rowNumber: number) => { this.onRowDoubleClick(rowNumber); };

    const mLogOptions: ILogOptions = new LogOptions('Testing using options');
    mLogOptions.componentName = "Search Account"
    mLogOptions.className = 'SearchAccountsComponent';
    mLogOptions.methodName = 'ngAfterViewInit';
    mLogOptions.level = LogLevel.Error;
    this._LoggingSvc.log(mLogOptions);
    this._LoggingSvc.console('Testing using console method', LogLevel.Info);
    mLogOptions.level = LogLevel.Warn;
    mLogOptions.msg = mLogOptions.msg + ' with level Warn'
    this._LoggingSvc.log(mLogOptions);
  }

  ngOnInit(): void {
    this._SearchCriteriaChangedSub = this._SearchSvc.searchCriteriaChanged.subscribe((criteria: INameValuePare) => {
      if(criteria.name.trim().toLowerCase() === this.configurationName.trim().toLowerCase()) {
        this._SearchSvc.searchAccounts(criteria).then((results) => {
          this._DataSvc.notifyDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
        }).catch((error) => {
          console.log(error);
        });
      }
    });
    // Get the initial SearchCriteriaNVP from the service
    const mAccountTableConfig = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
    let mResults: SearchCriteriaNVP = this._SearchSvc.getSearchCriteriaFromConfig(this.configurationName, mAccountTableConfig);
    // Set the search criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
  }

  ngOnDestroy(): void {
    this._SearchCriteriaChangedSub.unsubscribe();
  }

  private onBtnTopLeft () {
    this._LoggingSvc.toast('hi from SearchAccountsComponent.onBtnTopLeft', 'onBtnTopLeft', LogLevel.Info);
  }

  private onBtnTopRight () {
    // this._LoggingSvc.toast('hi from SearchAccountsComponent.onBtnTopRight', 'onBtnTopRight', LogLevel.Info);
    const mModalOptions: IModalOptions = new ModalOptions('testModal', 'header text', 'hello there', ModalSize.ExtraLarge);
    const mCallbackButton = new CallbackButton('cancel', 'cancel_testModal', 'cancel_testModal', true);
    mCallbackButton.callbackMethod = () => {
      this._ModalSvc.close(mModalOptions.modalId);
    };
    // mModalOptions.buttons.closeButton = JSON.parse(JSON.stringify(mCallbackButton));
    mModalOptions.buttons.closeButton = {...mCallbackButton};
    mCallbackButton.id = 'ok_testModal',
    mCallbackButton.text = "OK"
    // mModalOptions.buttons.okButton = JSON.parse(JSON.stringify(mCallbackButton));
    mModalOptions.buttons.okButton = {...mCallbackButton};

    this._ModalSvc.open(mModalOptions);
  }

  private onBtnBottomLeft () {
    this._LoggingSvc.toast('hi from SearchAccountsComponent.onBtnBottomLeft', 'onBtnBottomLeft', LogLevel.Info);
  }

  private onBtnBottomRight () {
    this._LoggingSvc.toast('hi from SearchAccountsComponent.onBtnBottomRight', 'onBtnBottomRight', LogLevel.Info);
  }

  private onRowClick (rowNumber: number): void {
    const mMessage = 'hi from SearchAccountsComponent.onRowClick row "' + rowNumber + '" was clicked';
    this._LoggingSvc.toast(mMessage, 'onRowClick', LogLevel.Info);
  }

  private onRowDoubleClick (rowNumber: number): void {
    console.log(this.dynamicTable.getRowData(rowNumber));
    const mMessage = 'hi from SearchAccountsComponent.onRowDoubleClick row "' + rowNumber + '" was clicked';
    this._LoggingSvc.toast(mMessage, 'onRowDoubleClick', LogLevel.Info);
  }
}
