import { AfterViewInit, Component, OnInit  } from '@angular/core';
import { ViewChild  } from '@angular/core';
import { DynamicTableComponent } from 'projects/gw-lib/src/public-api';
import { DynamicTableService, SearchService } from 'projects/gw-lib/src/public-api';
import { GWCommon, SearchCriteria } from 'projects/gw-lib/src/public-api';

@Component({
  selector: 'app-search-accounts',
  templateUrl: './search-accounts.component.html',
  styleUrls: ['./search-accounts.component.scss']
})
export class SearchAccountsComponent implements AfterViewInit, OnInit {
  @ViewChild('searchFunctions', {static: false}) searchFunctionsComponent: DynamicTableComponent;

  private _SearchCriteria: SearchCriteria;

  public configurationName = 'Accounts';
  public results: any;

  constructor(private _GWCommon: GWCommon, private _SearchSvc: SearchService, private _DynamicTableSvc: DynamicTableService) { }

  ngAfterViewInit(): void {
    // Testing having multiple dynamic table components and overrideing the
    // dynamic table components getData method
    this.searchFunctionsComponent.getData = () => {
      // b/c we are using the search service and only overriding as an example
      // we only need to attempt getting data if "our" search criteria exists
      let mSearchCriteria = this._SearchSvc.getSearchCriteria('Functions');
      if(!this._GWCommon.isNullorEmpty(mSearchCriteria.columns)) {
        this._SearchSvc.getResults(mSearchCriteria).then((results) => {
          this._DynamicTableSvc.setData('Functions', results);
        }).catch((error) => {
          console.log(error);
        });
      }
    }
    let mSearchCriteria = this._SearchSvc.getSearchCriteria('Functions');
    if(this._GWCommon.isNullorEmpty(mSearchCriteria.columns)) {
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
