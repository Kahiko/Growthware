import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs';
// Library
import { AccountService, IAuthenticationResponse } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';
import { DynamicTableService } from '@growthware/core/dynamic-table';

export function appInitializer(
	_ActivatedRoute: ActivatedRoute,
	_AccountSvc: AccountService, 
	_ConfigurationSvc: ConfigurationService,
	_DynamicTableSvc: DynamicTableService,
) {
	// Called by app.config.ts:ApplicationConfig
	return () => {
		return new Promise((resolve) => {
			_ConfigurationSvc.loadAppSettings();
			_DynamicTableSvc.loadDefaultTableConfig();
			_AccountSvc.refreshToken().pipe(finalize(() => {
				resolve(true);
			})).subscribe({
				next: (authenticationResponse: IAuthenticationResponse) => {
					// console.log('appInitializer.authenticationResponse', authenticationResponse);
					// For the '/accounts/reset-password' route we only need to log out without the default navigation
					// Otherwise if the accunt is anonymous we need to log out and navigate to the default page
					const isResetPasswordRoute = window.location && window.location.pathname && window.location.pathname.toLowerCase() === '/accounts/reset-password';
					if(!isResetPasswordRoute) {
						// We need to log out for the anonymous account in order to generate a Json Web Token
						// that is used in auth-guard.guard.ts (canActivate)
						if (authenticationResponse.account.toLocaleLowerCase() === 'anonymous') {
							_AccountSvc.logout(true);
						}
					} else {
						// We need to always log out and generate a Json Web Token that is used in auth-guard.guard.ts (canActivate).
						// We do not want to navigate afterwards so that the ChangePasswordComponent component is rendered.
						// See accounts-routing.module.ts
						_AccountSvc.logout(false, false);
					}
				},
				error: (error) => {
					console.log(error);
				}
			});
		});
	};
}
