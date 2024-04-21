import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
// Feature
import { SearchWorkflowsComponent } from '@growthware/core/workflows';

const childRoutes: Routes = [
	{ path: '', component: SearchWorkflowsComponent, canActivate: [AuthGuard] },
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class WorkflowsRoutingModule { }
