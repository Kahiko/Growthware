import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { AccountService, IAccountInformation } from '@Growthware/features/account';
import { ConfigurationService } from '@Growthware/features/configuration';
import { GWCommon } from '@Growthware/common-code';

@Component({
  selector: 'gw-frontend-blue-arrow-layout',
  templateUrl: './blue-arrow-layout.component.html',
  styleUrls: ['./blue-arrow-layout.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class BlueArrowLayoutComponent implements OnDestroy, OnInit {

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
      this._AccountSvc.accountInformationChanged$.subscribe((val: IAccountInformation) => {
        this.accountName = this._GWCommon.formatData(val.authenticationResponse.account, 'text:28');
      })
    );
    this._Subscription.add(
      this._AccountSvc.accountInformationChanged$.subscribe((val: IAccountInformation) => { this.securityEntity = val.clientChoices.securityEntityName; })
    );
    // this._Subscription.add(
    //   this._NavigationSvc.currentNavLink$.subscribe((val: INavLink) => { 
    //     // console.log('ArcHeaderComponent.ngOnInit.description', val.description);
    //     if(val.description.length > 0) {
    //       this.navDescription = val.description; 
    //     } else {
    //       this.navDescription = 'Home';
    //     }
    //   })
    // );
  }

}
