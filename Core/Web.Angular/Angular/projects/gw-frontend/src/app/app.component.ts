import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { AccountService, IAccountInformation, IClientChoices, SecurityEntityService } from '@Growthware';

@Component({
  selector: 'gw-frontend-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnDestroy, OnInit {

  private _Skin: BehaviorSubject<string> = new BehaviorSubject<string>('default');
  private _Subscription: Subscription = new Subscription();

  readonly skin$ = this._Skin.asObservable();
  skin = 'default';
  title = 'gw-frontend';

  constructor(
    private _AccountSvc: AccountService,
    private _SecurityEntitySvc: SecurityEntityService
  ) {}

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  ngOnInit(): void {
    this._Subscription.add(
      this._AccountSvc.accountInformationChanged$.subscribe((val: IAccountInformation) => {
        // console.log('AppComponent.ngOnInit.val', val);
        this.setSkin(val.clientChoices);
      })
    );
  }

  private setSkin(clientChoices: IClientChoices): void {
    this._SecurityEntitySvc.getSecurityEntity(clientChoices.securityEntityID).then((response: any) => {
      // console.log('AppComponent.setSkin.response', response);
      this._Skin.next(response.skin.toLowerCase());
    })
  }
}
