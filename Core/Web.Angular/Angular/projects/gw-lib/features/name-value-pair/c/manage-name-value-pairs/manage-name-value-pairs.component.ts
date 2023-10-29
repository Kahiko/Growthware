import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
// import { DataService } from '@Growthware/shared/services';
import { ISearchCriteria, SearchCriteria, SearchCriteriaNVP, SearchService } from '@Growthware/features/search';
import { LoggingService } from '@Growthware/features/logging';
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
  ],
  templateUrl: './manage-name-value-pairs.component.html',
  styleUrls: ['./manage-name-value-pairs.component.scss']
})
export class ManageNameValuePairsComponent implements OnDestroy, OnInit {

  private _Subscription: Subscription = new Subscription();
  private _Api_Name: string = 'GrowthwareNameValuePair/';
  private _Api_Nvp_Search: string = '';
  private _Api_Nvp_Details_Search: string = '';
  private _NameValuePairDataSubject = new BehaviorSubject<INvpParentProfile[]>([]);

  configurationName = 'SearchNameValuePairs';
  _nameValuePairWindowSize: WindowSize = new WindowSize(800, 600);

  nameValuePairColumns: Array<string> = ['Display', 'Description'];
  readonly nameValuePairData$ = this._NameValuePairDataSubject.asObservable();
  nameValuePairModalOptions: ModalOptions = new ModalOptions(this._NameValuePairService.modalIdNVPParrent, 'Edit NVP Parent', NameValuePairParentDetailComponent, this._nameValuePairWindowSize);
  
  constructor(
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
        if(criteria.name.trim().toLowerCase() === this.configurationName.trim().toLowerCase()) {
          this._SearchSvc.getResults(this._Api_Nvp_Search, criteria).then((results) => {
            // this.nameValuePairData = results.payLoad.data;
            this._NameValuePairDataSubject.next(results.payLoad.data);
            console.log('ManageNameValuePairsComponent.ngOnInit.nameValuePairData$', this._NameValuePairDataSubject.getValue());
            // this._DataSvc.notifySearchDataChanged(results.name, results.payLoad.data, results.payLoad.searchCriteria);
          }).catch((error) => {
            this._LoggingSvc.errorHandler(error, 'ManageNameValuePairsComponent', 'ngOnInit');
          });
        }
      })
    );

    // Get the initial SearchCriteriaNVP
    const mSearchColumns: Array<string> = ['Static_Name', 'Display', 'Description'];
    const mSortColumns: Array<string> = ['Display'];
    const mNumberOfRecords: number = 10;
    const mSearchCriteria: ISearchCriteria = new SearchCriteria(mSearchColumns,mSortColumns,mNumberOfRecords,'',1);
    const mResults: SearchCriteriaNVP = new SearchCriteriaNVP(this.configurationName, mSearchCriteria);
    console.log('ManageNameValuePairsComponent.ngOnInit.mResults', mResults);
    // Set the search criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(mResults.name, mResults.payLoad);
  }

  onAddClickNvpParent(): void {
    this._NameValuePairService.setNameNVPParrentRow(new NvpParentProfile());
    this.nameValuePairModalOptions.headerText = 'Add NVP';
    this._ModalSvc.open(this.nameValuePairModalOptions);    
  }

  onEditClickNvpParent(rowIndex: number): void {
    this._NameValuePairService.setNameNVPParrentRow(this._NameValuePairDataSubject.getValue()[rowIndex]);
    this.nameValuePairModalOptions.headerText = 'Edit NVP';
    this._ModalSvc.open(this.nameValuePairModalOptions);
  }

  onRowClickNvpParent(rowIndex: number): void {
    console.log('onRowClickNvpParent.row', this._NameValuePairDataSubject.getValue()[rowIndex]);
  }

}
