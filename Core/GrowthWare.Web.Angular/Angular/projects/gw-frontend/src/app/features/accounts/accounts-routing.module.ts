import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccountDetailsComponent } from '@Growthware/Lib';
import { SearchAccountsComponent } from './search-accounts/search-accounts.component';

const routes: Routes = [
  { path: '', component: SearchAccountsComponent },
  { path: 'edit-my-account', component: AccountDetailsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
