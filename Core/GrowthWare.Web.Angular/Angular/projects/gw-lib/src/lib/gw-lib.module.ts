import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { GWLibDynamicTableComponent } from './features/dynamic-table/dynamic-table.component';

@NgModule({
  declarations: [
    GWLibDynamicTableComponent,
  ],
  imports: [
    BrowserModule
  ],
  exports: [
    GWLibDynamicTableComponent,
  ]
})
export class GwLibModule { }
