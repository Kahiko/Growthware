import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
// Feature
import { StateDetailsComponent } from './c/state-details/state-details.component';
import { SearchStatesComponent } from './c/search-states/search-states.component';

@NgModule({
  declarations: [
    StateDetailsComponent,
    SearchStatesComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule
  ]
})
export class StatesModule { }
