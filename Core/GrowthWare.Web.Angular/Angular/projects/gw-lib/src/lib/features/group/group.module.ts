import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupDetailsComponent } from './c/group-details/group-details.component';
import { SearchgroupsComponent } from './c/searchgroups/searchgroups.component';



@NgModule({
  declarations: [
    GroupDetailsComponent,
    SearchgroupsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class GroupModule { }
