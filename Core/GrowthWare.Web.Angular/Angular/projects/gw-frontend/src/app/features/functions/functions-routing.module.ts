import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchfunctionsComponent } from '@Growthware/Lib';

const routes: Routes = [
  { path: '', component: SearchfunctionsComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FunctionsRoutingModule { }
