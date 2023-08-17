import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/src/common-code';
// Feature
import { SearchSecurityEntitiesComponent } from '@Growthware/src/features/security-entities/c/search-security-entities/search-security-entities.component';

const childRoutes: Routes = [
  { path: '', component: SearchSecurityEntitiesComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class SecurityEntitiesRoutingModule { }
