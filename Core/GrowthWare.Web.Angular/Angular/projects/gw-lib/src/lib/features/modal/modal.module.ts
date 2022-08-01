import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from './c/modal.component';



@NgModule({
  declarations: [
    ModalComponent
  ],
  imports: [
    // BrowserModule,
    CommonModule,
    FormsModule
  ],
  exports: [
    ModalComponent
  ]
})
export class ModalModule { }
