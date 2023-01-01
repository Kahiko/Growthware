import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { PickListComponent } from './c/pick-list/pick-list.component';



@NgModule({
  declarations: [
    PickListComponent
  ],
  imports: [
    CommonModule,
    MatIconModule
  ],
  exports: [
    PickListComponent
  ]
})
export class PickListModule { }
