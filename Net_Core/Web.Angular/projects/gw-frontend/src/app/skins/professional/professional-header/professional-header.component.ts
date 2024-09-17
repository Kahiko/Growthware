import { Component, computed, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
// Library
import { AccountService, IAccountInformation } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';

@Component({
	selector: 'gw-frontend-professional-header',
	templateUrl: './professional-header.component.html',
	styleUrls: ['./professional-header.component.scss']
})
export class ProfessionalHeaderComponent implements OnDestroy, OnInit {

	private _Subscription: Subscription = new Subscription();

	environment: string = 'Development';
	name = computed(() => this._AccountSvc.clientChoices().accountName);
	securityEntityName = computed(() => this._AccountSvc.clientChoices().securityEntityName);
	securityEntityTranslation: string = '';
	version: string = '';
  
	constructor(
    private _AccountSvc: AccountService,
    private _ConfigurationSvc: ConfigurationService,
	) { }
  
	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
	}
	ngOnInit(): void {
		this._Subscription.add(this._ConfigurationSvc.securityEntityTranslation$.subscribe((val: string) => { this.securityEntityTranslation = val; }));
		this._Subscription.add( this._ConfigurationSvc.version$.subscribe((val: string) => { this.version = val; }));
		// this._Subscription.add(
		// 	this._AccountSvc.accountInformationChanged$.subscribe((val: IAccountInformation) => { 
		// 		this.securityEntityName = val.clientChoices.securityEntityName; 
		// 		this.name = val.clientChoices.accountName;
		// 	})
		// );
	}

}
