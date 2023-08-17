import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Application
import { AuthGuard } from '@Growthware/common-code';

const routes: Routes = [
  { path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
  { path: 'accounts', loadChildren: () => import('./features/accounts/accounts.module').then(m => m.AccountsModule), canActivate: [AuthGuard]  },
  { path: 'manage_cache_dependency', loadChildren: () => import('./features/file-manager/file-manager.module').then(m => m.FileManagerModule) },
  // { path: 'manage_logs', loadChildren: () => import('./features/file-manager/file-manager.module').then(m => m.FileManagerModule) },
  { path: 'functions', loadChildren: () => import('./features/functions/functions.module').then(m => m.FunctionsModule), canActivate: [AuthGuard]  },  
  { path: 'search_name_value_pairs', loadChildren: () => import('./features/name-value-pairs/name-value-pairs.module').then(m => m.NameValuePairsModule), canActivate: [AuthGuard]  },  
  { path: 'search_states', loadChildren: () => import('./features/states/states-routing.module').then(m => m.StatesRoutingModule), canActivate: [AuthGuard]  },  
  { path: 'security', loadChildren: () => import('./features/security/security.module').then(m => m.SecurityModule)  },
  { path: 'search_security_entities', loadChildren: () => import('./features/security-entities/security-entities.module').then(m => m.SecurityEntitiesModule)  },
  { path: 'manage_groups', loadChildren: () => import('./features/groups/groups.module').then(m => m.GroupsModule), canActivate: [AuthGuard]  },
  { path: 'search_roles', loadChildren: () => import('./features/roles/roles.module').then(m => m.RolesModule), canActivate: [AuthGuard]  },
  { path: 'search_messages', loadChildren: () => import('./features/messages/messages.module').then(m => m.MessagesModule), canActivate: [AuthGuard]  },

  { path: 'sys_admin', loadChildren: () => import('./features/sys-admin/sys-admin.module').then(m => m.SysAdminModule)  },

  { path: 'communitycalendar', loadChildren: () => import('./features/community-calendar/community-calendar.module').then(m => m.CommunityCalendarModule)  },
  { path: 'addeditworkflow', loadChildren: () => import('./features/workflows/workflows.module').then(m => m.WorkflowsModule)  },
  { path: 'setloglevel', loadChildren: () => import('./features/logging/logging.module').then(m => m.LoggingModule)  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
