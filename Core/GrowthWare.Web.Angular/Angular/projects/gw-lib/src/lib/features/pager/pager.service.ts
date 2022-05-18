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
   * @memberof GWLibDynamicTableService
   */
   public getTotalPages(name: string): number {
    return this._TotalPages.get(name.trim().toLowerCase()) || -1;
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
    const mTotalPages = this.getTotalPages(name);
    const mTotalNumberofPages: number = Math.floor(totalRecords / pageSize);
    if(mTotalNumberofPages !== mTotalPages) {
      this._TotalPages.set(name, mTotalNumberofPages);
      this.totalPagesChanged.next(name.trim().toLowerCase());
    }

  }
}
