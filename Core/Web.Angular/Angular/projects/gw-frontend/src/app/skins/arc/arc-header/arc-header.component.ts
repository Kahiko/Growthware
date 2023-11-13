import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { AccountService, IAuthenticationResponse, IClientChoices } from '@Growthware/features/account';
import { GWCommon } from '@Growthware/common-code';
import { ConfigurationService } from '@Growthware/features/configuration';
// import { LoginComponent } from '@Growthware/features/account/c/login/login.component';
// import { ModalService, ModalOptions, WindowSize } from '@Growthware/features/modal';
import { INavLink, NavigationService } from '@Growthware/features/navigation';
// Skin

@Component({
  selector: 'gw-frontend-arc-header',
  templateUrl: './arc-header.component.html',
  styleUrls: ['./arc-header.component.scss']
})
export class ArcHeaderComponent implements OnDestroy, OnInit {
  private _Subscription: Subscription = new Subscription();

  accountName: string = '';
  applicationName: string = '';
  environment: string = 'Development';
  navDescription: string = '';
  securityEntity: string = '';
  securityEntityTranslation: string = '';
  version: string = '';

  constructor(
    private _AccountSvc: AccountService,
    private _ConfigurationSvc: ConfigurationService,
    private _GWCommon: GWCommon,
    // private _ModalSvc: ModalService,
    private _NavigationSvc: NavigationService,
    private _Router: Router
  ) {
    // do nothing atm
  }
  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscription.add(
      this._ConfigurationSvc.securityEntityTranslation$.subscribe((val: string) => { this.securityEntityTranslation = val;})
    );    
    this._Subscription.add(
      this._ConfigurationSvc.applicationName$.subscribe((val: string) => { this.applicationName = val; })
    );
    this._Subscription.add(
      this._ConfigurationSvc.version$.subscribe((val: string) => { this.version = val; })
    );
    this._Subscription.add(
      this._AccountSvc.authenticationResponse$.subscribe((val: IAuthenticationResponse) => {
        // this.isAuthenticated = val.account.toLowerCase() != this._AccountSvc.defaultAccount.toLowerCase();
        this.accountName = this._GWCommon.formatData(val.account, 'text:28');
      })
    );
    this._Subscription.add(
      this._AccountSvc.clientChoices$.subscribe((val: IClientChoices) => { this.securityEntity = val.securityEntityName; })
    );
    this._Subscription.add(
      this._NavigationSvc.currentNavLink$.subscribe((val: INavLink) => { 
        // console.log('ArcHeaderComponent.ngOnInit.description', val.description);
        if(val.description.length > 0) {
          this.navDescription = val.description; 
        } else {
          this.navDescription = 'Home';
        }
      })
    );
  }

  onLogoClick(): void {
    if(this._AccountSvc.authenticationResponse.account.trim().toLocaleLowerCase() !== this._AccountSvc.defaultAccount.trim().toLocaleLowerCase()) {
      this._Router.navigate(['home']);
    } else {
      this._Router.navigate(['generic_home']);
    }
  }

}
