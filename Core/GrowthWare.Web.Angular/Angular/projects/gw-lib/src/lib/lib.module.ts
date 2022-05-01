import { NgModule } from '@angular/core';
import { GWLibComponent } from './components/gw-lib.component';
import { DynamicTableComponent } from './components/dynamic-table/dynamic-table.component';

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
