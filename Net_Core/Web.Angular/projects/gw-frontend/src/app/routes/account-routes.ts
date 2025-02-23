import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { AccountDetailsComponent, ChangePasswordComponent, ForgotPasswordComponent } from '@growthware/core/account';
import { LogoutComponent, LoginComponent, SearchAccountsComponent, SelectPreferencesComponent } from '@growthware/core/account';
import { VerifyAccountComponent } from '@growthware/core/account';

export const accountRoutes: Routes = [
	{ path: '', component: SearchAccountsComponent, canActivate: [AuthGuard]},
	{ path: 'edit-account',  component: AccountDetailsComponent, canActivate: [AuthGuard] },
	{ path: 'edit-my-account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
	{ path: 'register', component: AccountDetailsComponent, canActivate: [AuthGuard] },
	{ path: 'selectpreferences', component: SelectPreferencesComponent, canActivate: [AuthGuard] },
	// { path: 'updateanonymousprofile', loadComponent: () => import('./c/update-anonymous-profile/update-anonymous-profile.component').then(m => m.UpdateAnonymousProfileComponent), canActivate: [AuthGuard] },
	{ path: 'logon', component: LoginComponent },
	{ path: 'change-password', component: ChangePasswordComponent },
	{ path: 'reset-password', component: ChangePasswordComponent },
	{ path: 'logout', component: LogoutComponent },
	{ path: 'forgot-password', component: ForgotPasswordComponent },
	{ path: 'verify-account', component: VerifyAccountComponent },
];