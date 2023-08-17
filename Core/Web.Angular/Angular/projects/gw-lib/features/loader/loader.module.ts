import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
// Feature
import { LoaderComponent } from './c/loader/loader.component';

@NgModule({
  declarations: [
    LoaderComponent
  ],
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
  ],
  exports: [
    LoaderComponent,
  ]
})
export class LoaderModule { }
