import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
// Feature
// import { SearchSecurityEntitiesComponent } from '@growthware/core/security-entities';

const childRoutes: Routes = [
	// { path: '', component: SearchSecurityEntitiesComponent, canActivate: [AuthGuard] },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class SecurityEntitiesRoutingModule { }
