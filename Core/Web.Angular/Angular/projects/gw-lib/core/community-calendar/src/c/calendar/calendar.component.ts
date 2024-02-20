import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
// Library
import { GWCommon } from '@growthware/common/services';
// Feature
// import { CalendarDay } from '../../calendar-day.model';
import { CalendarService } from '../../calendar.service';
import { DayOfWeekComponent } from '../day-of-week/day-of-week.component';
import { IMonth, Month } from '../../interfaces/month.model';
import { NamesOfDays } from '../../interfaces/names-of-days.enum';
import { Subscription } from 'rxjs';

@Component({
  selector: 'gw-core-calendar',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    // Angular Material
    MatButtonModule,
    MatIconModule,
    MatRadioModule,
    // Feature
    DayOfWeekComponent
  ],
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  private _FirstDayOfWeek: NamesOfDays = NamesOfDays.Monday;
  private _Subscriptions: Subscription = new Subscription();
  private _SelectedDate: Date = new Date();
  // public calendar: CalendarDay[] = [];
  public calendar: IMonth = new Month();
  public displayMonth: string = '';
  public displayYear: number = 0;
  public incrementBy: string = 'month';
  public weekDayNames: string[] = [];

  constructor(
		private _CalendarSvc: CalendarService,
		private _GWCommon: GWCommon
  ) { }

  ngOnInit(): void {
    this._FirstDayOfWeek = NamesOfDays.Sunday;
    this._Subscriptions.add(this._CalendarSvc.calendarData$.subscribe((data: IMonth) => {
      this.calendar = data;
      this.displayMonth = this._SelectedDate.toLocaleString('default', { month: 'long' });
      this.displayYear = this._SelectedDate.getFullYear();
    }));
    // Set the data for the calendar header
    this.getWeekDayNames();
    // console.log('CalendarComponent.ngOnInit.weekDayNames', this.weekDayNames);
    // Set the data for the calendar
    this.getCalendarData();
    // console.log('CalendarComponent.ngOnInit.calendar', this.calendar);
  }

  public increase() {
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (this.incrementBy) {
      case 'day': {
        this._SelectedDate.setDate(this._SelectedDate.getDate() + 1);
        break;
      }
      case 'month': {
        this._SelectedDate.setMonth(this._SelectedDate.getMonth() + 1);
        break;
      }
      case 'year': {
        this._SelectedDate.setFullYear(this._SelectedDate.getFullYear() + 1);
        break;
      }
      default: {
        this._SelectedDate.setMonth(this._SelectedDate.getMonth() + 1);
      }
    }
    this.getCalendarData();
  }

  public decrease() {
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (this.incrementBy) {
      case 'day': {
        this._SelectedDate.setDate(this._SelectedDate.getDate() - 1);
        break;
      }
      case 'month': {
        this._SelectedDate.setMonth(this._SelectedDate.getMonth() - 1);
        break;
      }
      case 'year': {
        this._SelectedDate.setFullYear(this._SelectedDate.getFullYear() - 1);
        break;
      }
      default: {
        this._SelectedDate.setMonth(this._SelectedDate.getMonth() - 1);
      }
    }
    this.getCalendarData();
  }

  public currentDate() {
    this._SelectedDate = new Date();
    this.getCalendarData();
  }

  private getCalendarData(): void {
    this._CalendarSvc.getMonthData(this._SelectedDate, this._FirstDayOfWeek);
  }

  private getWeekDayNames(): void {
    this.weekDayNames.length = 0;
    let mDay: number = this._FirstDayOfWeek;
    for (let i = 0; i < 7; i++) {
      this.weekDayNames.push(NamesOfDays[mDay]);
      if (mDay === 6) {
        mDay = 0;
      } else {
        mDay++;
      }
    }
  }
}
