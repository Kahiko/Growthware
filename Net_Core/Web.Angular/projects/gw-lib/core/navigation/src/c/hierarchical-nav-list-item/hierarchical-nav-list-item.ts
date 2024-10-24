import { Component, computed, HostBinding, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Router, RouterModule } from '@angular/router';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// Feature
import { INavLink } from '../../nav-link.model';
import { NavigationService } from '../../navigation.service';

@Component({
	selector: 'gw-core-vertical-list-item',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		// Angular Material
		MatIconModule,
		MatListModule
	],
	templateUrl: './hierarchical-nav-list-item.html',
	styleUrls: ['./hierarchical-nav-list-item.scss'],
	animations: [
		trigger('indicatorRotate', [
			state('collapsed', style({ transform: 'rotate(0deg)' })),
			state('expanded', style({ transform: 'rotate(90deg)' })),
			transition('expanded <=> collapsed',
				animate('225ms cubic-bezier(0.4,0.0,0.2,1)')
			)
		])
	]
})
export class HierarchicalNavListItemComponent {
	private _NavigationSvc = inject(NavigationService);
	private _Router = inject(Router);

	expanded!: boolean;
	showSideNavLinkText = computed(() =>this._NavigationSvc.showNavText$());
	@HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;
	depth = input<number>(0);
	item = input.required<INavLink>();

	onItemSelected(item: INavLink) {
		if (item.children && item.children.length) {
			this.expanded = !this.expanded;
		}
		this._NavigationSvc.navigateTo(item);
	}
}
