import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library Modules
import { DynamicTableModule } from '@Growthware/Lib';

// Feature
import { AccountsRoutingModule } from './accounts-routing.module';

import { AccountProfileComponent } from './account-profile/account-profile.component';
import { SearchAccountsComponent } from './search-accounts/search-accounts.component';

@NgModule({
  declarations: [
    AccountProfileComponent,
    SearchAccountsComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    AccountsRoutingModule,
  ]
})
export class AccountsModule { }
