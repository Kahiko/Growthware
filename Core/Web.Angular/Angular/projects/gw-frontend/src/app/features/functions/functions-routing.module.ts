import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { SearchfunctionsComponent } from '@Growthware/Lib/src/lib/features/function';

const childRoutes: Routes = [
  { path: '', component: SearchfunctionsComponent},
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class FunctionsRoutingModule { }
