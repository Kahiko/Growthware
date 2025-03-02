import { Component, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// Library
// Feature
import { NavigationComponentBase } from '../navigation-component-base/navigation-component-base.component';
import { MenuTypes } from '../../menu-types.enum';

@Component({
	selector: 'gw-core-horizontal',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		MatIconModule,
		MatListModule
	],
	templateUrl: './horizontal.component.html',
	styleUrls: ['./horizontal.component.scss']
})
export class HorizontalComponent extends NavigationComponentBase {
	@ViewChild('firstLevel', { static: false }) override firstLevel: ElementRef<HTMLUListElement> = {} as ElementRef<HTMLUListElement>;
	override _MenuType: MenuTypes = MenuTypes.Horizontal;
}
