import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  private _IsLoading$$ = new BehaviorSubject<boolean>(false);

  loadingChanged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  
  constructor() { }

  /**
   *
   *
   * @param {boolean} isLoading
   * @memberof LoaderService
   */
  setLoading(isLoading: boolean) {
    this._IsLoading$$.next(isLoading);
  }
}
