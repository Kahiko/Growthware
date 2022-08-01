import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'edit-account', loadChildren: () => import('./account-profile/account-profile.component').then(m => m.AccountProfileComponent) },
  { path: 'edit-my-account', loadChildren: () => import('./account-profile/account-profile.component').then(m => m.AccountProfileComponent) },
  { path: 'search-accounts', loadChildren: () => import('./search-accounts/search-accounts.module').then(m => m.SearchAccountsModule) }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
