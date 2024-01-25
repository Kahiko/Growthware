import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './c/home/home.component';
import { GenericHomeComponent } from './c/generic-home/generic-home.component';

const childRoutes: Routes = [
	{ path: '', component: GenericHomeComponent },
	{ path: 'generic_home', component: GenericHomeComponent },
	{ path: 'home', component: HomeComponent },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class HomeRoutingModule { }
