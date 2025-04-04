import { Routes } from '@angular/router';
// Library
import { AuthGuard } from '@growthware/common/services';
import { canLeaveFeedbackPage, FeedbackComponent, SearchFeedbacksComponent } from '@growthware/core/feedback'

export const feedbackRoutes: Routes = [
	{ path: '', component: SearchFeedbacksComponent, canActivate: [AuthGuard]},
	{ path: 'feedback', component: FeedbackComponent, canActivate: [AuthGuard], canDeactivate: [canLeaveFeedbackPage] },
];
