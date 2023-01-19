import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AccountDetailsComponent, ChangePasswordComponent, LoginComponent } from '@Growthware/Lib/src/lib/features/account';
import { LogoutComponent, SearchAccountsComponent } from '@Growthware/Lib/src/lib/features/account';

const childRoutes: Routes = [
  { path: '', component: SearchAccountsComponent},
  { path: 'edit-account', component: AccountDetailsComponent },
  { path: 'edit-my-account', component: AccountDetailsComponent },
  { path: 'login', component: LoginComponent},
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'logout', component: LogoutComponent },
];


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
