import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PagerComponent } from './c/pager/pager.component';

@NgModule({
  declarations: [
    PagerComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    PagerComponent
  ]
})
export class PagerModule { }