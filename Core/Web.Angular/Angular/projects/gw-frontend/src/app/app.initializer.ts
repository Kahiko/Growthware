// import { catchError, of, tap } from 'rxjs';
import { AccountService } from '@Growthware/features/account';
import { ConfigurationService } from '@Growthware/features/configuration';

export function appInitializer(accountSvc: AccountService, configurationSvc: ConfigurationService) {
  return () => {
    return new Promise((resolve) => {
      configurationSvc.loadAppSettings();
      resolve(true);
    });
  };  
  // configurationSvc.loadAppSettings();
  // return () => accountSvc.refreshFromToken()
  //   .pipe(
  //     tap(authenticationResponse => {  
  //       // console.log('appInitializer.authenticationResponse', authenticationResponse);
  //       if(authenticationResponse.account.toLocaleLowerCase() !== accountSvc.anonymous.toLocaleLowerCase()) {
  //         accountSvc.startRefreshTokenTimer();
  //       }
  //     }),
  //     // catch error to start app on success or failure
  //     catchError((err) => {
  //       console.log(err);
  //       return of();
  //     })
  //   );
}
