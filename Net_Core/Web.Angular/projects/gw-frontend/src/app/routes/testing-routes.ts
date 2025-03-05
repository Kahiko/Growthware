import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { TestLoggingComponent } from '@growthware/core/testing';
import { TestModalComponent } from '@growthware/core/testing';

export const testingRoutes: Routes = [
    { path: 'logging', component: TestLoggingComponent, canActivate: [AuthGuard] },
    { path: 'modal', component: TestModalComponent, canActivate: [AuthGuard] }
];