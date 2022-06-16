import { AfterViewInit, Component, OnDestroy, OnInit  } from '@angular/core';
import { ViewChild  } from '@angular/core';
import { Subscription } from 'rxjs';

import { DynamicTableComponent } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { SearchService } from '@Growthware/Lib/src/lib/services';
import { DynamicTableBtnMethods, INameValuePare, SearchCriteriaNVP, SearchResultsNVP } from '@Growthware/Lib/src/lib/models';

@Component({
  selector: 'app-search-accounts',
  templateUrl: './search-accounts.component.html',
  styleUrls: ['./search-accounts.component.scss']
})
export class SearchAccountsComponent implements AfterViewInit, OnDestroy, OnInit {
  @ViewChild('searchFunctions', {static: false}) searchFunctionsComponent: DynamicTableComponent;

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  public configurationName = 'Accounts';
  public results: any;

  constructor(
    private _SearchSvc: SearchService,
    private _DataSvc: DataService,
  ) { }

  ngAfterViewInit(): void {
    const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
    mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => {
      this.onBtnTopRight();
    }
    this.searchFunctionsComponent.setButtonMethods(mDynamicTableBtnMethods);
  }

  ngOnDestroy(): void {
      this._SearchCriteriaChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    // do nothing ATM
    this._SearchCriteriaChangedSub = this._SearchSvc.searchCriteriaChanged.subscribe((criteria: INameValuePare) => {
      // this is unnecessary b/c both cases do exactly the same thing but
      // this is to illistrate how you can have multiple 'gw-lib-dynamic-table'
      // and how you would handle loading the data for each of them
      switch (criteria.name.trim().toLowerCase()) {
        case 'accounts':
          this._SearchSvc.getResults(criteria).then((results) => {
            const mResutls: SearchResultsNVP = new SearchResultsNVP(results.name, { searchCriteria: results.payLoad.searchCriteria, data: results.payLoad.data });
            this._DataSvc.setData(mResutls);
          }).catch((error) => {
            console.log(error);
          });
          break;
        case 'functions':
          this._SearchSvc.getResults(criteria).then((results) => {
            const mResutls: SearchResultsNVP = new SearchResultsNVP(results.name, { searchCriteria: results.payLoad.searchCriteria, data: results.payLoad.data });
            this._DataSvc.setData(mResutls);
          }).catch((error) => {
            console.log(error);
          });
          break;
        default:
          break;
      }
    });
    // initiate getting the data for both of the controlls
    let mResutls: SearchCriteriaNVP = this._SearchSvc.getSearchCriteriaFromConfig('Accounts');
    this._SearchSvc.setSearchCriteria(mResutls);
    mResutls = this._SearchSvc.getSearchCriteriaFromConfig('Functions');
    this._SearchSvc.setSearchCriteria(mResutls);
  }

  private onBtnTopRight () {
    alert('hi from onBtnTopLeft')
  }
}
