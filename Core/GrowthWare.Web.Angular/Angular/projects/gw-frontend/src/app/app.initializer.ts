import { AccountService } from '@Growthware/Lib/src/lib/features/account';
import { catchError, finalize, of } from 'rxjs';

export function appInitializer(accountSvc: AccountService) {
  return () => accountSvc.refreshToken()
      .pipe(
          // catch error to start app on success or failure
          catchError(() => of())
      );
}
