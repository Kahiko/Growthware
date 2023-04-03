import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  private _IsLoading = new BehaviorSubject<boolean>(false);

  loadingChanged: Observable<boolean> = this._IsLoading.asObservable();
  
  constructor() { }

  /**
   * Sets _IsLoading.next triggering loadingChanged
   *
   * @param {boolean} isLoading
   * @memberof LoaderService
   */
  setLoading(isLoading: boolean) {
    this._IsLoading.next(isLoading);
  }
}
