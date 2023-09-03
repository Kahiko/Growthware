import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CalendarComponent } from './c/calendar/calendar.component';
import { DayOfWeekComponent } from './c/day-of-week/day-of-week.component';

@NgModule({
  declarations: [
    CalendarComponent,
    DayOfWeekComponent
  ],
  imports: [
    CommonModule
  ]
})
export class CommunityCalendarModule { }
