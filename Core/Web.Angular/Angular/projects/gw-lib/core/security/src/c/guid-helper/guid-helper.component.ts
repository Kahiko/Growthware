
import { Component, OnInit } from '@angular/core';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
// Feature
import { SecurityService } from '../../security.service';
import { LoggingService } from '@growthware/core/logging';

@Component({
	selector: 'gw-core-guid-helper',
	standalone: true,
	imports: [
		MatButtonModule
	],
	templateUrl: './guid-helper.component.html',
	styleUrls: ['./guid-helper.component.scss']
})
export class GuidHelperComponent implements OnInit {

	guidText: string = '';

	constructor(
    private _LoggingSvc: LoggingService,
    private _SecuritySvc: SecurityService
	) { }

	ngOnInit(): void {
	}

	onGuid(): void {
		this._SecuritySvc.getGuid().catch((error)=>{
			this._LoggingSvc.errorHandler(error, 'GuidHelperComponent', 'onGuid');
		}).then((response)=>{
			if(response) {
				this.guidText = response;
			}
		});
	}

}
