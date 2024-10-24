import { Component, ViewEncapsulation } from '@angular/core';
// Library
import { sideNavTextAnimation } from '../animations/side-nav';

@Component({
	selector: 'gw-frontend-dev-ops-layout',
	templateUrl: './dev-ops-layout.component.html',
	styleUrls: ['./dev-ops-layout.component.scss'],
	animations: [sideNavTextAnimation],
	encapsulation: ViewEncapsulation.None,
})
export class DevOpsLayoutComponent {

}
