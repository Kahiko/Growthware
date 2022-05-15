import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { GWLibDynamicTableComponent } from './features/dynamic-table/dynamic-table.component';
import { GWLibPagerComponent } from './features/pager/pager.component';

@NgModule({
  declarations: [
    GWLibDynamicTableComponent,
    GWLibPagerComponent,
  ],
  imports: [
    BrowserModule
  ],
  exports: [
    GWLibDynamicTableComponent,
    GWLibPagerComponent,
  ]
})
export class GwLibModule { }
