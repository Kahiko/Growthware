import { Routes } from '@angular/router';
// Application
import { AuthGuard } from '@growthware/common/services';

export const routes: Routes = [
	{ path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
	{ path: 'naturalsort', loadComponent: () => import('@growthware/core/sys-admin').then(m => m.NaturalSortComponent), canActivate: [AuthGuard] },
	{ path: 'selectasecurityentity', loadComponent: () => import('@growthware/core/security-entities').then(m => m.SelectSecurityEntityComponent), canActivate: [AuthGuard] },
	{ path: 'update', loadComponent: () => import('@growthware/core/sys-admin').then(m => m.UpdateSessionComponent), canActivate: [AuthGuard] },
	{ path: 'accounts', loadChildren: () => import('./features/accounts/accounts.module').then(m => m.AccountsModule)  },
	{ path: 'manage_cache_dependency', loadChildren: () => import('./features/file-manager/file-manager.module').then(m => m.FileManagerModule) },
	{ path: 'manage_logs', loadChildren: () => import('./features/file-manager/file-manager.module').then(m => m.FileManagerModule) },
	{ path: 'functions', loadChildren: () => import('./features/functions/functions.module').then(m => m.FunctionsModule), canActivate: [AuthGuard]  },  
	{ path: 'search_name_value_pairs', loadChildren: () => import('./features/name-value-pairs/name-value-pairs.module').then(m => m.NameValuePairsModule), canActivate: [AuthGuard]  },  
	{ path: 'search_states', loadChildren: () => import('./features/states/states-routing.module').then(m => m.StatesRoutingModule), canActivate: [AuthGuard]  },  
	{ path: 'security', loadChildren: () => import('./features/security/security.module').then(m => m.SecurityModule)  },
	{ path: 'search_security_entities', loadChildren: () => import('./features/security-entities/security-entities.module').then(m => m.SecurityEntitiesModule)  },
	{ path: 'manage_groups', loadChildren: () => import('./features/groups/groups.module').then(m => m.GroupsModule), canActivate: [AuthGuard]  },
	{ path: 'search_roles', loadChildren: () => import('./features/roles/roles.module').then(m => m.RolesModule), canActivate: [AuthGuard]  },
	{ path: 'search_messages', loadChildren: () => import('./features/messages/messages.module').then(m => m.MessagesModule), canActivate: [AuthGuard]  },
	{ path: 'sys_admin', loadChildren: () => import('./features/sys-admin/sys-admin.module').then(m => m.SysAdminModule)  },
	{ path: 'communitycalendar', loadComponent: () => import('@growthware/core/community-calendar').then(m => m.CalendarComponent)  },
	{ path: 'addeditworkflow', loadChildren: () => import('./features/workflows/workflows.module').then(m => m.WorkflowsModule)  },
	{ path: 'setloglevel', loadChildren: () => import('./features/logging/logging.module').then(m => m.LoggingModule)  },
	{ path: 'logging/test-logging', loadComponent: () => import('@growthware/core/logging').then(m => m.TestLoggingComponent)  },
];
// export const routes: Routes = [];
