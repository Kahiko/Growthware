import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { SearchResultsNVP } from '@Growthware/Lib/src/lib/models';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public dataChanged = new Subject<SearchResultsNVP>();

  constructor() { }

  public setData(results: SearchResultsNVP): void {
    const mResutls: SearchResultsNVP = new SearchResultsNVP(results.name, { searchCriteria: results.payLoad.searchCriteria, data: results.payLoad.data });
    this.dataChanged.next(mResutls);
  }
}
