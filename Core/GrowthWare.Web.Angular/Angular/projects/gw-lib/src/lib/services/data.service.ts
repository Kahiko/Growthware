import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
// Library Imports
import { SearchResultsNVP, SearchCriteria } from '@Growthware/Lib/src/lib/models';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public dataChanged = new Subject<SearchResultsNVP>();

  constructor(private _GWCommon: GWCommon) { }

  public notifyDataChanged(name: string, data: Array<any>, searchCriteria: SearchCriteria): void {
    const mTotalRecords = this._GWCommon.getTotalRecords(data);
    const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(name, { data: data, totalRecords: mTotalRecords, searchCriteria: searchCriteria })
    this.dataChanged.next(mSearchResultsNVP);
  }
}
