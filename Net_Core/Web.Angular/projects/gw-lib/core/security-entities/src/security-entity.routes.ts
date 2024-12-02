import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const securityEntityRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/search-security-entities/search-security-entities.component').then(m => m.SearchSecurityEntitiesComponent), canActivate: [AuthGuard] },
    { path: 'selectasecurityentity', loadComponent: () => import('./c/select-security-entity/select-security-entity.component').then(m => m.SelectSecurityEntityComponent), canActivate: [AuthGuard] },
];