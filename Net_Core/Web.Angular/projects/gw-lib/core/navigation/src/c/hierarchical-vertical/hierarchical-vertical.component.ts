import { Component, ElementRef, ViewChild, ViewEncapsulation } from '@angular/core';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
// Library
// Feature
import { HierarchicalNavListItemComponent } from '../hierarchical-nav-list-item/hierarchical-nav-list-item';
import { MenuTypes } from '../../menu-types.enum';
import { NavigationComponentBase } from '../navigation-component-base/navigation-component-base.component';

@Component({
	selector: 'gw-core-hierarchical-vertical',
	standalone: true,
	imports: [
		HierarchicalNavListItemComponent,
		// Angular Material
		MatIconModule,
	],
	templateUrl: './hierarchical-vertical.component.html',
	styleUrls: ['./hierarchical-vertical.component.scss'],
	encapsulation: ViewEncapsulation.ShadowDom,
})
export class HierarchicalVerticalComponent extends NavigationComponentBase {
	@ViewChild('firstLevel', { static: false }) override firstLevel: ElementRef<HTMLUListElement> = {} as ElementRef<HTMLUListElement>;
	override _MenuType: MenuTypes = MenuTypes.Hierarchical;
}
