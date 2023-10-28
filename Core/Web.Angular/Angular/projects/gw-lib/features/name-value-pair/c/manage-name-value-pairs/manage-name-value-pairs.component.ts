import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
// Library
// import { DataService } from '@Growthware/shared/services';
import { ISearchCriteria, SearchCriteria, SearchCriteriaNVP, SearchService } from '@Growthware/features/search';
import { LoggingService } from '@Growthware/features/logging';
import { INameValuePair } from '@Growthware/shared/models';
// Feature
// import { NameValuePairService } from '../../name-value-pairs.service';

@Component({
  selector: 'gw-lib-manage-name-value-pairs',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './manage-name-value-pairs.component.html',
  styleUrls: ['./manage-name-value-pairs.component.scss']
})
export class ManageNameValuePairsComponent implements OnDestroy, OnInit {

  private _Subscription: Subscription = new Subscription();
  private _Api_Name: string = 'GrowthwareNameValuePair/';
  private _Api_Search: string = '';

  configurationName = 'SearchNameValuePairs';
  
  constructor(
    // private _DataSvc: DataService,
    private _SearchSvc: SearchService,
    private _LoggingSvc: LoggingService
    // private _NameValuePairsSvc: NameValuePairService
  ){
    this._Api_Search = this._Api_Name + 'SearchNameValuePairs';
  }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscription.add(
      this._SearchSvc.searchCriteriaChanged.subscribe((criteria: INameValuePair) => {
        if(criteria.name.trim().toLowerCase() === this.configurationName.trim().toLowerCase()) {
          this._SearchSvc.getResults(this._Api_Search, criteria).then((results) => {
            console.log('ManageNameValuePairsComponent.ngOnInit.data', results.payLoad.data);
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

}
