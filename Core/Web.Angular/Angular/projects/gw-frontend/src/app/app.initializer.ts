import { AccountService, IAuthenticationResponse } from '@Growthware/features/account';
import { ConfigurationService } from '@Growthware/features/configuration';
import { finalize } from 'rxjs';

export function appInitializer(accountSvc: AccountService, configurationSvc: ConfigurationService) {
  return () => {
    return new Promise((resolve) => {
      configurationSvc.loadAppSettings();
      accountSvc.refreshFromToken().pipe(finalize(() => {
        resolve(true);
      })).subscribe({
        next: (authenticationResponse: IAuthenticationResponse) => {
          // console.log('appInitializer.authenticationResponse', authenticationResponse);
          if (authenticationResponse.account.toLocaleLowerCase() !== accountSvc.anonymous.toLocaleLowerCase()) {
            accountSvc.startRefreshTokenTimer();
          }
        },
        error: (error: any) => {
          console.log(error);
        }
      });
    })
  };
}
