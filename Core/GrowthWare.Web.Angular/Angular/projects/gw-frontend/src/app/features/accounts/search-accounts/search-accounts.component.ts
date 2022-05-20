import { AfterViewInit, Component, OnInit  } from '@angular/core';
import { ViewChild  } from '@angular/core';
import { GWLibDynamicTableComponent } from 'gw-lib';
import { GWLibDynamicTableService, GWLibSearchService } from 'gw-lib';
import { GWCommon, SearchCriteria } from 'gw-lib';

@Component({
  selector: 'app-search-accounts',
  templateUrl: './search-accounts.component.html',
  styleUrls: ['./search-accounts.component.scss']
})
export class SearchAccountsComponent implements AfterViewInit, OnInit {
  @ViewChild('searchFunctions', {static: false}) searchFunctionsComponent: GWLibDynamicTableComponent;

  private _SearchCriteria: SearchCriteria;

  public configurationName = 'Accounts';
  public results: any;

  constructor(private _SearchSvc: GWLibSearchService, private _DynamicTableSvc: GWLibDynamicTableService) { }

  ngAfterViewInit(): void {
    // Testing having multiple dynamic table components and overrideing the
    // dynamic table components getData method
    this.searchFunctionsComponent.getData = () => {
      // b/c we are using the search service and only overriding as an example
      // we only need to attempt getting data if "our" search criteria exists
      let mSearchCriteria = this._SearchSvc.getSearchCriteria('Functions');
      if(!GWCommon.isNullorEmpty(mSearchCriteria.columns)) {
        this._SearchSvc.getResults(mSearchCriteria).then((results) => {
          this._DynamicTableSvc.setData('Functions', results);
        }).catch((error) => {
          console.log(error);
        });
      }
    }
    let mSearchCriteria = this._SearchSvc.getSearchCriteria('Functions');
    if(GWCommon.isNullorEmpty(mSearchCriteria.columns)) {
      const mFunctionColumns = '[FunctionSeqId], [Name], [Description], [Action], [Added_By], [Added_Date], [Updated_By], [Updated_Date]';
      mSearchCriteria = new SearchCriteria(mFunctionColumns, "[Action]", "asc", 10, 1, "1=1");
      mSearchCriteria.tableOrView = '[ZGWSecurity].[Functions]';
    }
    // setSearchCriteria will call next on this._SearchSvc.searchCriteriaChanged
    this._SearchSvc.setSearchCriteria('Functions', mSearchCriteria);
  }

  ngOnInit(): void {
    // do nothing ATM
  }
}
