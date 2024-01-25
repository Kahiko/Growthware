import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FileManagerComponent } from '@growthware/core/file-manager';

const childRoutes: Routes = [
	{ path: '', component: FileManagerComponent},
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class FileManagerRoutingModule { }
