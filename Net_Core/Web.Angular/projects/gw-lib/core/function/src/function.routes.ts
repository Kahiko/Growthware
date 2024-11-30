import { Routes } from '@angular/router';

export const functionRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/searchfunctions/searchfunctions.component').then(m => m.SearchfunctionsComponent) },
    { path: 'copyfunctionsecurity', loadComponent: () => import('./c/copy-function-security/copy-function-security.component').then(m => m.CopyFunctionSecurityComponent) },
];