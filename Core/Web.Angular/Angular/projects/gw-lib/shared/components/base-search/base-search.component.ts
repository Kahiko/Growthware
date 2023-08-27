import { Component, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { DynamicTableComponent, DynamicTableService, DynamicTableBtnMethods } from '@Growthware/features/dynamic-table';
import { DataService } from '@Growthware/shared/services';
import { SearchService, SearchCriteriaNVP } from '@Growthware/features/search';
import { INameValuePare } from '@Growthware/shared/models';
import { ModalService, IModalOptions, ModalOptions, WindowSize } from '@Growthware/features/modal';

@Component({
  selector: 'gw-lib-base-search',
  template: ``,
  styles: [
  ]
})
export abstract class BaseSearchComponent implements OnDestroy {

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  protected _TheApi: string = '';
  protected _TheComponent: any = {};
  protected _TheFeatureName: string = 'Not set';
  protected _TheWindowSize = new WindowSize(450,900);

  protected _TheService: any = {};
  protected _DataSvc!: DataService;
  protected _DynamicTableSvc!: DynamicTableService;
  protected _ModalSvc!: ModalService;
  protected _SearchSvc!: SearchService;

  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;

  public configurationName = '';
  public results: any;

  protected ngAfterViewInit(): void {
    // Setup the dynamic table button methods
    const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
    mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => { this.onBtnTopRight(); }
    this.dynamicTable.setButtonMethods(mDynamicTableBtnMethods);
    // setup the dynamic table "single" and "double" row clicks
    this.dynamicTable.rowClickBackMethod = (rowNumber: number) => { this.onRowClick(rowNumber); };
    this.dynamicTable.rowDoubleClickBackMethod = (rowNumber: number) => { this.onRowDoubleClick(rowNumber); };    
  }

  ngOnDestroy(): void {
    this._SearchCriteriaChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    this._SearchCriteriaChangedSub = this._SearchSvc.searchCriteriaChanged.subscribe((criteria: INameValuePare) => {
      if(criteria.name.trim().toLowerCase() === this.configurationName.trim().toLowerCase()) {
        this._SearchSvc.getResults(this._TheApi, criteria).then((results) => {
          this._DataSvc.notifySearchDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
        }).catch((error) => {
          console.log(error);
        });
      }
    });
    // Get the initial SearchCriteriaNVP from the service
    const mAccountTableConfig = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
    let mResults: SearchCriteriaNVP = this._DynamicTableSvc.getSearchCriteriaFromConfig(this.configurationName, mAccountTableConfig);
    // Set the search criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
  }

  private onBtnTopRight () {
    this._TheService.editReason = 'NewProfile'
    const mModalOptions: IModalOptions = new ModalOptions(this._TheService.addModalId, 'Add ' + this._TheFeatureName, this._TheComponent, this._TheWindowSize);
    if(this._ModalSvc) {
      this._ModalSvc.open(mModalOptions);
    }
  }

  private onRowClick (rowNumber: number): void {
    // Do nothing ATM just leaving as an example
    // const mMessage = 'hi from SearchAccountsComponent.onRowClick row "' + rowNumber + '" was clicked';
    // this._LoggingSvc.toast(mMessage, 'onRowClick', LogLevel.Info);
  }

  private onRowDoubleClick (rowNumber: number): void {
    const mDataRow: any = this.dynamicTable.getRowData(rowNumber);
    this._TheService.editAccount = mDataRow.Account;
    this._TheService.editReason = 'EditAccount';
    const mModalOptions: IModalOptions = new ModalOptions(this._TheService.editModalId, 'Edit ' + this._TheFeatureName, this._TheComponent, this._TheWindowSize);
    if(this._ModalSvc) {
      this._ModalSvc.open(mModalOptions);
    }
  }
}
