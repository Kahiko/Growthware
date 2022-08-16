import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { DynamicTableModule } from '@Growthware/Lib/src/lib/features/dynamic-table';
// Feature Components
import { AccountDetailsComponent } from './c/account-details/account-details.component';
import { SearchAccountsComponent } from './c/search-accounts/search-accounts.component';

@NgModule({
  declarations: [
    AccountDetailsComponent,
    SearchAccountsComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    MatTabsModule,
  ],
  exports: [
    AccountDetailsComponent,
    SearchAccountsComponent
  ]
})
export class AccountModule { }
