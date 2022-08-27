import { Component, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
// Features (Components/Interfaces/Models/Services)
import { DynamicTableComponent, DynamicTableService } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { SearchService, SearchCriteriaNVP } from '@Growthware/Lib/src/lib/features/search';
import { DynamicTableBtnMethods, INameValuePare } from '@Growthware/Lib/src/lib/models';
import { ModalService, IModalOptions, ModalOptions, ModalSize } from '@Growthware/Lib/src/lib/features/modal';

import { FunctionDetailsComponent } from '../function-details/function-details.component';
import { FunctionService } from '../../function.service';

@Component({
  selector: 'gw-lib-searchfunctions',
  templateUrl: './searchfunctions.component.html',
  styleUrls: ['./searchfunctions.component.scss']
})
export class SearchfunctionsComponent implements OnInit {
  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  @ViewChild('dynamicTable', {static: false}) dynamicTable!: DynamicTableComponent;
  public configurationName = 'Functions';

  constructor(
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
    private _FunctionSvc: FunctionService,
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
        this._SearchSvc.searchFunctions(criteria).then((results) => {
          this._DataSvc.notifyDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
        }).catch((error) => {
          console.log(error);
        });
      }
    });
    // Get the initial SearchCriteriaNVP from the service
    const mFunctionTableConfig = this._DynamicTableSvc.getTableConfiguration(this.configurationName);
    let mResults: SearchCriteriaNVP = this._SearchSvc.getSearchCriteriaFromConfig(this.configurationName, mFunctionTableConfig);
    // Set the search criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
  }

  ngOnDestroy(): void {
    this._SearchCriteriaChangedSub.unsubscribe();
  }

  private onBtnTopRight () {
    this._FunctionSvc.reason = 'add'
    const mModalOptions: IModalOptions = new ModalOptions(this._FunctionSvc.addModalId, 'Add Function', FunctionDetailsComponent, ModalSize.ExtraLarge);
    this._ModalSvc.open(mModalOptions);
  }

  private onRowClick (rowNumber: number): void {
    // Do nothing ATM just leaving as an example
    // const mMessage = 'hi from SearchfunctionsComponent.onRowClick row "' + rowNumber + '" was clicked';
    // this._LoggingSvc.toast(mMessage, 'onRowClick', LogLevel.Info);
  }

  private onRowDoubleClick (rowNumber: number): void {
    const mDataRow: any = this.dynamicTable.getRowData(rowNumber);
    this._FunctionSvc.functionSeqId = mDataRow.FunctionSeqId;
    this._FunctionSvc.reason = 'edit';
    const mModalOptions: IModalOptions = new ModalOptions(this._FunctionSvc.editModalId, 'Edit Function', FunctionDetailsComponent, ModalSize.ExtraLarge);
    this._ModalSvc.open(mModalOptions);
  }
}
