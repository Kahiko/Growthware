import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
// import { SearchRolesComponent } from '@growthware/core/role';

const childRoutes: Routes = [
	// { path: '', component: SearchRolesComponent},
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class RolesRoutingModule { }
