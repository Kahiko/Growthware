import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleDetailsComponent } from './c/role-details/role-details.component';
import { SearchRolesComponent } from './c/search-roles/search-roles.component';



@NgModule({
  declarations: [
    RoleDetailsComponent,
    SearchRolesComponent
  ],
  imports: [
    CommonModule
  ]
})
export class RoleModule { }
