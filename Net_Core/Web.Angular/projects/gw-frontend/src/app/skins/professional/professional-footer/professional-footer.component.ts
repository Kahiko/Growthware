import { Component } from '@angular/core';
// Library Components
import { HorizontalComponent } from '@growthware/core/navigation';

@Component({
	selector: 'gw-frontend-professional-footer',
	standalone: true,
	templateUrl: './professional-footer.component.html',
	styleUrls: ['./professional-footer.component.scss'],
	imports: [
		HorizontalComponent,
	],
})
export class ProfessionalFooterComponent {

}
