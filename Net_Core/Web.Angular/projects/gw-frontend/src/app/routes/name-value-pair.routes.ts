import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { ManageNameValuePairsComponent } from '@growthware/core/name-value-pair';

export const nameValuePairRoutes: Routes = [
	{ path: '', component: ManageNameValuePairsComponent, canActivate: [AuthGuard] },
];