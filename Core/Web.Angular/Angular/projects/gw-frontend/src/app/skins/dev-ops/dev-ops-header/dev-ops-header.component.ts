import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { AccountService, IAccountInformation } from '@Growthware/features/account';
import { GWCommon } from '@Growthware/common-code';
import { LoginComponent } from '@Growthware/features/account/c/login/login.component';
import { ModalService, ModalOptions, WindowSize } from '@Growthware/features/modal';
import { ConfigurationService } from '@Growthware/features/configuration';

@Component({
  selector: 'gw-frontend-dev-ops-header',
  templateUrl: './dev-ops-header.component.html',
  styleUrls: ['./dev-ops-header.component.scss']
})
export class DevOpsHeaderComponent implements OnDestroy, OnInit {
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
    this._ConfigurationSvc.loadAppSettings();
    this._Subscription.add(
      this._ConfigurationSvc.applicationName$.subscribe((val: string) => { this.applicationName = val; })
    );
    this._Subscription.add(
      this._ConfigurationSvc.version$.subscribe((val: string) => { 
        this.version = val; 
      })
    );
    this._Subscription.add(
      this._AccountSvc.accountInformationChanged$.subscribe((val: IAccountInformation) => {
        this.isAuthenticated = val.authenticationResponse.account.toLowerCase() != this._AccountSvc.anonymous.toLowerCase();
        this.accountName = this._GWCommon.formatData(val.authenticationResponse.account, 'text:28');
      })
    );
  }

  onLogin(): void {
    const mWindowSize: WindowSize = new WindowSize(225, 450);
    const mModalOptions: ModalOptions = new ModalOptions(this._AccountSvc.logInModalId, 'Logon', LoginComponent, mWindowSize);
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this.onModalOk
    }
    this._ModalSvc.open(mModalOptions);
  }

  onLogoClick(): void {
    if(this._AccountSvc.authenticationResponse.account.trim().toLocaleLowerCase() !== this._AccountSvc.anonymous.trim().toLocaleLowerCase()) {
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