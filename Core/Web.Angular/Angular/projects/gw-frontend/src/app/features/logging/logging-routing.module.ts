import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/Lib/src/lib/guards';
// Feature
import { SetLogLevelComponent } from '@Growthware/Lib/src/lib/features/logging/c/set-log-level/set-log-level.component';

const childRoutes: Routes = [
  { path: '', component: SetLogLevelComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class LoggingRoutingModule { }
