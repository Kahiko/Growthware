import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
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
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatTabsModule,
    PickListModule,
    ReactiveFormsModule,
  ],
  exports: [
    AccountDetailsComponent,
    SearchAccountsComponent
  ]
})
export class AccountModule { }
