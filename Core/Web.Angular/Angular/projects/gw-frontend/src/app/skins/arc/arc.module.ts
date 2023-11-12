import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArcFooterComponent } from './arc-footer/arc-footer.component';
import { ArcHeaderComponent } from './arc-header/arc-header.component';
import { ArcLayoutComponent } from './arc-layout/arc-layout.component';



@NgModule({
  declarations: [
    ArcFooterComponent,
    ArcHeaderComponent,
    ArcLayoutComponent
  ],
  imports: [
    CommonModule
  ]
})
export class ArcModule { }
