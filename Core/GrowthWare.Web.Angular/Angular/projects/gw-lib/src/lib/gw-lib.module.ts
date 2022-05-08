import { NgModule } from '@angular/core';
import { GWLibDynamicTableComponent } from './features/dynamic-table/dynamic-table.component';
import { GWLibSearchComponent } from './features/search/search.component';

@NgModule({
  declarations: [
    GWLibDynamicTableComponent,
    GWLibSearchComponent,
  ],
  imports: [
  ],
  exports: [
    GWLibDynamicTableComponent,
    GWLibSearchComponent,
  ]
})
export class GwLibModule { }
