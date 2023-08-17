import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Feature
import { NameValuePairDetailsComponent } from './c/name-value-pair-details/name-value-pair-details.component';
import { SearchNameValuePairsComponent } from './c/search-name-value-pairs/search-name-value-pairs.component';
import { SearchNameValuePairDetailsComponent } from './c/search-name-value-pair-details/search-name-value-pair-details.component';


@NgModule({
  declarations: [
    NameValuePairDetailsComponent,
    SearchNameValuePairsComponent,
    SearchNameValuePairDetailsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class NameValuePairModule { }
