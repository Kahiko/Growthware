import { Component, Input, OnInit } from '@angular/core';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
// Feature
import { CalendarDay } from '../../calendar-day.model';

@Component({
  selector: 'gw-DayOfWeek',
  templateUrl: './day-of-week.component.html',
  styleUrls: ['./day-of-week.component.css']
})
export class DayOfWeekComponent implements OnInit {

  public monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];
  @Input() day?: any;

  constructor(
    private _GWCommon: GWCommon,
    private _LoggingService: LoggingService
  ) { }

  ngOnInit(): void {
    if(this._GWCommon.isNullOrUndefined(this.day)) {
      this._LoggingService.toast('the "day" property is required', 'DayOfWeekComponent', LogLevel.Error);
    } else {
      this.day = new CalendarDay(new Date(this.day.value.date));
      // console.log('DayOfWeek', this.day);
    }
  }

}