import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleDetailsComponent } from './c/role-details/role-details.component';
import { SearchrolesComponent } from './c/searchroles/searchroles.component';



@NgModule({
  declarations: [
    RoleDetailsComponent,
    SearchrolesComponent
  ],
  imports: [
    CommonModule
  ]
})
export class RoleModule { }
