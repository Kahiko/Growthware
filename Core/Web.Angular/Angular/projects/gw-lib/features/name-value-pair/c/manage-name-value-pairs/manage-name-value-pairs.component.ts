import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataService } from '@Growthware/shared/services';
import { DynamicTableModule, DynamicTableService } from '@Growthware/features/dynamic-table';
import { ISearchCriteria, SearchCriteria, SearchCriteriaNVP, SearchService } from '@Growthware/features/search';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
import { ModalService, ModalOptions, WindowSize } from '@Growthware/features/modal';
import { INameValuePair } from '@Growthware/shared/models';
// Feature
import { NameValuePairService } from '../../name-value-pairs.service';
import { INvpParentProfile, NvpParentProfile } from '../../name-value-pair-parent-profile.model';
import { NameValuePairParentDetailComponent } from '../name-value-pair-parent-detail/name-value-pair-parent-detail.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'gw-lib-manage-name-value-pairs',
  standalone: true,
  imports: [
    CommonModule,

    MatButtonModule,

    DynamicTableModule,
  ],
  templateUrl: './manage-name-value-pairs.component.html',
  styleUrls: ['./manage-name-value-pairs.component.scss']
})
export class ManageNameValuePairsComponent implements OnDestroy, OnInit {

  private _Subscription: Subscription = new Subscription();
  private _Api_Name: string = 'GrowthwareNameValuePair/';
  private _Api_Nvp_Search: string = '';
  private _Api_Nvp_Details_Search: string = '';
  private _SearchCriteriaNVP!: SearchCriteriaNVP;
  private _NameValuePairParentDataSubject = new BehaviorSubject<INvpParentProfile[]>([]);

  childConfigurationName = 'SearchNVPDetails';

  parentConfigurationName = 'SearchNameValuePairs';
  _nameValuePairWindowSize: WindowSize = new WindowSize(400, 600);

  nameValuePairColumns: Array<string> = ['Display', 'Description'];
  readonly nameValuePairParentData$ = this._NameValuePairParentDataSubject.asObservable();
  nameValuePairModalOptions: ModalOptions = new ModalOptions(this._NameValuePairService.modalIdNVPParrent, 'Edit NVP Parent', NameValuePairParentDetailComponent, this._nameValuePairWindowSize);
  
  constructor(
    private _DataSvc: DataService,
    private _DynamicTableSvc: DynamicTableService,
    private _ModalSvc: ModalService,
    private _NameValuePairService: NameValuePairService,
    private _SearchSvc: SearchService,
    private _LoggingSvc: LoggingService
  ){
    this._Api_Nvp_Search = this._Api_Name + 'SearchNameValuePairs';
    this._Api_Nvp_Details_Search = this._Api_Name + 'SearcNVPDetails';
    console.log('ManageNameValuePairsComponent.constructor._Api_Nvp_Details_Search', this._Api_Nvp_Details_Search);
  }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscription.add(
      this._SearchSvc.searchCriteriaChanged.subscribe((criteria: INameValuePair) => {
        if(criteria.name.trim().toLowerCase() === this.parentConfigurationName.trim().toLowerCase()) {
          this._SearchSvc.getResults(this._Api_Nvp_Search, criteria).then((results) => {
            this._NameValuePairParentDataSubject.next(results.payLoad.data);
          }).catch((error) => {
            this._LoggingSvc.errorHandler(error, 'ManageNameValuePairsComponent', 'ngOnInit');
          });
        }
      })
    );
    this._Subscription.add(
      this._SearchSvc.searchCriteriaChanged.subscribe((criteria: INameValuePair) => {
        if(criteria.name.trim().toLowerCase() === this.childConfigurationName.trim().toLowerCase()) {
          this._SearchSvc.getResults(this._Api_Nvp_Details_Search, criteria).then((results) => {
            this._DataSvc.notifySearchDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
          }).catch((error) => {
            this._LoggingSvc.errorHandler(error, 'ManageNameValuePairsComponent', 'ngOnInit');
          });
        }
      })
    );
    // Get the initial child SearchCriteriaNVP from the service
    const mAccountTableConfig = this._DynamicTableSvc.getTableConfiguration(this.childConfigurationName);
    this._SearchCriteriaNVP = this._DynamicTableSvc.getSearchCriteriaFromConfig(this.childConfigurationName, mAccountTableConfig);
    this._SearchCriteriaNVP.payLoad.searchText = '1'
    // Set the search child criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(this._SearchCriteriaNVP.name, this._SearchCriteriaNVP.payLoad);

    // Get the initial parent SearchCriteriaNVP
    const mSearchColumns: Array<string> = ['Static_Name', 'Display', 'Description'];
    const mSortColumns: Array<string> = ['Display'];
    const mNumberOfRecords: number = 10;
    const mSearchCriteria: ISearchCriteria = new SearchCriteria(mSearchColumns, mSortColumns, mNumberOfRecords, '', 1);
    const mResults: SearchCriteriaNVP = new SearchCriteriaNVP(this.parentConfigurationName, mSearchCriteria);
    // Set the search parent criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
  }

  onAddClickNvpParent(): void {
    this._NameValuePairService.setNameValuePairParrentRow(new NvpParentProfile());
    this.nameValuePairModalOptions.headerText = 'Add NVP';
    this._ModalSvc.open(this.nameValuePairModalOptions);    
  }

  onEditClickNvpParent(rowIndex: number): void {
    this._NameValuePairService.setNameValuePairParrentRow(this._NameValuePairParentDataSubject.getValue()[rowIndex]);
    this.nameValuePairModalOptions.headerText = 'Edit NVP';
    this._ModalSvc.open(this.nameValuePairModalOptions);
  }

  onRowClickNvpParent(rowIndex: number): void {
    this._NameValuePairService.setNameValuePairDetailRow(this._NameValuePairParentDataSubject.getValue()[rowIndex]);
    this._SearchCriteriaNVP.payLoad.searchText = this._NameValuePairParentDataSubject.getValue()[rowIndex].NVPSeqId.toString();
    // Set the search child criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(this._SearchCriteriaNVP.name, this._SearchCriteriaNVP.payLoad);
    this._LoggingSvc.toast('Supposed to set search criteria and get results', 'Name Value Pairs', LogLevel.Debug);
  }

}
