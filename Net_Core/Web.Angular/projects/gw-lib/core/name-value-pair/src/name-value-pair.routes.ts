import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const nameValuePairRoutes: Routes = [
	{ path: '', loadComponent: () => import('./c/manage-name-value-pairs/manage-name-value-pairs.component').then(m => m.ManageNameValuePairsComponent), canActivate: [AuthGuard] },
];