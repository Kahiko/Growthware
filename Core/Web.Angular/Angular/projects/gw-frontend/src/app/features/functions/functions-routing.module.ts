import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { SearchfunctionsComponent } from '@growthware/core/function';
import { CopyFunctionSecurityComponent } from '@growthware/core/function';

const childRoutes: Routes = [
	{ path: '', component: SearchfunctionsComponent },
	{ path: 'copyfunctionsecurity', component: CopyFunctionSecurityComponent },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class FunctionsRoutingModule { }
