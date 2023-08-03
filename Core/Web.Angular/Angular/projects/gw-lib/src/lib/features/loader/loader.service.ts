import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, debounceTime } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  private _Counter = 0;
  private _Delay = 700;
  private _IsLoading = new BehaviorSubject<boolean>(false);

  public stateChanged$ = new EventEmitter<boolean>();
  
  constructor() { 
    this._IsLoading.pipe(debounceTime(this._Delay))
      .subscribe(value => {
        if (value) {
          this._Counter++;
        }
        else if (this._Counter > 0) {
          this._Counter--;
        }
      this.stateChanged$.emit(this._Counter > 0);
    });
  }

  /**
   * Sets _IsLoading.next triggering loading$
   *
   * @param {boolean} isLoading
   * @memberof LoaderService
   */
  setLoading(isLoading: boolean) {
    this._IsLoading.next(isLoading);
  }
}
