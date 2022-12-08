import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupDetailsComponent } from './c/group-details/group-details.component';
import { SearchGroupsComponent } from './c/search-groups/search-groups.component';
// Library
import { DynamicTableModule } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { PickListModule } from '@Growthware/Lib/src/lib/features/pick-list';


@NgModule({
  declarations: [
    GroupDetailsComponent,
    SearchGroupsComponent,
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    PickListModule
  ]
})
export class GroupModule { }
