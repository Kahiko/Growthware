import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

// Library
import { AccountService, IAuthenticationResponse } from '@Growthware/Lib/src/lib/features/account';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
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

  accountName: string = '';
  applicationName: string = '';
  isAuthenticated: boolean = false;
  version: string = '';

  @Input() sidenav: any;

  constructor(
    private _AccountSvc: AccountService,
    private _ConfigurationSvc: ConfigurationService,
    private _GWCommon: GWCommon,
    private _ModalSvc: ModalService,
    private _Router: Router) { }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscription.add(
      this._ConfigurationSvc.applicationName$.subscribe((val: string) => { this.applicationName = val; })
    );
    this._Subscription.add(
      this._ConfigurationSvc.version$.subscribe((val: string) => { this.version = val; })
    );
    this._Subscription.add(
      this._AccountSvc.authenticationResponse$.subscribe((val: IAuthenticationResponse) => {
        this.isAuthenticated = val.account.toLowerCase() != this._AccountSvc.defaultAccount.toLowerCase();
        this.accountName = this._GWCommon.formatData(val.account, 'text:28');
      })
    );
  }

  onLogin(): void {
    const mWindowSize: WindowSize = new WindowSize(225, 450);
    const mModalOptions: ModalOptions = new ModalOptions(this._AccountSvc.loginModalId, this._AccountSvc.loginModalId, LoginComponent, mWindowSize);
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this.onModalOk
    }
    this._ModalSvc.open(mModalOptions);
  }

  onLogoClick(): void {
    if(this._AccountSvc.authenticationResponse.account.trim().toLocaleLowerCase() !== this._AccountSvc.defaultAccount.trim().toLocaleLowerCase()) {
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