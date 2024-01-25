import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
// Feature
import { LineCountComponent } from '@growthware/core/sys-admin';
import { EditDbInformationComponent } from '@growthware/core/configuration';

const childRoutes: Routes = [
	{ path: 'linecount', component: LineCountComponent, canActivate: [AuthGuard] },
	{ path: 'editdbinformation', component: EditDbInformationComponent, canActivate: [AuthGuard] },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class SysAdminRoutingModule { }
