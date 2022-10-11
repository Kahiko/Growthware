import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

// Library
import { AccountService } from '@Growthware/Lib/src/lib/features/account';
import { LoginComponent } from '@Growthware/Lib/src/lib/features/account/c/login/login.component';
import { ModalService, ModalOptions, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
import { ConfigurationService } from '@Growthware/Lib/src/lib/services';

@Component({
  selector: 'gw-frontend-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent implements OnDestroy, OnInit {
  private _Subscription: Subscription = new Subscription();

  applicationName: string = '';
  isAuthenticated: boolean = false;
  version: string = '';

  @Input() sidenav: any;

  constructor(
    private _AccountSvc: AccountService,
    private _ModalSvc: ModalService,
    private _ConfigurationSvc: ConfigurationService,
    private _Router: Router) { }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscription.add(
      this._ConfigurationSvc.applicationName.subscribe((val: string) => { this.applicationName = val; })
    );
    this._Subscription.add(
      this._ConfigurationSvc.version.subscribe((val: string) => { this.version = val; })
    );
    this._Subscription.add(
      this._AccountSvc.isAuthenticated.subscribe((val: boolean) => {
        this.isAuthenticated = val;
      })
    );
  }

  onLogin(): void {
    const mWindowSize: WindowSize = new WindowSize(325, 450);
    const mModalOptions: ModalOptions = new ModalOptions(this._AccountSvc.loginModalId, this._AccountSvc.loginModalId, LoginComponent, mWindowSize);
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this.onModalOk
    }
    this._ModalSvc.open(mModalOptions);
  }

  onLogoClick(): void {
    if(this._AccountSvc.account.trim().toLocaleLowerCase() !== this._AccountSvc.defaultAccount.trim().toLocaleLowerCase()) {
      this._Router.navigate(['home']);
    } else {
      this._Router.navigate(['generic_home']);
    }
  }

  onModalOk() {

  }

  onLogout(): void {
    this._AccountSvc.logout();
  }

}
