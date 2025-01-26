import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject, debounceTime } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class LoaderService {
	private _Counter = 0;
	private _Delay = 700;
	private _IsLoading = new BehaviorSubject<boolean>(false);
	private _Paused: boolean = false;

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
	public setLoading(isLoading: boolean): void {
		if (!this._Paused) {
			this._IsLoading.next(isLoading);
		}
	}

	/**
	 * Pauses the loading state, preventing any changes to the loading indicator.
	 * Once paused, calls to setLoading will not trigger state changes.
	 * Use resume() to allow state changes again.
	 */
	public pause(): void {
		this._Paused = true;
	}

	/**
	 * Resumes the loading state, allowing setLoading to trigger state changes again.
	 */
	public resume(): void {
		this._Paused = false;
	}
}
