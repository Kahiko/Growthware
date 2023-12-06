import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
// Feature
import { SearchStatesComponent } from './c/search-states/search-states.component';

@NgModule({
  declarations: [
    SearchStatesComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule
  ]
})
export class StatesModule { }
