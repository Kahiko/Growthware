import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupDetailsComponent } from './c/group-details/group-details.component';
import { SearchGroupsComponent } from './c/search-groups/search-groups.component';



@NgModule({
  declarations: [
    GroupDetailsComponent,
    SearchGroupsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class GroupModule { }
