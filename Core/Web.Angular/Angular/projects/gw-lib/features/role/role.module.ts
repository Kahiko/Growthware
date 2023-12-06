import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
import { PickListModule } from '@Growthware/features/pick-list';
// Feature Components
import { SearchRolesComponent } from './c/search-roles/search-roles.component';


@NgModule({
  declarations: [
    SearchRolesComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    PickListModule
  ]
})
export class RoleModule { }
