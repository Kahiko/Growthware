import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Application
import { AuthGuard } from '@Growthware/Lib/src/lib/guards';

const routes: Routes = [
  { path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
  { path: 'manage_cache_dependency', loadChildren: () => import('./features/file-manager/file-manager.module').then(m => m.FileManagerModule) },
  { path: 'manage_logs', loadChildren: () => import('./features/file-manager/file-manager.module').then(m => m.FileManagerModule) },
  { path: 'accounts', loadChildren: () => import('@Growthware/Lib/src/lib/features/account').then(m => m.AccountModule), canActivate: [AuthGuard]  },
  { path: 'search-functions', loadChildren: () => import('./features/functions/functions.module').then(m => m.FunctionsModule), canActivate: [AuthGuard]  },
  { path: 'manage-groups', loadChildren: () => import('./features/groups/groups.module').then(m => m.GroupsModule), canActivate: [AuthGuard]  },
  { path: 'manage-roles', loadChildren: () => import('./features/roles/roles.module').then(m => m.RolesModule), canActivate: [AuthGuard]  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }