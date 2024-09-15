import { Component, HostBinding, input, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Subscription } from 'rxjs';
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
		MatIconModule,
		MatListModule,

		RouterModule
	],
	templateUrl: './vertical-list-item.component.html',
	styleUrls: ['./vertical-list-item.component.scss'],
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
export class VerticalListItemComponent implements OnDestroy, OnInit {
	expanded!: boolean;
	showSideNavLinkText!: boolean;
	@HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;
	depth = input<number>(0);
	item = input.required<INavLink>();

	private _Subscription: Subscription = new Subscription();

	constructor(
		private _NavigationSvc: NavigationService,
		public _Router: Router
	) { }

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}

	ngOnInit(): void {
		this._Subscription.add(
			this._NavigationSvc.showNavText$.subscribe((value) => { this.showSideNavLinkText = value; })
		);
	}

	onItemSelected(item: INavLink) {
		if (item.children && item.children.length) {
			this.expanded = !this.expanded;
		}
		this._NavigationSvc.navigateTo(item);
	}
}
