import { Component, ChangeDetectionStrategy } from '@angular/core';
// Library Components
import { HorizontalComponent } from '@growthware/core/navigation';

@Component({
    selector: 'gw-frontend-professional-footer',
    templateUrl: './professional-footer.component.html',
    styleUrls: ['./professional-footer.component.scss'],
    changeDetection: ChangeDetectionStrategy.Eager,
    imports: [
        HorizontalComponent,
    ]
})
export class ProfessionalFooterComponent {

}
