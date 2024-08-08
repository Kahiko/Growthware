
import { FormsModule } from '@angular/forms';
import { Component } from '@angular/core';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
// Library
import { LogLevel, LoggingService } from '@growthware/core/logging';
// Feature
import { SecurityService } from '../../security.service';

@Component({
	selector: 'gw-core-random-numbers',
	standalone: true,
	imports: [
		FormsModule,
		MatButtonModule
	],
	templateUrl: './random-numbers.component.html',
	styleUrls: ['./random-numbers.component.scss']
})
export class RandomNumbersComponent {


	results: string = '';
	amountOfNumbers: number = 8;
	maxNumber: number = 255;
	minNumber: number = 0;

	constructor(
		private _LoggingSvc: LoggingService,
		private _SecuritySvc: SecurityService
	) { 
		// do nothing here
	}

	/**
   * @description 
   *
   * @memberof RandomNumbersComponent
   */
	doGetNumbers(): void {
		this.results = '';
		this._SecuritySvc.getRandomNumbers(this.amountOfNumbers, this.maxNumber, this.minNumber).catch(()=>{
			this._LoggingSvc.toast('Error calling the API', 'Random Numbers', LogLevel.Error);
		}).then((response)=>{
			if(response)
			{
				this.results = response.toString();
			}
		});
	}

}
