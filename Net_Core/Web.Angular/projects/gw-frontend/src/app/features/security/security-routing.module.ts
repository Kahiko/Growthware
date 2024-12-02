import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
// Feature
// import { EncryptDecryptComponent } from '@growthware/core/security';
// import { GuidHelperComponent } from '@growthware/core/security';
// import { RandomNumbersComponent } from '@growthware/core/security';

const childRoutes: Routes = [
	// { path: '', component: EncryptDecryptComponent, canActivate: [AuthGuard] },
	// { path: 'guid_helper', component: GuidHelperComponent },        // /security/guid_helper
	// { path: 'random-numbers', component: RandomNumbersComponent },  // /security/random_numbers
];


@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class SecurityRoutingModule { }
