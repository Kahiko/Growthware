import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const roleRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/search-roles/search-roles.component').then(m => m.SearchRolesComponent), canActivate: [AuthGuard] },
];