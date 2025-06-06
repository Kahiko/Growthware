import { Component, OnInit } from '@angular/core';

// Feature
import { AccountService } from '../../account.service';

@Component({
	selector: 'gw-core-logout',
	standalone: true,
	imports: [],
	templateUrl: './logout.component.html',
	styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

	constructor(
    private _AccountSvc: AccountService
	) { }

	ngOnInit(): void {
		this._AccountSvc.logout();
	}

}
