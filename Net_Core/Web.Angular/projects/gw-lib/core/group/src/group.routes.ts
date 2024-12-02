import { Routes } from '@angular/router';

export const groupRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/search-groups/search-groups.component').then(m => m.SearchGroupsComponent) },  
];