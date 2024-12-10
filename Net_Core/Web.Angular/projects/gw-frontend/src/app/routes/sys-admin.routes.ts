import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { EditDbInformationComponent } from '@growthware/core/configuration';
import { LineCountComponent, NaturalSortComponent } from '@growthware/core/sys-admin';

export const sysAdminRoutes: Routes = [
    // update-session really isn't needed because of the way the angular 2.x and > work but left it in for now
    // { path: '', loadComponent: () => import('./c/update-session/update-session.component').then(m => m.UpdateSessionComponent) , canActivate: [AuthGuard]},
    { path: '', component: NaturalSortComponent, canActivate: [AuthGuard]},
	{ path: 'linecount', component: LineCountComponent, canActivate: [AuthGuard] },
	{ path: 'editdbinformation', component: EditDbInformationComponent   , canActivate: [AuthGuard] },
];