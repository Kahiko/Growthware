import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';

export const accountRoutes: Routes = [
    { path: '', loadComponent: () => import('./c/search-accounts/search-accounts.component').then(m => m.SearchAccountsComponent), canActivate: [AuthGuard]},
	{ path: 'edit-account',  loadComponent: () => import('./c/account-details/account-details.component').then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
	{ path: 'edit-my-account', loadComponent: () => import('./c/account-details/account-details.component').then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
	{ path: 'register', loadComponent: () => import('./c/account-details/account-details.component').then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
	{ path: 'selectpreferences', loadComponent: () => import('./c/select-preferences/select-preferences.component').then(m => m.SelectPreferencesComponent), canActivate: [AuthGuard] },
	// { path: 'updateanonymousprofile', loadComponent: () => import('./c/update-anonymous-profile/update-anonymous-profile.component').then(m => m.UpdateAnonymousProfileComponent), canActivate: [AuthGuard] },
	{ path: 'logon', loadComponent: () => import('./c/login/login.component').then(m => m.LoginComponent) },
	{ path: 'change-password', loadComponent: () => import('./c/change-password/change-password.component').then(m => m.ChangePasswordComponent) },
	{ path: 'reset-password', loadComponent: () => import('./c/change-password/change-password.component').then(m => m.ChangePasswordComponent) },
	{ path: 'logout', loadComponent: () => import('./c/logout/logout.component').then(m => m.LogoutComponent) },
	{ path: 'forgot-password', loadComponent: () => import('./c/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) },
	{ path: 'verify-account', loadComponent: () => import('./c/verify-account/verify-account.component').then(m => m.VerifyAccountComponent) },
];