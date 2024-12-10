import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';
import { SearchMessagesComponent } from '@growthware/core/message';

export const messagesRoutes: Routes = [
	{ path: '', component: SearchMessagesComponent, canActivate: [AuthGuard] },
];