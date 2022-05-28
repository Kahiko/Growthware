import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { DynamicTableComponent } from './features/dynamic-table/dynamic-table.component';
import { PagerComponent } from './features/pager/pager.component';

@NgModule({
  declarations: [
    DynamicTableComponent,
    PagerComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  exports: [
    DynamicTableComponent,
    PagerComponent,
  ]
})
export class GWLibModule { }
