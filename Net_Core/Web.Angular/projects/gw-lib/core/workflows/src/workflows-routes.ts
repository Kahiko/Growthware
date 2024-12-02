import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const workflowRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/search-workflows/search-workflows.component').then(m => m.SearchWorkflowsComponent) , canActivate: [AuthGuard]},
];