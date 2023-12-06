import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/common-code';
// Feature
import { SetLogLevelComponent } from '@Growthware/features/logging/c/set-log-level/set-log-level.component';

const childRoutes: Routes = [
  { path: '', component: SetLogLevelComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class LoggingRoutingModule { }
