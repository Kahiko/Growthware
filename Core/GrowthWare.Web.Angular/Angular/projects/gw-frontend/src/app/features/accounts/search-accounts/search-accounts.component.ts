import { AfterViewInit, Component, OnInit  } from '@angular/core';
import { ViewChild  } from '@angular/core';
import { GWLibDynamicTableComponent } from 'projects/gw-lib/src/public-api';
import { GWLibSearchService, SearchCriteria } from 'projects/gw-lib/src/public-api';
import { GWLibDynamicTableService } from 'projects/gw-lib/src/public-api';

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
      const mFunctionColumns = '[FunctionSeqId], [Name], [Description], [Action], [Added_By], [Added_Date], [Updated_By], [Updated_Date]';
      const mSearchCriteria: SearchCriteria = new SearchCriteria(mFunctionColumns, "[Action]", "asc", 10, 1, "1=1");
      mSearchCriteria.tableOrView = '[ZGWSecurity].[Functions]';
      this._SearchSvc.getResults(mSearchCriteria).then((results) => {
        this._DynamicTableSvc.setData('Functions', results);
      }).catch((error) => {
        console.log(error);
      });
    }
    this._DynamicTableSvc.requestData('Functions');
    this._DynamicTableSvc.requestData(this.configurationName);
  }

  ngOnInit(): void {
    // do nothing ATM
  }
}
