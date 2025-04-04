import { Routes } from '@angular/router';

export const homeRoutes: Routes = [
	{ path: '', loadComponent: () => import('./c/generic-home/generic-home.component').then(m => m.GenericHomeComponent) },
	{ path: 'generic_home', loadComponent: () => import('./c/generic-home/generic-home.component').then(m => m.GenericHomeComponent) },
	{ path: 'home', loadComponent: () => import('./c/home/home.component').then(m => m.HomeComponent) },
];
