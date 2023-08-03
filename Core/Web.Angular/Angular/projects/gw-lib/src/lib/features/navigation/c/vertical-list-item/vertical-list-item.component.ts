import { Component, HostBinding, Input, OnDestroy, OnInit } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Subscription } from 'rxjs';
import { INavLink } from '../../nav-link.model';
import { Router } from '@angular/router';
import { MenuListService } from '../../menu-list.service';

@Component({
  selector: 'gw-lib-vertical-list-item',
  templateUrl: './vertical-list-item.component.html',
  styleUrls: ['./vertical-list-item.component.scss'],
  animations: [
    trigger('indicatorRotate', [
      state('collapsed', style({transform: 'rotate(0deg)'})),
      state('expanded', style({transform: 'rotate(180deg)'})),
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
  @Input() depth!: number;
  @Input() item!: INavLink;

  private _Subscription: Subscription = new Subscription();

  constructor(
    private _MenuListSvc: MenuListService,
    public _Router: Router
  ) { }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    if (this.depth === undefined) {
      this.depth = 0;
    }
    this._Subscription.add(
      this._MenuListSvc.showNavText$.subscribe((value) => { this.showSideNavLinkText = value; })
    );
  }

  onItemSelected(item: INavLink) {
    if (!item.children || !item.children.length) {
      this._Router.navigate([item.link]);
      // this.navService.closeNav();
    }
    if (item.children && item.children.length) {
      this.expanded = !this.expanded;
    }
  }
}
