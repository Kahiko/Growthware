import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/src/common-code';
// Feature
import { SearchStatesComponent } from '@Growthware/src/features/states/c/search-states/search-states.component';

const childRoutes: Routes = [
  { path: '', component: SearchStatesComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class StatesRoutingModule { }
