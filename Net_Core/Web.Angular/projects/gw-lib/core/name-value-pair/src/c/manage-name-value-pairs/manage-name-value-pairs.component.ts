import { Component, AfterViewInit, OnInit, ViewChild, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DynamicTableBtnMethods, DynamicTableComponent } from '@growthware/core/dynamic-table';
import { ISearchCriteriaNVP, SearchService } from '@growthware/core/search';
import { LoggingService } from '@growthware/core/logging';
import { ModalService, ModalOptions, WindowSize } from '@growthware/core/modal';
// Feature
import { NameValuePairService } from '../../name-value-pairs.service';
import { INvpParentProfile } from '../../name-value-pair-parent-profile.model';
import { NameValuePairChildDetailComponent } from '../name-value-pair-child-detail/name-value-pair-child-detail.component';
import { NameValuePairParentDetailComponent } from '../name-value-pair-parent-detail/name-value-pair-parent-detail.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
	selector: 'gw-core-manage-name-value-pairs',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
		MatButtonModule,
		// Library
		DynamicTableComponent,
	],
	templateUrl: './manage-name-value-pairs.component.html',
	styleUrls: ['./manage-name-value-pairs.component.scss']
})
export class ManageNameValuePairsComponent implements AfterViewInit, OnInit {

	private _Api_Name: string = 'GrowthwareNameValuePair/';
	private _Api_Nvp_Search: string = '';
	private _Api_Nvp_Details_Search: string = '';
	private _nameValuePairWindowSize: WindowSize = new WindowSize(300, 400);
	private _nameValuePairChildWindowSize: WindowSize = new WindowSize(225, 400);

	activeParrentRowIndex: number = 0;
	childConfigurationName: string = '';

	@ViewChild('dynamicTable', { static: false }) dynamicTable!: DynamicTableComponent;

	nameValuePairColumns: Array<string> = ['Display', 'Description'];
	nameValuePairParentData$ = signal<INvpParentProfile[]>([]);
	nvpChildModalOptions!: ModalOptions;
	nvpParentModalOptions!: ModalOptions;

	constructor(
		private _ModalSvc: ModalService,
		private _NameValuePairService: NameValuePairService,
		private _SearchSvc: SearchService,
		private _LoggingSvc: LoggingService
	) {
		this._Api_Nvp_Search = this._Api_Name + 'SearchNameValuePairs';
		this._Api_Nvp_Details_Search = this._Api_Name + 'SearcNVPDetails';
		this.childConfigurationName = this._NameValuePairService.childConfigurationName;
		this.nvpChildModalOptions = new ModalOptions(this._NameValuePairService.addEditModalId, 'Edit NVP Child', NameValuePairChildDetailComponent, this._nameValuePairChildWindowSize)
		this.nvpParentModalOptions = new ModalOptions(this._NameValuePairService.addEditModalId, 'Edit NVP Parent', NameValuePairParentDetailComponent, this._nameValuePairWindowSize);
		// console.log('ManageNameValuePairsComponent.constructor._Api_Nvp_Details_Search', this._Api_Nvp_Details_Search);
		effect(() => {
			let criteria = this._SearchSvc.searchCriteriaChanged$();
			if (criteria.name.trim().toLowerCase() === this._NameValuePairService.parentConfigurationName.trim().toLowerCase()) {
				this._SearchSvc.getResults(this._Api_Nvp_Search, criteria).then((results) => {
					// console.log('ManageNameValuePairsComponent.ngOnInit results.payLoad.data', results.payLoad.data);
					this.nameValuePairParentData$.update(() => results.payLoad.data);
					this._NameValuePairService.nvpChildId = 0;
					const mNvpParentProfile = this.nameValuePairParentData$()[0];
					this._NameValuePairService.nvpParentId = mNvpParentProfile.nvpSeqId;
					this._NameValuePairService.searchChildNameValuePairs(mNvpParentProfile.nvpSeqId);
				}).catch((error) => {
					this._LoggingSvc.errorHandler(error, 'ManageNameValuePairsComponent', 'ngOnInit');
				});
			}
			if (criteria.name.trim().toLowerCase() === this._NameValuePairService.childConfigurationName.trim().toLowerCase()) {
				this._SearchSvc.getResults(this._Api_Nvp_Details_Search, criteria).then((results) => {
					this._SearchSvc.notifySearchDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
				}).catch((error) => {
					this._LoggingSvc.errorHandler(error, 'ManageNameValuePairsComponent', 'ngOnInit');
				});
			}			
		})
	}

	ngAfterViewInit(): void {
		// Setup the dynamic table button methods
		const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
		mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => { this.onAddClickNvpDetail(); };
		this.dynamicTable.setButtonMethods(mDynamicTableBtnMethods);
		this.dynamicTable.rowDoubleClickBackMethod = (rowNumber: number) => { this.onEditNvpChild(rowNumber); };
	}

	ngOnInit(): void {
		// Get the initial child SearchCriteriaNVP from the service
		let mNvpParentId = this._NameValuePairService.nvpParentId;
		if (!mNvpParentId) {
			mNvpParentId = 1;
		}
		this._NameValuePairService.searchChildNameValuePairs(mNvpParentId);
		// Initial parent SearchCriteriaNVP
		this._NameValuePairService.searchParentNameValuePairs();
	}

	onAddClickNvpParent(): void {
		// this._NameValuePairService.setNameValuePairParrentRow(new NvpParentProfile());
		this._NameValuePairService.nvpParentId = -1;
		this._NameValuePairService.modalReason = 'Add';
		this.nvpParentModalOptions.headerText = 'Add NVP';
		this._ModalSvc.open(this.nvpParentModalOptions);
	}

	onAddClickNvpDetail(): void {
		this._NameValuePairService.nvpChildId = -1;
		this.nvpChildModalOptions.headerText = 'Add NVP Detail';
		this._ModalSvc.open(this.nvpChildModalOptions);
	}

	onEditClickNvpParent(rowIndex: number): void {
		this.onRowClickNvpParent(rowIndex);
		const mNameValuePairParentData = this.nameValuePairParentData$()[rowIndex];
		this._NameValuePairService.nvpParentId = mNameValuePairParentData.nvpSeqId;
		this._NameValuePairService.modalReason = 'Edit';
		this.nvpParentModalOptions.headerText = 'Edit NVP';
		this._ModalSvc.open(this.nvpParentModalOptions);
	}

	onEditNvpChild(rowIndex: number): void {
		// eslint-disable-next-line @typescript-eslint/no-explicit-any
		const mChildRow: any = this.dynamicTable.getRowData(rowIndex);
		this._NameValuePairService.nvpParentId = parseInt(mChildRow['NVP_SEQ_ID']);
		this._NameValuePairService.nvpChildId = parseInt(mChildRow['NVP_SEQ_DET_ID']);
		this.nvpChildModalOptions.headerText = 'Edit NVP Detail';
		this._ModalSvc.open(this.nvpChildModalOptions);
	}

	onRowClickNvpParent(rowIndex: number): void {
		this.activeParrentRowIndex = rowIndex;
		const mNvpParentProfile: INvpParentProfile = this.nameValuePairParentData$()[rowIndex];
		if (mNvpParentProfile) {
			this._NameValuePairService.nvpParentId = mNvpParentProfile.nvpSeqId;
			this._NameValuePairService.searchChildNameValuePairs(mNvpParentProfile.nvpSeqId);
		}
	}
}
