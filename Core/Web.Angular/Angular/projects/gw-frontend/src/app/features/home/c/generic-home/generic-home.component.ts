import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { AccountService } from '@Growthware/features/account';
import { ConfigurationService } from '@Growthware/features/configuration';
import { MenuType } from '@Growthware/features/navigation';

@Component({
  selector: 'gw-frontend-generic-home',
  templateUrl: './generic-home.component.html',
  styleUrls: ['./generic-home.component.scss']
})
export class GenericHomeComponent implements OnDestroy, OnInit {
  private _Subscription: Subscription = new Subscription();

  applicationName: string = '';

  constructor(
    private _AccountSvc: AccountService,
    private _ConfigurationSvc: ConfigurationService
  ) { }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    this._AccountSvc.getNavLinks(MenuType.Hierarchical);
    this._Subscription.add(
      this._ConfigurationSvc.applicationName$.subscribe((val: string) => { this.applicationName = val;})
    );
  }

}
