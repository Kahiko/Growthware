import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library Modules
import { AccountModule } from '@Growthware/Lib';

// Feature
import { AccountsRoutingModule } from './accounts-routing.module';
import { ListAccountsComponent } from './list-accounts/list-accounts.component';

@NgModule({
  declarations: [
    ListAccountsComponent,
  ],
  imports: [
    CommonModule,
    AccountModule,
    AccountsRoutingModule,
  ]
})
export class AccountsModule { }
