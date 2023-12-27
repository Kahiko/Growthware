import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
// Library Imports
import { DataNVP } from '@Growthware/shared/models';
import { GWCommon } from '@Growthware/common-code';
import { SearchResultsNVP, SearchCriteria } from '@Growthware/features/search';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public dataChanged = new Subject<DataNVP>();
  public searchDataChanged = new Subject<SearchResultsNVP>();

  constructor(private _GWCommon: GWCommon) { }

  public notifyDataChanged(dataName: string, data: unknown[]): void {
    const mDataNVP: DataNVP = new DataNVP(dataName, data);
    this.dataChanged.next(mDataNVP);
  }

  public notifySearchDataChanged(name: string, data: unknown[], searchCriteria: SearchCriteria): void {
    const mTotalRecords = this._GWCommon.getTotalRecords(data);
    const mSearchResultsNVP: SearchResultsNVP = new SearchResultsNVP(name, { data: data, totalRecords: mTotalRecords, searchCriteria: searchCriteria });
    this.searchDataChanged.next(mSearchResultsNVP);
  }
}
