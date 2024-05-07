
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library MISC
import { AuthGuard } from '@growthware/common/services';
// Library Components
import { AccountDetailsComponent } from '@growthware/core/account';
import { ChangePasswordComponent } from '@growthware/core/account';
import { ForgotPasswordComponent } from '@growthware/core/account/src/c/forgot-password/forgot-password.component';
import { LoginComponent } from '@growthware/core/account';
import { LogoutComponent } from '@growthware/core/account';
import { SearchAccountsComponent } from '@growthware/core/account';
import { SelectPreferencesComponent } from '@growthware/core/account';
import { UpdateAnonymousProfileComponent } from '@growthware/core/account';

const childRoutes: Routes = [
	{ path: '', component: SearchAccountsComponent, canActivate: [AuthGuard]},
	{ path: 'edit-account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
	{ path: 'edit-my-account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
	{ path: 'register', component: AccountDetailsComponent },
	{ path: 'selectpreferences', component: SelectPreferencesComponent, canActivate: [AuthGuard] },
	{ path: 'updateanonymousprofile', component: UpdateAnonymousProfileComponent, canActivate: [AuthGuard] },
	{ path: 'logon', component: LoginComponent},
	{ path: 'change-password', component: ChangePasswordComponent },
	{ path: 'logout', component: LogoutComponent },
	{ path: 'forgot-password', component: ForgotPasswordComponent },
];


@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class AccountsRoutingModule { }
