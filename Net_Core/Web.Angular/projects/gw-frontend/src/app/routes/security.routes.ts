import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';
import { EncryptDecryptComponent, GuidHelperComponent, RandomNumbersComponent } from '@growthware/core/security';

export const secutiryRoutes: Routes = [
	{ path: '', component: EncryptDecryptComponent, canActivate: [AuthGuard] },
	{ path: 'guid_helper', component: GuidHelperComponent },             // /security/guid_helper
	{ path: 'random-numbers', component: RandomNumbersComponent }, // /security/random_numbers

];