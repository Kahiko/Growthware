import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/common-code';
// Feature
import { ManageNameValuePairsComponent } from '@Growthware/features/name-value-pair/c/manage-name-value-pairs/manage-name-value-pairs.component';

const childRoutes: Routes = [
  { path: '', component: ManageNameValuePairsComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class NameValuePairsRoutingModule { }
