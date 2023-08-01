import { AccountService } from '@Growthware/Lib/src/features/account';
import { catchError, finalize, of, tap } from 'rxjs';

export function appInitializer(accountSvc: AccountService) {
  return () => accountSvc.refreshToken()
    .pipe(
      tap(authenticationResponse => {  
        // console.log('appInitializer.authenticationResponse', authenticationResponse);
      }),
      // catch error to start app on success or failure
      catchError((err) => {
        console.log(err);
        return of();
      })
    );
}
