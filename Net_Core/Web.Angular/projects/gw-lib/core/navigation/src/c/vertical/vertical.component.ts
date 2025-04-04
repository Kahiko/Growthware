import { Component, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// Library
// Feature
import { NavigationComponentBase } from '../navigation-component-base/navigation-component-base.component';
import { MenuTypes } from '../../menu-types.enum';

@Component({
	selector: 'gw-core-vertical',
	standalone: true,
	imports: [
		CommonModule,
		MatIconModule,
		MatListModule
	],
	templateUrl: './vertical.component.html',
	styleUrls: ['./vertical.component.scss']
})
export class VerticalComponent extends NavigationComponentBase {
	@ViewChild('firstLevel', { static: false }) override firstLevel: ElementRef<HTMLUListElement> = {} as ElementRef<HTMLUListElement>;
	override _MenuType: MenuTypes = MenuTypes.Vertical;
}

