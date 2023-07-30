import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/Lib/src/lib/guards';
// Feature
import { SearchSecurityEntitiesComponent } from '@Growthware/Lib/src/lib/features/security-entities/c/search-security-entities/search-security-entities.component';

const childRoutes: Routes = [
  { path: '', component: SearchSecurityEntitiesComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class SecurityEntitiesRoutingModule { }
