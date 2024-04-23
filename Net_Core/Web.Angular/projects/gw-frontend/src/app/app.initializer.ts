import { finalize } from 'rxjs';
// Library
import { AccountService, IAuthenticationResponse } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';
import { DynamicTableService } from '@growthware/core/dynamic-table';
export function appInitializer(
	accountSvc: AccountService, 
	configurationSvc: ConfigurationService,
	dynamicTableSvc: DynamicTableService,
) {
	// Called by app.config.ts:ApplicationConfig
	return () => {
		return new Promise((resolve) => {
			configurationSvc.loadAppSettings();
			dynamicTableSvc.loadDefaultTableConfig();
			accountSvc.refreshToken().pipe(finalize(() => {
				resolve(true);
			})).subscribe({
				next: (authenticationResponse: IAuthenticationResponse) => {
					// console.log('appInitializer.authenticationResponse', authenticationResponse);
					// We need to log out for the anonymous account in order to generate a Json Web Token
					// that is used in auth-guard.guard.ts (canActivate)
					if (authenticationResponse.account.toLocaleLowerCase() === 'anonymous') {
						accountSvc.logout(true);
					}
				},
				error: (error) => {
					console.log(error);
				}
			});
		});
	};
}
