import { AfterViewInit, Component, OnDestroy, OnInit  } from '@angular/core';
import { ViewChild  } from '@angular/core';
import { Subscription } from 'rxjs';

import { DynamicTableComponent } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { DataService, SearchService } from '@Growthware/Lib/src/lib/services';
import { DynamicTableBtnMethods, INameValuePare, SearchCriteriaNVP, SearchResultsNVP } from '@Growthware/Lib/src/lib/models';
import { LoggingService, LogLevel, LogOptions } from '@Growthware/Lib/src/lib/features/logging';

@Component({
  selector: 'app-search-accounts',
  templateUrl: './search-accounts.component.html',
  styleUrls: ['./search-accounts.component.scss']
})
export class SearchAccountsComponent implements AfterViewInit, OnDestroy, OnInit {
  @ViewChild('searchFunctions', {static: false}) searchFunctionsComponent!: DynamicTableComponent;

  private _SearchCriteriaChangedSub: Subscription = new Subscription();

  public configurationName = 'Accounts';
  public results: any;

  constructor(
    private _DataSvc: DataService,
    private _LoggingSvc: LoggingService,
    private _SearchSvc: SearchService,
  ) { }

  ngAfterViewInit(): void {
    const mDynamicTableBtnMethods: DynamicTableBtnMethods = new DynamicTableBtnMethods();
    mDynamicTableBtnMethods.btnTopLeftCallBackMethod = () => { this.onBtnTopLeft(); }
    mDynamicTableBtnMethods.btnTopRightCallBackMethod = () => { this.onBtnTopRight(); }
    mDynamicTableBtnMethods.btnBottomLeftCallBackMethod = () => { this.onBtnBottomLeft(); }
    mDynamicTableBtnMethods.btnBottomRightCallBackMethod = () => { this.onBtnBottomRight(); }
    this.searchFunctionsComponent.setButtonMethods(mDynamicTableBtnMethods);
    const mLogOptions: LogOptions = new LogOptions('Testing using options');
    mLogOptions.componentName = "Search Account"
    mLogOptions.className = 'SearchAccountsComponent';
    mLogOptions.methodName = 'ngAfterViewInit';
    mLogOptions.level = LogLevel.Error;
    this._LoggingSvc.log(mLogOptions);
    this._LoggingSvc.console('Testing using console method', LogLevel.Info);
    mLogOptions.level = LogLevel.Warn;
    mLogOptions.msg = mLogOptions.msg + ' with level Warn'
    this._LoggingSvc.log(mLogOptions);

  }

  ngOnDestroy(): void {
      this._SearchCriteriaChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    this._SearchCriteriaChangedSub = this._SearchSvc.searchCriteriaChanged.subscribe((criteria: INameValuePare) => {
      // this is unnecessary b/c both cases do exactly the same thing but
      // this is to illustrate how you can have multiple 'gw-lib-dynamic-table'
      // and how you would handle loading the data for each of them
      switch (criteria.name.trim().toLowerCase()) {
        case 'accounts':
          this._SearchSvc.searchAccounts(criteria).then((results) => {
            const mResults: SearchResultsNVP = new SearchResultsNVP(results.name, { searchCriteria: results.payLoad.searchCriteria, data: results.payLoad.data });
            this._DataSvc.setData(mResults);
          }).catch((error) => {
            console.log(error);
          });
          break;
        case 'functions':
          this._SearchSvc.searchFunctions(criteria).then((results) => {
            const mResults: SearchResultsNVP = new SearchResultsNVP(results.name, { searchCriteria: results.payLoad.searchCriteria, data: results.payLoad.data });
            this._DataSvc.setData(mResults);
          }).catch((error) => {
            console.log(error);
          });
          break;
        default:
          break;
      }
    });
    // Get the initial SearchCriteriaNVP from the service
    let mResults: SearchCriteriaNVP = this._SearchSvc.getSearchCriteriaFromConfig('Accounts');
    // Set the search criteria to initiate search criteria changed subject
    this._SearchSvc.setSearchCriteria(mResults);
    // repeating for the functions again just to illustrate how this works for multiple <gw-lib-dynamic-table> components
    mResults = this._SearchSvc.getSearchCriteriaFromConfig('Functions');
    this._SearchSvc.setSearchCriteria(mResults);
  }

  private onBtnTopLeft () {
    alert('hi from SearchAccountsComponent.onBtnTopLeft')
  }

  private onBtnTopRight () {
    alert('hi from SearchAccountsComponent.onBtnTopRight')
  }

  private onBtnBottomLeft () {
    alert('hi from SearchAccountsComponent.onBtnBottomLeft')
  }

  private onBtnBottomRight () {
    alert('hi from SearchAccountsComponent.onBtnBottomRight')
  }
}
