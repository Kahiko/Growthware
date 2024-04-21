import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
// Feature
import { SearchStatesComponent } from '@growthware/core/states';

const childRoutes: Routes = [
	{ path: '', component: SearchStatesComponent, canActivate: [AuthGuard] },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class StatesRoutingModule { }
