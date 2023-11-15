import { AccountService } from '@Growthware/features/account';
import { catchError, of, tap } from 'rxjs';

export function appInitializer(accountSvc: AccountService) {
  return () => accountSvc.refreshFromToken()
    .pipe(
      tap(authenticationResponse => {  
        // console.log('appInitializer.authenticationResponse', authenticationResponse);
        if(authenticationResponse.account.toLocaleLowerCase() !== accountSvc.anonymous.toLocaleLowerCase()) {
          accountSvc.startRefreshTokenTimer();
        }
      }),
      // catch error to start app on success or failure
      catchError((err) => {
        console.log(err);
        return of();
      })
    );
}
