import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { SearchGroupsComponent } from '@Growthware/features/group';

const childRoutes: Routes = [
  { path: '', component: SearchGroupsComponent},
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class GroupsRoutingModule { }
