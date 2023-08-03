import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastComponent } from './c/toast/toast.component';
import { ToasterComponent } from './c/toaster/toaster.component';


@NgModule({
  declarations: [
    ToastComponent,
    ToasterComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    ToasterComponent
  ]
})
export class ToastModule { }
