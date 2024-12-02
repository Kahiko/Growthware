import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const secutiryRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/encrypt-decrypt/encrypt-decrypt.component').then(m => m.EncryptDecryptComponent), canActivate: [AuthGuard] },
	{ path: 'guid_helper', loadComponent: () => import('./c/guid-helper/guid-helper.component').then(m => m.GuidHelperComponent) },             // /security/guid_helper
	{ path: 'random-numbers', loadComponent: () => import('./c/random-numbers/random-numbers.component').then(m => m.RandomNumbersComponent) }, // /security/random_numbers

];