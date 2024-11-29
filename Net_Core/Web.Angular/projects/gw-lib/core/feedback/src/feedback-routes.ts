import { Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';
// Feature
import { canLeaveFeedbackPage } from './c/feedback/feedback.component';

export const feedbackRoutes: Routes = [
	{ path: '', loadComponent: () => import('./c/search-feedbacks/search-feedbacks.component').then(m => m.SearchFeedbacksComponent), canActivate: [AuthGuard]},
	{ path: 'feedback', loadComponent: () => import('./c/feedback/feedback.component').then(m => m.FeedbackComponent), canActivate: [AuthGuard], canDeactivate: [canLeaveFeedbackPage] },
];
