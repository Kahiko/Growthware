
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library MISC
import { AuthGuard } from '@growthware/common/services';

const mAccountPath = '@growthware/core/account';
const childRoutes: Routes = [
	{ path: '', loadComponent: () => import(mAccountPath).then(m => m.SearchAccountsComponent), canActivate: [AuthGuard]},
	{ path: 'edit-account', loadComponent: () => import(mAccountPath).then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
	{ path: 'edit-my-account', loadComponent: () => import(mAccountPath).then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
	{ path: 'register', loadComponent: () => import(mAccountPath).then(m => m.AccountDetailsComponent) },
	{ path: 'selectpreferences', loadComponent: () => import(mAccountPath).then(m => m.SelectPreferencesComponent), canActivate: [AuthGuard] },
	// { path: 'updateanonymousprofile', loadComponent: () => import(mAccountPath).then(m => m.UpdateAnonymousProfileComponent), canActivate: [AuthGuard] },
	{ path: 'logon', loadComponent: () => import(mAccountPath).then(m => m.LoginComponent) },
	{ path: 'change-password', loadComponent: () => import(mAccountPath).then(m => m.ChangePasswordComponent) },
	{ path: 'reset-password', loadComponent: () => import(mAccountPath).then(m => m.ChangePasswordComponent) },
	{ path: 'logout', loadComponent: () => import(mAccountPath).then(m => m.LogoutComponent) },
	{ path: 'forgot-password', loadComponent: () => import(mAccountPath).then(m => m.ForgotPasswordComponent) },
	{ path: 'verify-account', loadComponent: () => import(mAccountPath).then(m => m.VerifyAccountComponent) },
];


@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class AccountsRoutingModule { }
