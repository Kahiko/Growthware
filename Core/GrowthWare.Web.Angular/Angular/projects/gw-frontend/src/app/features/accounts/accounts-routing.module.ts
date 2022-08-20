import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccountDetailsComponent, LoginComponent, SearchAccountsComponent } from '@Growthware/Lib';

const routes: Routes = [
  { path: '', component: SearchAccountsComponent},
  { path: 'edit-account', component: AccountDetailsComponent },
  { path: 'edit-my-account', component: AccountDetailsComponent },
  { path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
