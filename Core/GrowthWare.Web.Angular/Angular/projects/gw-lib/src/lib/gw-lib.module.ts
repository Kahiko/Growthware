import { NgModule } from '@angular/core';
import { GwLibComponent } from './gw-lib.component';
import { DynamicTableComponent } from './features/dynamic-table/dynamic-table.component';



@NgModule({
  declarations: [
    GwLibComponent,
    DynamicTableComponent
  ],
  imports: [
  ],
  exports: [
    GwLibComponent
  ]
})
export class GwLibModule { }
