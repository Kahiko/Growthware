import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const sysAdminRoutes: Routes = [
    // update-session really isn't needed because of the way the angular 2.x and > work but left it in for now
    // { path: '', loadComponent: () => import('./c/update-session/update-session.component').then(m => m.UpdateSessionComponent) , canActivate: [AuthGuard]},
    { path: '', loadComponent: () => import('./c/natural-sort/natural-sort.component').then(m => m.NaturalSortComponent) , canActivate: [AuthGuard]},
	{ path: 'linecount', loadComponent: () => import('./c/line-count/line-count.component').then(m => m.LineCountComponent) , canActivate: [AuthGuard] },
	{ path: 'editdbinformation', loadComponent: () => import('@growthware/core/configuration').then(m => m.EditDbInformationComponent)   , canActivate: [AuthGuard] },
];