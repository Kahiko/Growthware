import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
import { PickListModule } from '@Growthware/features/pick-list';
// Feature
import { SearchfunctionsComponent } from './c/searchfunctions/searchfunctions.component';
import { CopyFunctionSecurityComponent } from './c/copy-function-security/copy-function-security.component';

@NgModule({
  declarations: [
    SearchfunctionsComponent,
    CopyFunctionSecurityComponent,
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    PickListModule
  ]
})
export class FunctionModule { }
