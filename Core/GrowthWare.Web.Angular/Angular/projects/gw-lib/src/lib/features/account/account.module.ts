import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountDetailsComponent } from './c/account-details/account-details.component';
import { SearchAccountsComponent } from './c/search-accounts/search-accounts.component';

import { DynamicTableModule } from '@Growthware/Lib/src/lib/features/dynamic-table';

@NgModule({
  declarations: [
    AccountDetailsComponent,
    SearchAccountsComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
  ],
  exports: [
    AccountDetailsComponent,
    SearchAccountsComponent
  ]
})
export class AccountModule { }
