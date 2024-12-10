The "Routes" directory contains the routing configuration for Growthware Frontend and serves an an example of how to implement the routing.  Of course, any routing you may need should be implemented as you see fit.

Please note that each of the .routes.ts files are a simple component defined route(s) with the appropriate import of the component(s), the following is an example of the SearchAccountsComponent route:

import { SearchAccountsComponent } from '@growthware/core/account';

export const accountRoutes: Routes = [
    { path: '', component: SearchAccountsComponent, canActivate: [AuthGuard]},
];

Lazy loading is accomplished by using the `loadChildren` property in the ../app.routes.ts file.  Using a loadComponent here would not be usefull.