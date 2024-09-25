import { Component, ViewEncapsulation } from '@angular/core';
// Skin
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
	selector: 'gw-frontend-dashboard-layout',
	templateUrl: './dashboard-layout.component.html',
	styleUrls: ['./dashboard-layout.component.scss'],
	animations: [sideNavTextAnimation],
	encapsulation: ViewEncapsulation.None
})
export class DashboardLayoutComponent {

	constructor() { }

}
