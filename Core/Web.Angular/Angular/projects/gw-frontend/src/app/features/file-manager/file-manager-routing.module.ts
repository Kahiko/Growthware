import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { FileManagerComponent } from '@Growthware/src/features/file-manager/c/file-manager/file-manager.component';
import { FileManagerComponent } from '@Growthware/src/features/file-manager';

const childRoutes: Routes = [
  { path: '', component: FileManagerComponent},
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class FileManagerRoutingModule { }
