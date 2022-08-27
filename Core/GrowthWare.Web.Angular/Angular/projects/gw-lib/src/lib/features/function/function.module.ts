import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FunctionDetailsComponent } from './c/function-details/function-details.component';
import { SearchfunctionsComponent } from './c/searchfunctions/searchfunctions.component';



@NgModule({
  declarations: [
    FunctionDetailsComponent,
    SearchfunctionsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class FunctionModule { }
