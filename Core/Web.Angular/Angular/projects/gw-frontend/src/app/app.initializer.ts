import { AccountService, IAuthenticationResponse } from '@Growthware/features/account';
import { ConfigurationService } from '@Growthware/features/configuration';
import { finalize } from 'rxjs';

export function appInitializer(accountSvc: AccountService, configurationSvc: ConfigurationService) {
  return () => {
    return new Promise((resolve) => {
      configurationSvc.loadAppSettings();
      accountSvc.refreshToken().pipe(finalize(() => {
        resolve(true);
      })).subscribe({
        next: (authenticationResponse: IAuthenticationResponse) => {
          // console.log('appInitializer.authenticationResponse', authenticationResponse);
          // We need to log out for the anonymous account in order to generate a Json Web Token
          // that is used in auth-guard.guard.ts (canActivate)
          if(authenticationResponse.account.toLocaleLowerCase() === 'anonymous') {
            accountSvc.logout(true);
          }
        },
        error: (error) => {
          console.log(error);
        }
      });
    })
  };
}
