import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DynamicTableComponent } from './c/dynamic-table.component';
import { PagerModule } from '@Growthware/Lib/src/lib/features/pager';

@NgModule({
  declarations: [
    DynamicTableComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    PagerModule
  ],
  exports: [
    DynamicTableComponent
  ]
})
export class DynamicTableModule { }
