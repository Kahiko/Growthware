import { Component, OnInit } from '@angular/core';
// Library
import { LogLevel, LoggingService } from '@Growthware/features/logging';
// Feature
import { SecurityService } from '../../security.service';

@Component({
  selector: 'gw-lib-random-numbers',
  templateUrl: './random-numbers.component.html',
  styleUrls: ['./random-numbers.component.scss']
})
export class RandomNumbersComponent implements OnInit {


  results: string = '';
  amountOfNumbers: number = 8;
  maxNumber: number = 255;
  minNumber: number = 0;

  constructor(
    private _LoggingSvc: LoggingService,
    private _SecuritySvc: SecurityService
  ) { }

  ngOnInit(): void {
    // do nothing atm
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
