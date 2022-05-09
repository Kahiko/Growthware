import { typeWithParameters } from '@angular/compiler/src/render3/util';
import { Component, OnInit } from '@angular/core';
import { GWLibSearchService, SearchCriteria } from 'projects/gw-lib/src/public-api';
import { GWLibDynamicTableService } from 'projects/gw-lib/src/public-api';

@Component({
  selector: 'app-search-accounts',
  templateUrl: './search-accounts.component.html',
  styleUrls: ['./search-accounts.component.scss']
})
export class SearchAccountsComponent implements OnInit {
  private _Columns: string = "[AccountSeqId], [Account], [First_Name], [Last_Name], [Email], [Added_Date], [Last_Login]";
  private _SearchCriteria: SearchCriteria;

  public configurationName = "Accounts";
  public results: any;

  constructor(private _SearchSvc: GWLibSearchService, private _DynamicTableSvc: GWLibDynamicTableService) { }

  ngOnInit(): void {
    this._SearchCriteria = new SearchCriteria(this._Columns, "[Account]", "asc", 10, 1, "1=1");
    this._SearchSvc.getResults(this._SearchCriteria).then((results) => {
      this._DynamicTableSvc.setData(this.configurationName, results);
    }).catch((error) => {
      console.log(error);
    });
  }

}
