import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular material
import { MatIconModule } from '@angular/material/icon';
// Feature
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
