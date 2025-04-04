import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { SearchRolesComponent } from '@growthware/core/role';

export const roleRoutes: Routes = [
	{ path: '', component: SearchRolesComponent, canActivate: [AuthGuard] },
];