import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SystemRoutingModule } from './system-routing.module';
import { SystemFooterComponent } from './system-footer/system-footer.component';
import { SystemHeaderComponent } from './system-header/system-header.component';
import { SystemLayoutComponent } from './system-layout/system-layout.component';


@NgModule({
  declarations: [
    SystemFooterComponent,
    SystemHeaderComponent,
    SystemLayoutComponent
  ],
  imports: [
    CommonModule,
    SystemRoutingModule
  ],
  exports: [
    SystemLayoutComponent
  ]
})
export class SystemModule { }
