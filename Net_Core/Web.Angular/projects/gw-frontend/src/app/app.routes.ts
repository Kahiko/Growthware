import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';

export const routes: Routes = [
	{ path: '', loadChildren: () => import('./features/home/home.routes').then(m => m.homeRoutes) },
	{ path: 'naturalsort', loadChildren: () => import('./routes/sys-admin.routes').then(m => m.sysAdminRoutes), canActivate: [AuthGuard] },
	{ path: 'sys_admin', loadChildren: () => import('./routes/sys-admin.routes').then(m => m.sysAdminRoutes) },
	{ path: 'securityentity', loadChildren: () => import('./routes/security-entity.routes').then(m => m.securityEntityRoutes), canActivate: [AuthGuard] },
	{ path: 'accounts', loadChildren: () => import('./routes/account-routes').then(m => m.accountRoutes) },
	{ path: 'manage_cache_dependency', loadChildren: () => import('./routes/file-manager.routes').then(m => m.fileManagerRoutes) },
	{ path: 'manage_logs', loadChildren: () => import('./routes/file-manager.routes').then(m => m.fileManagerRoutes) },
	{ path: 'feedbacks', loadChildren: () => import('./routes/feedback-routes').then(m => m.feedbackRoutes), canActivate: [AuthGuard] },
	{ path: 'functions', loadChildren: () => import('./routes/function.routes').then(m => m.functionRoutes), canActivate: [AuthGuard] },
	{ path: 'search_name_value_pairs', loadChildren: () => import('./routes/name-value-pair.routes').then(m => m.nameValuePairRoutes), canActivate: [AuthGuard] },
	{ path: 'search_states', loadChildren: () => import('./routes/states.routes').then(m => m.statesRoutes), canActivate: [AuthGuard] },
	{ path: 'security', loadChildren: () => import('./routes/security.routes').then(m => m.secutiryRoutes) },
	{ path: 'manage_groups', loadChildren: () => import('./routes/group.routes').then(m => m.groupRoutes), canActivate: [AuthGuard] },
	{ path: 'search_roles', loadChildren: () => import('./routes/role.routes').then(m => m.roleRoutes), canActivate: [AuthGuard] },
	{ path: 'search_messages', loadChildren: () => import('./routes/message.routes').then(m => m.messagesRoutes), canActivate: [AuthGuard] },
	{ path: 'testing', loadChildren: () => import('./routes/testing-routes').then(m => m.testingRoutes), canActivate: [AuthGuard] },
	{ path: 'communitycalendar', loadChildren: () => import('./routes/community-calendar-routes').then(m => m.communityCalendarRoutes) },
	{ path: 'addeditworkflow', loadChildren: () => import('./routes/workflows-routes').then(m => m.workflowRoutes) },
	{ path: 'logging', loadChildren: () => import('./routes/logging-routes').then(m => m.loggingRoutes) },
];
