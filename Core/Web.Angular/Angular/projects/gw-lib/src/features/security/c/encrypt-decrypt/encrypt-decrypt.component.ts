import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { LoggingService } from '@Growthware/Lib/src/features/logging';
import { GWCommon } from '@Growthware/Lib/src/common-code';

@Component({
  selector: 'gw-lib-encrypt-decrypt',
  templateUrl: './encrypt-decrypt.component.html',
  styleUrls: ['./encrypt-decrypt.component.scss']
})
export class EncryptDecryptComponent implements OnInit {

  processedText: string = '';
  selectedEncryptionType: number = 3;
  textValue: string = '';

  validEncryptionTypes = [
    { id: 3, name: "Aes" },
    { id: 2, name: "Des" },
    { id: 1, name: "Triple Des" },
    { id: 0, name: "None" }
  ];

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
  ) { }

  ngOnInit(): void {
  }

  encryptDecrypt(encrypt: boolean) {
    const mQueryParameter = new HttpParams()
      .set('txtValue', this.textValue)
      .set('encryptionType', this.selectedEncryptionType)
      .set('encrypt', encrypt);
    const mHttpOptions: Object = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      responseType: 'text',
      params: mQueryParameter
    }
    const mUrl = this._GWCommon.baseURL + 'GrowthwareAccount/EncryptDecrypt';
    this._HttpClient.get<string>(mUrl, mHttpOptions).subscribe({
      next: (response: any) => {
        this.processedText = response;
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'EncryptDecryptComponent', 'encryptDecrypt');
        this.processedText = 'Failed to call the API';
      },
      // complete: () => {}
    });
  }

}
