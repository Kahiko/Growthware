import { Component, OnInit } from '@angular/core';
// Library
import { GWCommon } from '@Growthware/common-code';
// Feature
import { CalendarDay } from '../../calendar-day.model';

@Component({
  selector: 'gw-lib-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  private _MonthIndex: number = 0;

  public calendar: CalendarDay[] = [];
  public monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];
  public displayMonth: string = '';

  constructor(
    private _GWCommon: GWCommon
  ){}

  ngOnInit(): void {
    this.generateCalendarDays(this._MonthIndex);
  }

  private generateCalendarDays(monthIndex: number): void {
    // we reset our calendar
    this.calendar = [];
    let mCalendar: CalendarDay[] = [];
    // we set the date 
    let day: Date = new Date(new Date().setMonth(new Date().getMonth() + monthIndex));

    // set the dispaly month for UI
    this.displayMonth = this.monthNames[day.getMonth()];

    let startingDateOfCalendar = this.getStartDateForCalendar(day);

    let dateToAdd = startingDateOfCalendar;

    for (var i = 0; i < 42; i++) {
      this.calendar.push(new CalendarDay(new Date(dateToAdd)));
      dateToAdd = new Date(dateToAdd.setDate(dateToAdd.getDate() + 1));
    }
    mCalendar = this._GWCommon.splitArray(this.calendar, 7);
    this.calendar = JSON.parse(JSON.stringify(mCalendar));
    // console.log('calendar', JSON.stringify(this.calendar));
  }

  private getStartDateForCalendar(selectedDate: Date){
    // for the day we selected let's get the previous month last day
    let lastDayOfPreviousMonth = new Date(selectedDate.setDate(0));

    // start by setting the starting date of the calendar same as the last day of previous month
    let startingDateOfCalendar: Date = lastDayOfPreviousMonth;

    // but since we actually want to find the last Monday of previous month
    // we will start going back in days intil we encounter our last Monday of previous month
    if (startingDateOfCalendar.getDay() != 1) {
      do {
        startingDateOfCalendar = new Date(startingDateOfCalendar.setDate(startingDateOfCalendar.getDate() - 1));
      } while (startingDateOfCalendar.getDay() != 1);
    }

    return startingDateOfCalendar;
  }

   public increaseMonth() {
    this._MonthIndex++;
    this.generateCalendarDays(this._MonthIndex);
  }

  public decreaseMonth() {
    this._MonthIndex--
    this.generateCalendarDays(this._MonthIndex);
  }

  public setCurrentMonth() {
    this._MonthIndex = 0;
    this.generateCalendarDays(this._MonthIndex);
  }
}
