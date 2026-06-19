import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

// Feature
import { AccountService } from '../../account.service';

@Component({
    selector: 'gw-core-logout',
    imports: [],
    templateUrl: './logout.component.html',
    changeDetection: ChangeDetectionStrategy.Eager,
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
