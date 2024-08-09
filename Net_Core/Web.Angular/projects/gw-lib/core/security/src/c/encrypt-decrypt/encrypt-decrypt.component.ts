
import { FormsModule } from '@angular/forms';
import { Component } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { GWCommon } from '@growthware/common/services';

@Component({
	selector: 'gw-core-encrypt-decrypt',
	standalone: true,
	imports: [
		FormsModule,
		MatButtonModule,
		MatFormFieldModule,
		MatSelectModule,
		MatTabsModule
	],
	templateUrl: './encrypt-decrypt.component.html',
	styleUrls: ['./encrypt-decrypt.component.scss']
})
export class EncryptDecryptComponent {

	processedText: string = '';
	selectedEncryptionType: number = 3;
	textValue: string = '';

	validEncryptionTypes = [
		{ id: 3, name: 'Aes' },
		{ id: 2, name: 'Des' },
		{ id: 1, name: 'Triple Des' },
		{ id: 0, name: 'None' }
	];

	constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
	) { }

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

	encryptDecrypt(encrypt: boolean) {
		const mQueryParameter = new HttpParams()
			.set('txtValue', this.textValue)
			.set('encryptionType', this.selectedEncryptionType)
			.set('encrypt', encrypt);
		// could not use an object for the HttpOptions b/c it produces an error reguarding responseType
		const mUrl = this._GWCommon.baseURL + 'GrowthwareAccount/EncryptDecrypt';
		this._HttpClient.get(mUrl, { responseType: 'text', params: mQueryParameter }).subscribe({
			next: (response) => {
				this.processedText = response;
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'EncryptDecryptComponent', 'encryptDecrypt');
				this.processedText = 'Failed to call the API';
			},
			// complete: () => {}
		});
	}

	swap(): void {
		// const mTextValue = this.textValue;
		const mProcessedText = this.processedText;
		this.textValue = mProcessedText;
		this.processedText = '';
	}
}
