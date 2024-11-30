import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';
// Feature
import { AccountDetailsComponent } from './c/account-details/account-details.component';
import { ForgotPasswordComponent } from './c/forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './c/change-password/change-password.component';
import { LoginComponent } from './c/login/login.component';
import { LogoutComponent } from './c/logout/logout.component';
import { SearchAccountsComponent} from './c/search-accounts/search-accounts.component'
import { SelectPreferencesComponent } from './c/select-preferences/select-preferences.component';
import { VerifyAccountComponent } from './c/verify-account/verify-account.component';

export const accountRoutes: Routes = [
    { path: '', component: SearchAccountsComponent, canActivate: [AuthGuard]},
	{ path: 'edit-account',  component: AccountDetailsComponent, canActivate: [AuthGuard] },
	{ path: 'edit-my-account',  component: AccountDetailsComponent, canActivate: [AuthGuard] },
	{ path: 'selectpreferences', component: SelectPreferencesComponent, canActivate: [AuthGuard] },
	// { path: 'updateanonymousprofile', loadComponent: () => import('./c/update-anonymous-profile/update-anonymous-profile.component').then(m => m.UpdateAnonymousProfileComponent), canActivate: [AuthGuard] },
	{ path: 'logon', component: LoginComponent },
	{ path: 'change-password', component: ChangePasswordComponent },
	{ path: 'reset-password', component: ChangePasswordComponent },
	{ path: 'logout', component: LogoutComponent },
	{ path: 'forgot-password', component: ForgotPasswordComponent },
	{ path: 'verify-account', component: VerifyAccountComponent },
];
// export const accountRoutes: Routes = [
//     { path: '', loadComponent: () => import('./c/search-accounts/search-accounts.component').then(m => m.SearchAccountsComponent), canActivate: [AuthGuard]},
// 	{ path: 'edit-account',  loadComponent: () => import('./c/account-details/account-details.component').then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
// 	{ path: 'edit-my-account', loadComponent: () => import('./c/account-details/account-details.component').then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
// 	{ path: 'register', loadComponent: () => import('./c/account-details/account-details.component').then(m => m.AccountDetailsComponent), canActivate: [AuthGuard] },
// 	{ path: 'selectpreferences', loadComponent: () => import('./c/select-preferences/select-preferences.component').then(m => m.SelectPreferencesComponent), canActivate: [AuthGuard] },
// 	// { path: 'updateanonymousprofile', loadComponent: () => import('./c/update-anonymous-profile/update-anonymous-profile.component').then(m => m.UpdateAnonymousProfileComponent), canActivate: [AuthGuard] },
// 	{ path: 'logon', loadComponent: () => import('./c/login/login.component').then(m => m.LoginComponent) },
// 	{ path: 'change-password', loadComponent: () => import('./c/change-password/change-password.component').then(m => m.ChangePasswordComponent) },
// 	{ path: 'reset-password', loadComponent: () => import('./c/change-password/change-password.component').then(m => m.ChangePasswordComponent) },
// 	{ path: 'logout', loadComponent: () => import('./c/logout/logout.component').then(m => m.LogoutComponent) },
// 	{ path: 'forgot-password', loadComponent: () => import('./c/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) },
// 	{ path: 'verify-account', loadComponent: () => import('./c/verify-account/verify-account.component').then(m => m.VerifyAccountComponent) },
// ];