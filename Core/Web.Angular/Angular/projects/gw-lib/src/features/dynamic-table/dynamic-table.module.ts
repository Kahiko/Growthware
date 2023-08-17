import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Third Party
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
// Library
import { PagerModule } from '@Growthware/src/features/pager';
// Our Components/Modules
import { DynamicTableComponent } from './c/dynamic-table/dynamic-table.component';

@NgModule({
  declarations: [
    DynamicTableComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCheckboxModule,
    PagerModule
  ],
  exports: [
    DynamicTableComponent
  ]
})
export class DynamicTableModule { }
