import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AccountDetailsComponent, LoginComponent, SearchAccountsComponent } from '@Growthware/Lib/src/lib/features/account';

const childRoutes: Routes = [
  { path: 'search-accounts', component: SearchAccountsComponent},
  { path: 'edit-account', component: AccountDetailsComponent },
  { path: 'edit-my-account', component: AccountDetailsComponent },
  { path: 'login', component: LoginComponent}
];


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
