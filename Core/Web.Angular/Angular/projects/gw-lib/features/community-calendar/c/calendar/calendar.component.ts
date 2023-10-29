import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Library
import { GWCommon } from '@Growthware/common-code';
// Feature
import { CalendarDay } from '../../calendar-day.model';
import { DayOfWeekComponent } from '../day-of-week/day-of-week.component';

@Component({
  selector: 'gw-lib-calendar',
  standalone: true,
  imports: [
    CommonModule,

    MatButtonModule,
    MatIconModule,

    DayOfWeekComponent,
  ],
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
    const day: Date = new Date(new Date().setMonth(new Date().getMonth() + monthIndex));

    // set the dispaly month for UI
    this.displayMonth = this.monthNames[day.getMonth()];

    const startingDateOfCalendar = this.getStartDateForCalendar(day);

    let dateToAdd = startingDateOfCalendar;
    let mDayNeededForCalendar = this.getNumberOfCalendarDays(day);
    for (var i = 0; i < mDayNeededForCalendar; i++) {
      this.calendar.push(new CalendarDay(new Date(dateToAdd)));
      dateToAdd = new Date(dateToAdd.setDate(dateToAdd.getDate() + 1));
    }
    mCalendar = this._GWCommon.splitArray(this.calendar, 7);
    this.calendar = JSON.parse(JSON.stringify(mCalendar));
  }

  /**
   * Calculates the number of calendar days in a given month.
   *
   * @param {Date} day - The date used to determine the month.
   * @return {number} The number of calendar days in the specified month.
   */
  private getNumberOfCalendarDays(day: Date): number {
    const mYear = day.getFullYear();
    const mMonth = day.getMonth()+ 1;
    const mFristDay = new Date(mYear, mMonth, 1).getDay();
    const mLastDay = new Date(mYear, mMonth + 1, 0).getDay();
    const mDaysFromLastMonth = mFristDay - 1 == -1 ? 6 : mFristDay - 1;
    const mDaysFromNextMonth = 6 - mLastDay == -1 ? 0 : 6 - mLastDay;
    const mDaysInMonth = new Date(mYear, mMonth + 1, 0).getDate();
    const mRetVal = (mDaysFromLastMonth + mDaysInMonth + mDaysFromNextMonth) + 1;
    return mRetVal;
  }

  private getStartDateForCalendar(selectedDate: Date){
    // for the day we selected let's get the previous month last day
    const lastDayOfPreviousMonth = new Date(selectedDate.setDate(0));
    const mLastDayOfPreviousMonthName = lastDayOfPreviousMonth.toLocaleString('en-us', {  weekday: 'long' });

    // start by setting the starting date of the calendar same as the last day of previous month
    let startingDateOfCalendar: Date = lastDayOfPreviousMonth;

    // but since we actually want to find the last Monday of previous month
    // we will start going back in days intil we encounter our last Monday of previous month
    if(mLastDayOfPreviousMonthName !== 'Sunday') {
      if (startingDateOfCalendar.getDay() != 1) {
        do {
          startingDateOfCalendar = new Date(startingDateOfCalendar.setDate(startingDateOfCalendar.getDate() - 1));
        } while (startingDateOfCalendar.getDay() != 1);
      }  
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
