import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library MISC
import { AuthGuard } from '@Growthware/common-code';
// Library Components
import { AccountDetailsComponent } from '@Growthware/features/account/c/account-details/account-details.component';
import { ChangePasswordComponent } from '@Growthware/features/account/c/change-password/change-password.component';
import { LoginComponent } from '@Growthware/features/account/c/login/login.component';
import { LogoutComponent } from '@Growthware/features/account/c/logout/logout.component';
import { SearchAccountsComponent } from '@Growthware/features/account/c/search-accounts/search-accounts.component';
import { SelectPreferencesComponent } from '@Growthware/features/account/c/select-preferences/select-preferences.component';
import { UpdateAnonymousProfileComponent } from '@Growthware/features/account/c/update-anonymous-profile/update-anonymous-profile.component';

const childRoutes: Routes = [
  { path: '', component: SearchAccountsComponent, canActivate: [AuthGuard]},
  { path: 'edit-account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
  { path: 'edit-my-account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
  { path: 'selectpreferences', component: SelectPreferencesComponent, canActivate: [AuthGuard] },
  { path: 'updateanonymousprofile', component: UpdateAnonymousProfileComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent},
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'logout', component: LogoutComponent },
];


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
