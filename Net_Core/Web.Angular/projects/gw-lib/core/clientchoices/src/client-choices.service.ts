import { Injectable } from '@angular/core';
import { ClientChoices, IClientChoices } from './client-choices.model';

@Injectable({
	providedIn: 'root'
})
export class ClientChoicesService {

	constructor() { }

	/**
   * Retruns a client choices object from sessionStorage or new if not found.
   * @returns IClientChoices
   */
	getClientChoices(): IClientChoices {
		// Note: sessionStorage is managed by the AccountService. 
		const mClientChoicesString = sessionStorage.getItem('clientChoices');
		const mRetVal: IClientChoices = mClientChoicesString ? JSON.parse(mClientChoicesString) : new ClientChoices();
		return mRetVal;
	}
}
