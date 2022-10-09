import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SystemRoutingModule } from './system-routing.module';
import { SystemLayoutComponent } from './system-layout/system-layout.component';
import { SystemFooterComponent } from './system-footer/system-footer.component';
import { SystemHeaderComponent } from './system-header/system-header.component';


@NgModule({
  declarations: [
    SystemLayoutComponent,
    SystemFooterComponent,
    SystemHeaderComponent,
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
