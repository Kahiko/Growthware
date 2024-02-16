import { Component, OnInit } from '@angular/core';

// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
// Library
import { AccountService, IClientChoices } from '@growthware/core/account';
import { LoggingService, LogLevel } from '@growthware/core/logging';
// Feature
import { SecurityEntityService } from '../../security-entity.service';
import { IValidSecurityEntities } from '../../valid-security-entities.model';

@Component({
	selector: 'gw-core-select-security-entity',
	standalone: true,
	imports: [
		MatButtonModule,
		MatFormFieldModule,
		MatIconModule,
		MatSelectModule
	],
	templateUrl: './select-security-entity.component.html',
	styleUrls: ['./select-security-entity.component.scss']
})
export class SelectSecurityEntityComponent implements OnInit {

	selectedSecurityEntity: number = -1;

	validSecurityEntities: IValidSecurityEntities[] = [];

	constructor(
    private _AccountSvc: AccountService,
    private _LoggingSvc: LoggingService,
    private _SecurityEntitySvc: SecurityEntityService
	) { }

	ngOnInit(): void {
		this._SecurityEntitySvc.getValidSecurityEntities().then((securityEntities: IValidSecurityEntities[]) => { 
			// console.log('SelectSecurityEntityComponent.ngOnInit', securityEntities);
			this.validSecurityEntities = securityEntities;
			this.selectedSecurityEntity = this._AccountSvc.clientChoices.securityEntityID;
		}).catch((error) => { 
			this._LoggingSvc.toast('Error calling the API', 'Select Security Entity', LogLevel.Error);
		});
	}

	public onSave(): void {
		// console.log('SelectSecurityEntityComponent.onSave');
		const mClientChoices: IClientChoices = JSON.parse(JSON.stringify(this._AccountSvc.clientChoices));
		mClientChoices.securityEntityID = this.selectedSecurityEntity;
		this._AccountSvc.saveClientChoices(mClientChoices).then(() => {
			this._LoggingSvc.toast('Selection saved', 'Select Security Entity', LogLevel.Success);
		}).catch(() => {
			this._LoggingSvc.toast('Unable to save client choices', 'Save client choices', LogLevel.Error);
		});
	}
}
