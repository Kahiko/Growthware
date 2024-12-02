import { Routes } from '@angular/router';
// Feature
import { CalendarComponent } from './c/calendar/calendar.component';

export const communityCalendarRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/calendar/calendar.component').then(m => m.CalendarComponent) } ,
]