import { AfterViewInit, Component, effect, inject, OnInit, Type, ViewChild } from '@angular/core';
// Library
import { BaseService } from '@growthware/core/base/services';
import { DynamicTableComponent, DynamicTableService, DynamicTableBtnMethods } from '@growthware/core/dynamic-table';
import { SearchService, ISearchCriteriaNVP } from '@growthware/core/search';
import { LoggingService } from '@growthware/core/logging';
import { ModalService, IModalOptions, ModalOptions, WindowSize } from '@growthware/core/modal';

@Component({
	selector: 'gw-core-base-search',
	template: '',
	styles: [
	]
})
export abstract class BaseSearchComponent implements AfterViewInit, OnInit {

	protected _TheApi: string = '';
	protected _TheComponent!: Type<unknown>;
	protected _TheFeatureName: string = 'Not set';
	protected _TheWindowSize = new WindowSize(450, 900);

	protected _TheService!: BaseService;
	protected _DynamicTableSvc!: DynamicTableService;
	protected _LoggingSvc: LoggingService = inject(LoggingService);
	protected _ModalSvc!: ModalService;
	protected _SearchSvc!: SearchService;

	@ViewChild('dynamicTable', { static: false }) dynamicTable!: DynamicTableComponent;

	public configurationName = '';
	public results: unknown;

	constructor() {
		effect(() => {
			const criteria = this._SearchSvc.searchCriteriaChanged$();
			if (criteria.name.trim().toLowerCase() === this.configurationName.trim().toLowerCase()) {
				this._SearchSvc.getResults(this._TheApi, criteria).then((results) => {
					this._SearchSvc.notifySearchDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
				}).catch((error) => {
					this._LoggingSvc.errorHandler(error, 'BaseSearchComponent', 'constructor.effect');
				});
			}
		});
	}

	ngAfterViewInit(): void {
		// Setup the dynamic table button methods
		const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
		mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => { this.onBtnTopRight(); };
		this.dynamicTable.setButtonMethods(mDynamicTableBtnMethods);
		// setup the dynamic table "single" and "double" row clicks
		this.dynamicTable.rowClickBackMethod = (rowNumber: number) => { this.onRowClick(rowNumber); };
		this.dynamicTable.rowDoubleClickBackMethod = (rowNumber: number) => { this.onRowDoubleClick(rowNumber); };
	}

	ngOnInit(): void {
		// Get the initial SearchCriteriaNVP from the service
		const mAccountTableConfig = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
		const mResults: ISearchCriteriaNVP = this._DynamicTableSvc.getSearchCriteriaFromConfig(this.configurationName, mAccountTableConfig);
		// Set the search criteria to initiate search criteria changed subject
		this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
		this.init();
	}

	private onBtnTopRight() {
		this._TheService.modalReason = 'NewProfile';
		const mModalOptions: IModalOptions = new ModalOptions(this._TheService.addEditModalId, 'Add ' + this._TheFeatureName, this._TheComponent, this._TheWindowSize);
		if (this._ModalSvc) {
			this._ModalSvc.open(mModalOptions);
		}
	}

	// eslint-disable-next-line @typescript-eslint/no-unused-vars
	private onRowClick(rowNumber: number): void {
		// Do nothing ATM just leaving as an example
		// const mMessage = 'hi from BaseSearchComponent.onRowClick row "' + rowNumber + '" was clicked';
		// this._LoggingSvc.toast(mMessage, 'onRowClick', LogLevel.Info);
	}

	protected onRowDoubleClick(rowNumber: number): void {
		const mDataRow = JSON.parse(JSON.stringify(this.dynamicTable.getRowData(rowNumber)));
		// console.log('onRowDoubleClick.mDataRow', mDataRow);
		this._TheService.selectedRow = mDataRow;
		this._TheService.modalReason = 'EditProfile';
		const mModalOptions: IModalOptions = new ModalOptions(this._TheService.addEditModalId, 'Edit ' + this._TheFeatureName, this._TheComponent, this._TheWindowSize);
		if (this._ModalSvc) {
			this._ModalSvc.open(mModalOptions);
		}
	}

	protected init(): void {
		// this.init();
	}

}
