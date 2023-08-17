import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { SearchfunctionsComponent } from '@Growthware/src/features/function';
import { CopyFunctionSecurityComponent } from '@Growthware/src/features/function';

const childRoutes: Routes = [
  { path: '', component: SearchfunctionsComponent },
  { path: 'copyfunctionsecurity', component: CopyFunctionSecurityComponent },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class FunctionsRoutingModule { }
