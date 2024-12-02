import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const statesRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/search-states/search-states.component').then(m => m.SearchStatesComponent), canActivate: [AuthGuard] },
    // { path: 'selectasecurityentity', loadComponent: () => import('./c/select-security-entity/select-security-entity.component').then(m => m.SelectSecurityEntityComponent), canActivate: [AuthGuard] },
];