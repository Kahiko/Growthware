import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library Modules
import { DynamicTableModule } from '@Growthware/Lib';
import { AccountModule } from '@Growthware/Lib';

// Feature
import { AccountsRoutingModule } from './accounts-routing.module';
import { SearchAccountsComponent } from './search-accounts/search-accounts.component';

@NgModule({
  declarations: [
    SearchAccountsComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    AccountModule,
    AccountsRoutingModule,
  ]
})
export class AccountsModule { }
