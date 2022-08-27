import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DynamicTableModule } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { PickListModule } from '@Growthware/Lib/src/lib/features/pick-list';

import { FunctionDetailsComponent } from './c/function-details/function-details.component';
import { SearchfunctionsComponent } from './c/searchfunctions/searchfunctions.component';



@NgModule({
  declarations: [
    FunctionDetailsComponent,
    SearchfunctionsComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    PickListModule
  ]
})
export class FunctionModule { }
