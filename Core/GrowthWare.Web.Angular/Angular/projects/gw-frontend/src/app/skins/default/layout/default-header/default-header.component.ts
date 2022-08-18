import { Component, Input, OnInit } from '@angular/core';
import { ModalService, ModalOptions, ModalSize } from '@Growthware/Lib';

@Component({
  selector: 'gw-frontend-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent implements OnInit {
  private mLoginId: string = 'loginModal';

  isAuthenticated: boolean = false;

  @Input() sidenav: any;

  constructor(private _ModalSvc: ModalService) { }

  ngOnInit(): void {
  }

  onLogin(): void {
    const mModalOptions: ModalOptions = new ModalOptions(this.mLoginId, 'Login', 'account/password and buttons', ModalSize.Normal);
    this._ModalSvc.open(mModalOptions);
    this.isAuthenticated = true;
  }

  onLogout(): void {
    this.isAuthenticated = false;
  }

}
