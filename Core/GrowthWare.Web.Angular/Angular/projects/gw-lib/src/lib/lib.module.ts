import { NgModule } from '@angular/core';
import { GWLibComponent } from './features/gw-lib.component';
import { DynamicTableComponent } from './features/dynamic-table/dynamic-table.component';

@NgModule({
  declarations: [
    GWLibComponent,
    DynamicTableComponent,
  ],
  imports: [
  ],
  exports: [
    GWLibComponent
  ]
})
export class GWLibModule { }
