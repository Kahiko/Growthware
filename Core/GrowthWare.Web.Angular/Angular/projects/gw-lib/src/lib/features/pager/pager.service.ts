import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GWLibPagerService {
  private _TotalPages: Map<string, number>;

  // Supports the GWLibPagerComponent
  public totalPagesChanged = new Subject<string>();

  constructor() {
    this._TotalPages = new Map<string, number>();
  }

  /**
   * Return the total number of pages for the given compentent name
   *
   * @param {string} name
   * @return {*}  {number}
   * @memberof GWLibPagerService
   */
  public getTotalPages(name: string): number {
    const mName = name.trim().toLowerCase();
    return this._TotalPages.get(mName) || -1;
  }

  /**
   *
   *
   * @param {string} name
   * @param {number} totalRecords
   * @param {number} pageSize
   * @memberof GWLibPagerService
   */
  public setTotalNumberOfPages(name: string, totalRecords: number, pageSize: number): void {
    const mCurrentPages = this.getTotalPages(name);
    const mCalculatedPages: number = Math.floor(totalRecords / pageSize);
    if(mCurrentPages !== mCalculatedPages) {
      this._TotalPages.set(name.trim().toLowerCase(), mCalculatedPages);
      this.totalPagesChanged.next(name);
    }
  }
}
