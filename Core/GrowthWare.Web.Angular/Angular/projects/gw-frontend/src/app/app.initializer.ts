import { AccountService } from '@Growthware/Lib/src/lib/features/account';

export function appInitializer(accountService: AccountService) {
  return () => new Promise(resolve => {
      // attempt to refresh token on app start up to auto authenticate
      accountService.refreshToken()
          .subscribe()
          .add(() => teardown(resolve));
  });

  function teardown(resolve: any): void {
    return resolve;
  }
}
