import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/Lib/src/lib/guards';
// Feature
import { SearchNameValuePairsComponent } from '@Growthware/Lib/src/lib/features/name-value-pair/c/search-name-value-pairs/search-name-value-pairs.component';

const childRoutes: Routes = [
  { path: '', component: SearchNameValuePairsComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class NameValuePairsRoutingModule { }
