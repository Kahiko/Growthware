import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Third Party
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';

// Our Components/Modules
import { DynamicTableComponent } from './c/dynamic-table.component';
import { PagerModule } from '@Growthware/Lib/src/features/pager';

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
