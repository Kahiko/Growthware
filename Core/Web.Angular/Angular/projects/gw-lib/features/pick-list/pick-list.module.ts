import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Feature
import { PickListComponent } from './c/pick-list/pick-list.component';

@NgModule({
  declarations: [
    PickListComponent
  ],
  imports: [
    CommonModule,

    MatButtonModule,
    MatIconModule
  ],
  exports: [
    PickListComponent
  ]
})
export class PickListModule { }
