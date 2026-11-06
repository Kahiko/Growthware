
import { Component, effect, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
// Library
import { GWCommon } from '@growthware/common/services';
// Feature
import { CalendarService } from '../../calendar.service';
import { DayOfWeekComponent } from '../day-of-week/day-of-week.component';
import { IMonth, Month } from '../../interfaces/month.model';
import { NamesOfDays } from '../../interfaces/names-of-days.enum';
import { MatActionList } from '@angular/material/list';

@Component({
  selector: 'gw-core-calendar',
  imports: [
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatRadioModule,
    MatSelectModule,
    DayOfWeekComponent,
    MatActionList
  ],
  templateUrl: './calendar.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  private _Action: string = '';

  public avalibleMonths: Array<{ number: number; text: string; abbreviation: string }> = [];
  public calendar: IMonth = new Month();
  public displayMonth: string = '';
  public displayYear: number = 0;
  public weekDayNames: string[] = [];

  constructor(
    private _CalendarSvc: CalendarService,
    private _GWCommon: GWCommon,
    private _Router: Router
  ) {
    this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
    this.avalibleMonths = this._GWCommon.months;
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

  /*
   * Update the selected date when the month is changed
  **/
  public onMonthChange() {
    const mCurrentDate = new Date(this._CalendarSvc.selectedDate);
    const mSelectedMonth = this.avalibleMonths.find((m) => m.text === this.displayMonth)?.number ?? 0;
    mCurrentDate.setMonth(mSelectedMonth - 1);
    this._CalendarSvc.setSelectedDate(this._Action, mCurrentDate);
  }

  /* 
   * Change the year of the selected date by a given direction (1 for next year, -1 for previous year)
   * @param direction - The direction to change the year (1 for next year, -1 for previous year)  
  **/
  public onChangeYear(direction: number) {
    const mCurrentDate = new Date(this._CalendarSvc.selectedDate);
    mCurrentDate.setFullYear(mCurrentDate.getFullYear() + direction);
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
