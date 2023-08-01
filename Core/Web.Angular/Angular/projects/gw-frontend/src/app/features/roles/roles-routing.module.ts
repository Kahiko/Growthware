import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { SearchRolesComponent } from '@Growthware/Lib/src/features/role';

const childRoutes: Routes = [
  { path: '', component: SearchRolesComponent},
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class RolesRoutingModule { }
