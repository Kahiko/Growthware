import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Application
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
  { path: 'search-accounts', loadChildren: () => import('./features/accounts/accounts.module').then(m => m.AccountsModule), canActivate: [AuthGuard]  },
  { path: 'search-functions', loadChildren: () => import('./features/functions/functions.module').then(m => m.FunctionsModule), canActivate: [AuthGuard]  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
