import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Feature
import { WorkflowDetailsComponent } from './c/workflow-details/workflow-details.component';
import { SearchWorkflowsComponent } from './c/search-workflows/search-workflows.component';

@NgModule({
  declarations: [
    WorkflowDetailsComponent,
    SearchWorkflowsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class WorkflowsModule { }
