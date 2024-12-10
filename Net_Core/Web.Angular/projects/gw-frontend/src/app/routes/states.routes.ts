import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { SearchStatesComponent } from '@growthware/core/states';

export const statesRoutes: Routes = [
    { path: '', component: SearchStatesComponent, canActivate: [AuthGuard] },
    // { path: 'selectasecurityentity', loadComponent: () => import('./c/select-security-entity/select-security-entity.component').then(m => m.SelectSecurityEntityComponent), canActivate: [AuthGuard] },
];