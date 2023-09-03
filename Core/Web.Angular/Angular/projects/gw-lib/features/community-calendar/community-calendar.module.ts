import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Feature
import { CalendarComponent } from './c/calendar/calendar.component';
import { DayOfWeekComponent } from './c/day-of-week/day-of-week.component';

@NgModule({
  declarations: [
    CalendarComponent,
    DayOfWeekComponent,
  ],
  imports: [
    CommonModule,
    
    MatButtonModule,
    MatIconModule,
  ]
})
export class CommunityCalendarModule { }
