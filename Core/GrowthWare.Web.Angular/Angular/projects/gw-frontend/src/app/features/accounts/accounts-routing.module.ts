import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccountProfileComponent } from './account-profile/account-profile.component';
import { SearchAccountsComponent } from './search-accounts/search-accounts.component';

const routes: Routes = [
  { path: '', component: SearchAccountsComponent },
  { path: 'edit-account', component: AccountProfileComponent },
  { path: 'edit-my-account', component: AccountProfileComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
