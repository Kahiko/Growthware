import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
// Feature
import { SetLogLevelComponent } from '@growthware/core/configuration';

const childRoutes: Routes = [
	{ path: '', component: SetLogLevelComponent, canActivate: [AuthGuard] },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class LoggingRoutingModule { }
