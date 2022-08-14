import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent, DynamicTableService } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { SearchService, SearchCriteriaNVP } from '@Growthware/Lib/src/lib/features/search';
import { DynamicTableBtnMethods, INameValuePare } from '@Growthware/Lib/src/lib/models';
import { LoggingService, LogLevel, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { ModalService, IModalOptions, ModalOptions, ModalSize } from '@Growthware/Lib/src/lib/features/modal';

import { AccountDetailsComponent } from '../account-details/account-details.component';
import { AccountService } from '../../account.service';

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
    private _AccountSvc: AccountService,
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _SearchSvc: SearchService,
  ) { }

  ngAfterViewInit(): void {
    // Setup the dynamic table button methods
    const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
    mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => { this.onBtnTopRight(); }
    this.dynamicTable.setButtonMethods(mDynamicTableBtnMethods);
    // setup the dynamic table "single" and "double" row clicks
    this.dynamicTable.rowClickBackMethod = (rowNumber: number) => { this.onRowClick(rowNumber); };
    this.dynamicTable.rowDoubleClickBackMethod = (rowNumber: number) => { this.onRowDoubleClick(rowNumber); };
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

  private onBtnTopRight () {
    const mModalOptions: IModalOptions = new ModalOptions('addAccount', 'Add Account', AccountDetailsComponent, ModalSize.ExtraLarge);
    this._ModalSvc.open(mModalOptions);
  }

  private onRowClick (rowNumber: number): void {
    // Do nothing ATM just leaving as an example
    // const mMessage = 'hi from SearchAccountsComponent.onRowClick row "' + rowNumber + '" was clicked';
    // this._LoggingSvc.toast(mMessage, 'onRowClick', LogLevel.Info);
  }

  private onRowDoubleClick (rowNumber: number): void {
    const mDataRow: any = this.dynamicTable.getRowData(rowNumber);
    this._AccountSvc.accountId = mDataRow.AccountSeqId;
    const mModalOptions: IModalOptions = new ModalOptions('editAccount', 'Edit Account', AccountDetailsComponent, ModalSize.ExtraLarge);
    this._ModalSvc.open(mModalOptions);
  }
}
