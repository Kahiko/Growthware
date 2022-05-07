import { NgModule } from '@angular/core';
import { DynamicTableComponent } from './features/dynamic-table/dynamic-table.component';
import { SearchComponent } from './features/search/search.component';

@NgModule({
  declarations: [
    DynamicTableComponent,
    SearchComponent,
  ],
  imports: [
  ],
  exports: [
    DynamicTableComponent,
  ]
})
export class GwLibModule { }
