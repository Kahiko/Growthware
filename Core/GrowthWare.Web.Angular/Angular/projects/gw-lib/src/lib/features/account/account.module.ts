import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
// Library
import { DynamicTableModule } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { PickListModule } from '@Growthware/Lib/src/lib/features/pick-list';
// Feature Components
import { AccountDetailsComponent } from './c/account-details/account-details.component';
import { SearchAccountsComponent } from './c/search-accounts/search-accounts.component';
import { LoginComponent } from './c/login/login.component';

@NgModule({
  declarations: [
    AccountDetailsComponent,
    SearchAccountsComponent,
    LoginComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    MatButtonModule,
    MatTabsModule,
    PickListModule,
  ],
  exports: [
    AccountDetailsComponent,
    SearchAccountsComponent
  ]
})
export class AccountModule { }
