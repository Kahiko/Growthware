import { Component, Input, OnInit } from '@angular/core';
import { LoginComponent } from '@Growthware/Lib/src/lib/features/account/c/login/login.component';
import { ModalService, ModalOptions, ModalSize, WindowSize } from '@Growthware/Lib/src/lib/features/modal';

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
    const mWindowSize: WindowSize = new WindowSize(350, 500);
    const mModalOptions: ModalOptions = new ModalOptions(this.mLoginId, 'Login', LoginComponent, mWindowSize);
    // mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this.onModalOk
    }
    this._ModalSvc.open(mModalOptions);
    this.isAuthenticated = true;
  }

  onModalOk() {

  }

  onLogout(): void {
    this.isAuthenticated = false;
  }

}
