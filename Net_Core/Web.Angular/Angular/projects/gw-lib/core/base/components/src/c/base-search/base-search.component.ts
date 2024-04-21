import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { BaseService } from '@growthware/core/base/services';
import { DynamicTableComponent, DynamicTableService, DynamicTableBtnMethods } from '@growthware/core/dynamic-table';
import { DataService } from '@growthware/common/services';
import { SearchService, ISearchCriteriaNVP } from '@growthware/core/search';
import { ModalService, IModalOptions, ModalOptions, WindowSize } from '@growthware/core/modal';

@Component({
	selector: 'gw-core-base-search',
	template: '',
	styles: [
	]
})
export abstract class BaseSearchComponent implements AfterViewInit, OnDestroy, OnInit {

	protected _Subscription: Subscription = new Subscription();

	protected _TheApi: string = '';
	protected _TheComponent: object = {};
	protected _TheFeatureName: string = 'Not set';
	protected _TheWindowSize = new WindowSize(450,900);

	protected _TheService!: BaseService;
	protected _DataSvc!: DataService;
	protected _DynamicTableSvc!: DynamicTableService;
	protected _ModalSvc!: ModalService;
	protected _SearchSvc!: SearchService;

  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;

  public configurationName = '';
  public results: unknown;

  ngAfterViewInit(): void {
  	// Setup the dynamic table button methods
  	const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
  	mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => { this.onBtnTopRight(); };
  	this.dynamicTable.setButtonMethods(mDynamicTableBtnMethods);
  	// setup the dynamic table "single" and "double" row clicks
  	this.dynamicTable.rowClickBackMethod = (rowNumber: number) => { this.onRowClick(rowNumber); };
  	this.dynamicTable.rowDoubleClickBackMethod = (rowNumber: number) => { this.onRowDoubleClick(rowNumber); };    
  }

  ngOnDestroy(): void {
  	this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
  	this._Subscription.add(
  		this._SearchSvc.searchCriteriaChanged$.subscribe((criteria: ISearchCriteriaNVP) => {
  			if(criteria.name.trim().toLowerCase() === this.configurationName.trim().toLowerCase()) {
  				this._SearchSvc.getResults(this._TheApi, criteria).then((results) => {
  					this._SearchSvc.notifySearchDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
  				}).catch((error) => {
  					console.log(error);
  				});
  			}
  		})
  	);

  	// Get the initial SearchCriteriaNVP from the service
  	const mAccountTableConfig = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
  	const mResults: ISearchCriteriaNVP = this._DynamicTableSvc.getSearchCriteriaFromConfig(this.configurationName, mAccountTableConfig);
  	// Set the search criteria to initiate search criteria changed subject
  	this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
  	this.init();
  }

  private onBtnTopRight () {
  	this._TheService.modalReason = 'NewProfile';
  	const mModalOptions: IModalOptions = new ModalOptions(this._TheService.addEditModalId, 'Add ' + this._TheFeatureName, this._TheComponent, this._TheWindowSize);
  	if(this._ModalSvc) {
  		this._ModalSvc.open(mModalOptions);
  	}
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  private onRowClick (rowNumber: number): void {
  	// Do nothing ATM just leaving as an example
  	// const mMessage = 'hi from SearchAccountsComponent.onRowClick row "' + rowNumber + '" was clicked';
  	// this._LoggingSvc.toast(mMessage, 'onRowClick', LogLevel.Info);
  }

  protected onRowDoubleClick (rowNumber: number): void {
  	const mDataRow = JSON.parse(JSON.stringify(this.dynamicTable.getRowData(rowNumber)));
  	console.log('onRowDoubleClick.mDataRow', mDataRow);
  	this._TheService.selectedRow = mDataRow;
  	this._TheService.modalReason = 'EditProfile';
  	const mModalOptions: IModalOptions = new ModalOptions(this._TheService.addEditModalId, 'Edit ' + this._TheFeatureName, this._TheComponent, this._TheWindowSize);
  	if(this._ModalSvc) {
  		this._ModalSvc.open(mModalOptions);
  	}
  }

  protected init(): void {
  	// this.init();
  }

}
