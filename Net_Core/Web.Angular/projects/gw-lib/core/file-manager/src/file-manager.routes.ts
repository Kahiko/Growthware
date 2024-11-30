import { Routes } from '@angular/router';

export const fileManagerRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/file-manager/file-manager.component').then(m => m.FileManagerComponent) },
];