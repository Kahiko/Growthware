import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { SearchWorkflowsComponent } from '@growthware/core/workflows';

export const workflowRoutes: Routes = [
	{ path: '', component: SearchWorkflowsComponent , canActivate: [AuthGuard]},
];