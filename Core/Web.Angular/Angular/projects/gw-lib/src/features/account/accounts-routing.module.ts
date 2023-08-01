import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/Lib/src/guards';
// Feature
import { AccountDetailsComponent } from './c/account-details/account-details.component';
import { ChangePasswordComponent } from './c/change-password/change-password.component';
import { LoginComponent } from './c/login/login.component';
import { LogoutComponent } from './c/logout/logout.component';
import { SearchAccountsComponent } from './c/search-accounts/search-accounts.component';

const childRoutes: Routes = [
  { path: '', component: SearchAccountsComponent, canActivate: [AuthGuard]},
  { path: 'edit-account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
  { path: 'edit-my-account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent},
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'logout', component: LogoutComponent },
];


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
