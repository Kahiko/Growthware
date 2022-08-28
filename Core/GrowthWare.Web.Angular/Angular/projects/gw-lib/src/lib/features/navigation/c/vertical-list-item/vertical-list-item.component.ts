import { Component, HostBinding, Input, OnInit } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
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
    ]),
    trigger('animateText', [
      state('hide', style({ width: 0, opacity: 0, })),
      state('show', style({ width: 150, opacity: 1, })),
      transition('hide => show',  // not working
        animate('3500ms ease-in')
      ),
      transition('show => hide',  // not working
        animate('200ms ease-out')
      ),
    ])
  ]
})
export class VerticalListItemComponent implements OnInit {
  expanded!: boolean;
  @HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;
  @Input() depth!: number;
  @Input() item!: INavLink;
  @Input() showSideNavLinkText: boolean = true;

  constructor(
    private _MenuListSvc: MenuListService,
    public router: Router
  ) { }

  ngOnInit(): void {
    if (this.depth === undefined) {
      this.depth = 0;
    }
  }

  onItemSelected(item: INavLink) {
    if (!item.children || !item.children.length) {
      this.router.navigate([item.link]);
      // this.navService.closeNav();
    }
    if (item.children && item.children.length) {
      this.expanded = !this.expanded;
    }
  }
}
