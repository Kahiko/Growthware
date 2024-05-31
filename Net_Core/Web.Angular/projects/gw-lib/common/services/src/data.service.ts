import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
// Library Imports
import { INameDataPair, NameDataPair } from '@growthware/common/interfaces';
import { GWCommon } from '../src/gw-common.service';

@Injectable({
	providedIn: 'root'
})
export class DataService {
	public dataChanged$ = new Subject<INameDataPair>();

	constructor(private _GWCommon: GWCommon) { }

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	public notifyDataChanged(name: string, data: Array<any>): void {
		if(!this._GWCommon.isNullOrUndefined(data)) {
			const mNameDataPair: INameDataPair = new NameDataPair(name, data);
			this.dataChanged$.next(mNameDataPair);
		} else {
			// console.warn('DataService.notifyDataChanged: nameDataPair is null or undefined');
		}
	}
}
