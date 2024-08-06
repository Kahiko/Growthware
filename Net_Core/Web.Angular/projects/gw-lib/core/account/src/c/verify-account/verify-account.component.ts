import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
// Library
import { GWCommon } from '@growthware/common/services';
// Feature
import { AccountService } from '../../account.service';

@Component({
	selector: 'gw-core-verify-account',
	standalone: true,
	imports: [],
	templateUrl: './verify-account.component.html',
	styleUrl: './verify-account.component.scss'
})
export class VerifyAccountComponent {

	constructor(
    private _AccountSvc: AccountService,
    private _ActivatedRoute: ActivatedRoute,
    private _GWCommon: GWCommon
	) { 
		this._ActivatedRoute.queryParams.subscribe((params) => {
			this._AccountSvc.verifyAccount(params['verificationToken']);
		});
	}

}
