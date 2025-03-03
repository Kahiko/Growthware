import { Component, ElementRef, ViewChild, ViewEncapsulation } from '@angular/core';

// Library
// Feature
import { MenuTypes } from '../../menu-types.enum';
import { NavigationComponentBase } from '../navigation-component-base/navigation-component-base.component';
// import { INavLink } from '../../nav-link.model';

@Component({
	selector: 'gw-core-hierarchical-horizontal',
	standalone: true,
	imports: [],
	templateUrl: './hierarchical-horizontal.component.html',
	styleUrls: ['./hierarchical-horizontal.component.scss'],
	encapsulation: ViewEncapsulation.ShadowDom,
})
export class HierarchicalHorizontalComponent extends NavigationComponentBase {
	@ViewChild('firstLevel', { static: false }) override firstLevel: ElementRef<HTMLUListElement> = {} as ElementRef<HTMLUListElement>;
	override _MenuType: MenuTypes = MenuTypes.Hierarchical;
}
