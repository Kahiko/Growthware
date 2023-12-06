import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchGroupsComponent } from './c/search-groups/search-groups.component';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
import { PickListModule } from '@Growthware/features/pick-list';


@NgModule({
  declarations: [
    SearchGroupsComponent,
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    PickListModule
  ]
})
export class GroupModule { }
