import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const loggingRoutes: Routes = [
	{ path: 'setloglevel', loadComponent: () => import('./c/set-log-level/set-log-level.component').then(m => m.SetLogLevelComponent), canActivate: [AuthGuard] },
	{ path: 'test-logging', loadComponent: () => import('./c/test-logging/test-logging.component').then(m => m.TestLoggingComponent)  },
];