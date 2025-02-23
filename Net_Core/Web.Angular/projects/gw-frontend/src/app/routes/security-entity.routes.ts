import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { SearchSecurityEntitiesComponent, SelectSecurityEntityComponent } from '@growthware/core/security-entities';

export const securityEntityRoutes: Routes = [
	{ path: '', component: SearchSecurityEntitiesComponent, canActivate: [AuthGuard] },
	{ path: 'selectasecurityentity', component: SelectSecurityEntityComponent, canActivate: [AuthGuard] },
];