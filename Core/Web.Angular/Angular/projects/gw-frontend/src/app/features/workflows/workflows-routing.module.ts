import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/src/common-code';
// Feature
import { SearchWorkflowsComponent } from '@Growthware/src/features/workflows/c/search-workflows/search-workflows.component';

const childRoutes: Routes = [
  { path: '', component: SearchWorkflowsComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class WorkflowsRoutingModule { }
