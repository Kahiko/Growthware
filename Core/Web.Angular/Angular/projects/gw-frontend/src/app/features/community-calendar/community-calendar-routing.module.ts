import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library MISC
// Library Components
import { CalendarComponent } from '@Growthware/Lib/src/features/community-calendar';

const childRoutes: Routes = [
  // { path: '', component: CalendarComponent, canActivate: [AuthGuard]},
  { path: '', component: CalendarComponent},
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class CommunityCalendarRoutingModule { }
