import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/common-code';
// Feature
import { LineCountComponent } from '@Growthware/features/sys-admin';
import { EditDbInformationComponent } from '@Growthware/features/sys-admin';

const childRoutes: Routes = [
  { path: 'linecount', component: LineCountComponent, canActivate: [AuthGuard] },
  { path: 'editdbinformation', component: EditDbInformationComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class SysAdminRoutingModule { }
