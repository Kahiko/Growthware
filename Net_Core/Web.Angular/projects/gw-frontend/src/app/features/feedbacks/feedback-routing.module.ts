import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library Services
import { AuthGuard } from '@growthware/common/services';
// Feature
import { SearchFeedbacksComponent } from '@growthware/core/feedback';
import { FeedbackComponent } from '@growthware/core/feedback';

const childRoutes: Routes = [
	{ path: '', component: SearchFeedbacksComponent, canActivate: [AuthGuard]},
	{ path: 'feedback', component: FeedbackComponent, canActivate: [AuthGuard] }
];

@NgModule({
	imports: [RouterModule.forChild(childRoutes)],
	exports: [RouterModule]
})
export class FeedbackRoutingModule { }
