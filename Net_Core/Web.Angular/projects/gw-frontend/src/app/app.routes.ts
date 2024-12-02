import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';

export const routes: Routes = [
	{ path: '',							loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
	{ path: 'naturalsort',				loadChildren: () => import('@growthware/core/sys-admin').then(m => m.sysAdminRoutes), canActivate: [AuthGuard] },
	{ path: 'sys_admin', 				loadChildren: () => import('@growthware/core/sys-admin').then(m => m.sysAdminRoutes)  },
	{ path: 'securityentity',			loadChildren: () => import('@growthware/core/security-entities').then(m => m.securityEntityRoutes), canActivate: [AuthGuard] },
	{ path: 'accounts',					loadChildren: () => import('@growthware/core/account').then(m => m.accountRoutes)  },
	{ path: 'manage_cache_dependency',	loadChildren: () => import('@growthware/core/file-manager').then(m => m.fileManagerRoutes) },
	{ path: 'manage_logs', 				loadChildren: () => import('@growthware/core/file-manager').then(m => m.fileManagerRoutes) },
	{ path: 'feedbacks', 				loadChildren: () => import('@growthware/core/feedback').then(m => m.feedbackRoutes), canActivate: [AuthGuard]  },
	{ path: 'functions', 				loadChildren: () => import('@growthware/core/function').then(m => m.functionRoutes), canActivate: [AuthGuard]  },  
	{ path: 'search_name_value_pairs', 	loadChildren: () => import('@growthware/core/name-value-pair').then(m => m.nameValuePairRoutes), canActivate: [AuthGuard]  },  
	{ path: 'search_states', 			loadChildren: () => import('@growthware/core/states').then(m => m.statesRoutes), canActivate: [AuthGuard]  },  
	{ path: 'security', 				loadChildren: () => import('@growthware/core/security').then(m => m.secutiryRoutes)  },
	{ path: 'manage_groups', 			loadChildren: () => import('@growthware/core/group').then(m => m.groupRoutes), canActivate: [AuthGuard]  },
	{ path: 'search_roles', 			loadChildren: () => import('@growthware/core/role').then(m => m.roleRoutes), canActivate: [AuthGuard]  },
	{ path: 'search_messages', 			loadChildren: () => import('@growthware/core/message').then(m => m.messagesRoutes), canActivate: [AuthGuard]  },
	{ path: 'communitycalendar', 		loadChildren: () => import('@growthware/core/community-calendar').then(m => m.communityCalendarRoutes)  },
	{ path: 'addeditworkflow', 			loadChildren: () => import('@growthware/core/workflows').then(m => m.workflowRoutes)  },
	{ path: 'logging', 					loadChildren: () => import('@growthware/core/logging').then(m => m.loggingRoutes)  },
];
