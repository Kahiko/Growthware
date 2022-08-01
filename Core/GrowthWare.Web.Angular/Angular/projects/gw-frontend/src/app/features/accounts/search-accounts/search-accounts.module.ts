import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library Modules
import { DynamicTableModule } from '@Growthware/Lib';
// Feature Modules/Components
import { SearchAccountsRoutingModule } from './search-accounts-routing.module';
import { SearchAccountsComponent } from './search-accounts.component';


@NgModule({
  declarations: [
    SearchAccountsComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    SearchAccountsRoutingModule
  ]
})
export class SearchAccountsModule { }
