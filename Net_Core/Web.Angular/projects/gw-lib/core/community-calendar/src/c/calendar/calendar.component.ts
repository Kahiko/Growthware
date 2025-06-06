import { CommonModule } from '@angular/common';
import { Component, effect, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
// Library
import { GWCommon } from '@growthware/common/services';
// Feature
import { CalendarService } from '../../calendar.service';
import { DayOfWeekComponent } from '../day-of-week/day-of-week.component';
import { IMonth, Month } from '../../interfaces/month.model';
import { NamesOfDays } from '../../interfaces/names-of-days.enum';

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
  private _Action: string = '';
  // public calendar: CalendarDay[] = [];
  public calendar: IMonth = new Month();
  public displayMonth: string = '';
  public displayYear: number = 0;
  public incrementBy: string = 'month';
  public weekDayNames: string[] = [];

  constructor(
    private _CalendarSvc: CalendarService,
    private _GWCommon: GWCommon,
    private _Router: Router
  ) {
    this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
    effect(() => {
      this.calendar = this._CalendarSvc.calendarData$();
      this.displayMonth = this._CalendarSvc.selectedDate.toLocaleString('default', { month: 'long' });
      this.displayYear = this._CalendarSvc.selectedDate.getFullYear();
      setTimeout(() => {
        const mSelectedElement = document.getElementsByClassName('selected-header');
        // console.log('CalendarComponent.ngAfterContentInit.mSelectedElement', mSelectedElement);
        if (mSelectedElement) {
          const mSelectedDay = mSelectedElement[0];
          if (mSelectedDay) {
            // mSelectedDay.scrollIntoView({behavior: 'smooth'});
            mSelectedDay.scrollIntoView({
              behavior: 'smooth',
              block: 'center',
              inline: 'nearest'
            });
          }
        }
      }, 500);
    });
  }

  ngOnInit(): void {
    // Set the first day of the week
    // TODO: Add the first day of the week to the clientChoices
    this._CalendarSvc.setFirstDayOfWeek(NamesOfDays.Sunday);
    // Set the data for the calendar header
    this.getWeekDayNames();
    // console.log('CalendarComponent.ngOnInit.weekDayNames', this.weekDayNames);
    // Set the data for the calendar
    this._CalendarSvc.setSelectedDate(this._Action, this._CalendarSvc.selectedDate, true);
    // console.log('CalendarComponent.ngOnInit.calendar', this.calendar);
  }

  public increase() {
    const mCurrentDate = new Date(this._CalendarSvc.selectedDate);
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (this.incrementBy) {
      case 'day': {
        mCurrentDate.setDate(mCurrentDate.getDate() + 1);
        break;
      }
      case 'month': {
        mCurrentDate.setMonth(mCurrentDate.getMonth() + 1);
        break;
      }
      case 'year': {
        mCurrentDate.setFullYear(mCurrentDate.getFullYear() + 1);
        break;
      }
      default: {
        mCurrentDate.setMonth(mCurrentDate.getMonth() + 1);
      }
    }
    this._CalendarSvc.setSelectedDate(this._Action, mCurrentDate);
  }

  public decrease() {
    const mCurrentDate = new Date(this._CalendarSvc.selectedDate);
    switch (this.incrementBy) {
      case 'day': {
        mCurrentDate.setDate(mCurrentDate.getDate() - 1);
        break;
      }
      case 'month': {
        mCurrentDate.setMonth(mCurrentDate.getMonth() - 1);
        break;
      }
      case 'year': {
        mCurrentDate.setFullYear(mCurrentDate.getFullYear() - 1);
        break;
      }
      default: {
        mCurrentDate.setMonth(mCurrentDate.getMonth() - 1);
      }
    }
    this._CalendarSvc.setSelectedDate(this._Action, mCurrentDate);
  }

  public currentDate() {
    const mCurrentDate = new Date();
    this._CalendarSvc.setSelectedDate(this._Action, mCurrentDate);
  }

  private getWeekDayNames(): void {
    this.weekDayNames.length = 0;
    let mDay: number = this._CalendarSvc.firstDayOfWeek;
    for (let i = 0; i < 7; i++) {
      this.weekDayNames.push(NamesOfDays[mDay]);
      if (mDay === 6) {
        mDay = 0;
      } else {
        mDay++;
      }
    }
  }

  private updateSelectedDate(): void {
    this._CalendarSvc.setSelectedDate(this._Action, this._CalendarSvc.selectedDate);
  }
}
