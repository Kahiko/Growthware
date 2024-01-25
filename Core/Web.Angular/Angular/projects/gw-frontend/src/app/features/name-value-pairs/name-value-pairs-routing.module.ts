import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
// Feature
import { ManageNameValuePairsComponent } from '@growthware/core/name-value-pair';

const childRoutes: Routes = [
	{ path: '', component: ManageNameValuePairsComponent, canActivate: [AuthGuard] },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class NameValuePairsRoutingModule { }
