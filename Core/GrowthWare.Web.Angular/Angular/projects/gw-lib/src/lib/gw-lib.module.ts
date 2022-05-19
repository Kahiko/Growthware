import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { GWLibDynamicTableComponent } from './features/dynamic-table/dynamic-table.component';
import { GWLibPagerComponent } from './features/pager/pager.component';

@NgModule({
  declarations: [
    GWLibDynamicTableComponent,
    GWLibPagerComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  exports: [
    GWLibDynamicTableComponent,
    GWLibPagerComponent,
  ]
})
export class GwLibModule { }
