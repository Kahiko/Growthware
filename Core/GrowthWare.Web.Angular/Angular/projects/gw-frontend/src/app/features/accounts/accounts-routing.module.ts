import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/Lib/src/lib/guards/auth.guard';
import { AccountDetailsComponent, ChangePasswordComponent, LoginComponent } from '@Growthware/Lib/src/lib/features/account';
import { LogoutComponent, SearchAccountsComponent } from '@Growthware/Lib/src/lib/features/account';

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
