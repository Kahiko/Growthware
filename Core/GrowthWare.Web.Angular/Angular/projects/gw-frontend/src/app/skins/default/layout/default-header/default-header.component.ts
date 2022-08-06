import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'gw-frontend-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent implements OnInit {

  @Input() sidenav: any;

  constructor() { }

  ngOnInit(): void {
  }

}
