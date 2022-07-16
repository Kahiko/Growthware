import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Third Party
import { MatButtonModule } from '@angular/material/button';
// Our Components/Modules
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
    MatButtonModule,
    PagerModule
  ],
  exports: [
    DynamicTableComponent
  ]
})
export class DynamicTableModule { }
