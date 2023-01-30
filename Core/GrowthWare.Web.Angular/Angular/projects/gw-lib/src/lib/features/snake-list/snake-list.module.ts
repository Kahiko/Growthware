import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SnakeListComponent } from './c/snake-list/snake-list.component';



@NgModule({
  declarations: [
    SnakeListComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    SnakeListComponent
  ]
})
export class SnakeListModule { }
