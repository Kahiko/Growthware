import { Routes } from '@angular/router';
// Feature

export const fileManagerRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/file-manager/file-manager.component').then(m => m.FileManagerComponent) },
];