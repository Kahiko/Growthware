import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PickListComponent } from './c/pick-list/pick-list.component';



@NgModule({
  declarations: [
    PickListComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PickListComponent
  ]
})
export class PickListModule { }
