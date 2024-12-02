import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const messagesRoutes: Routes = [
	{ path: '', loadComponent: () => import('./c/search-messages/search-messages.component').then(m => m.SearchMessagesComponent), canActivate: [AuthGuard] },
];