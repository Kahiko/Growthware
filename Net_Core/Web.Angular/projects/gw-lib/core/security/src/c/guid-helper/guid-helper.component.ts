import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
// Feature
import { SecurityService } from '../../security.service';
import { LoggingService, LogLevel } from '@growthware/core/logging';

@Component({
	selector: 'gw-core-guid-helper',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,

		MatButtonModule,
		MatTabsModule
	],
	templateUrl: './guid-helper.component.html',
	styleUrls: ['./guid-helper.component.scss']
})
export class GuidHelperComponent {

	guidText: string = '';

	constructor(
    private _LoggingSvc: LoggingService,
    private _SecuritySvc: SecurityService
	) { }

	onGuid(): void {
		this._SecuritySvc.getGuid().catch((error)=>{
			this._LoggingSvc.errorHandler(error, 'GuidHelperComponent', 'onGuid');
		}).then((response)=>{
			if(response) {
				this.guidText = response;
			}
		});
	}

	copyInputMessage(inputElement: HTMLTextAreaElement) {
		inputElement.select();
		navigator.clipboard.writeText(inputElement.value).then(() => {
			this._LoggingSvc.toast('Text copied to clipboard', 'Encrypt/Decrypt', LogLevel.Info);
			console.log('Text copied to clipboard');
		}, (err) => {
			console.error('Could not copy text: ', err);
			this._LoggingSvc.toast('Text copied to clipboard', 'Encrypt/Decrypt', LogLevel.Error);
		});
		inputElement.setSelectionRange(0, 0);
	}
}
