import { Component, ElementRef, input, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// Library
import { AccountService } from '@growthware/core/account';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
// Feature
import { NavigationComponentBase } from '../navigation-component-base/navigation-component-base.component';
import { NavigationService } from '../../navigation.service';
import { MenuTypes } from '../../menu-types.enum';
import { INavLink } from '../../nav-link.model';

@Component({
	selector: 'gw-core-vertical',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		MatIconModule,
		MatListModule
	],
	templateUrl: './vertical.component.html',
	styleUrls: ['./vertical.component.scss']
})
export class VerticalComponent extends NavigationComponentBase {
	@ViewChild('firstLevel', { static: false }) override firstLevel!: ElementRef<HTMLUListElement>;
	override _MenuType: MenuTypes = MenuTypes.Vertical;
  }

