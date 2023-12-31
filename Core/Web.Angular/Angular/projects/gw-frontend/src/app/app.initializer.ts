import { AccountService } from '@Growthware/features/account';
import { ConfigurationService } from '@Growthware/features/configuration';
import { finalize } from 'rxjs';

export function appInitializer(accountSvc: AccountService, configurationSvc: ConfigurationService) {
  return () => {
    return new Promise((resolve) => {
      configurationSvc.loadAppSettings();
      accountSvc.refreshToken().pipe(finalize(() => {
        resolve(true);
      })).subscribe({
        // next: (authenticationResponse: IAuthenticationResponse) => {
        //   // console.log('appInitializer.authenticationResponse', authenticationResponse);
        // },
        error: (error) => {
          console.log(error);
        }
      });
    })
  };
}
