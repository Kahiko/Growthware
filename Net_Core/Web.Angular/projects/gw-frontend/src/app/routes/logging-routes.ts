import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { SetLogLevelComponent, TestLoggingComponent } from '@growthware/core/logging';

export const loggingRoutes: Routes = [
	{ path: 'setloglevel', component: SetLogLevelComponent, canActivate: [AuthGuard] },
	{ path: 'test-logging', component: TestLoggingComponent  },
];