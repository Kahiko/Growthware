import { Component, Input, OnInit } from '@angular/core';
import { LoginComponent } from '@Growthware/Lib/src/lib/features/account/c/login/login.component';
import { ModalService, ModalOptions, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
import { ConfigurationService } from '@Growthware/Lib/src/lib/services';
import { IAppSettings } from '@Growthware/Lib/src/lib/models/app-settings.model';

@Component({
  selector: 'gw-frontend-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent implements OnInit {
  applicationName: string = '';

  private mLoginId: string = 'loginModal';

  isAuthenticated: boolean = false;

  @Input() sidenav: any;

  constructor(private _ModalSvc: ModalService, private _ConfigurationSvc: ConfigurationService) { }

  ngOnInit(): void {
    this._ConfigurationSvc.getAppSettings().then((response: IAppSettings) => {
      this.applicationName = response.name;
    });
  }

  onLogin(): void {
    const mWindowSize: WindowSize = new WindowSize(325, 450);
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
