import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';

const rootRoutes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'counter', loadChildren: () => import('./ms-demo/ms-demo.module').then(m => m.MSDemoModule) },
  { path: 'search-accounts', loadChildren: () => import('./features/accounts/accounts.module').then(m => m.AccountsModule) },
  { path: 'search-functions', loadChildren: () => import('./features/functions/functions.module').then(m => m.FunctionsModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(rootRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
