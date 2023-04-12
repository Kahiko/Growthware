import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'gw-lib-encrypt-decrypt',
  templateUrl: './encrypt-decrypt.component.html',
  styleUrls: ['./encrypt-decrypt.component.scss']
})
export class EncryptDecryptComponent implements OnInit {

  encryptedText: string = '';
  decryptedText: string = '';
  saltExpression: string = '';
  selectedEncryptionType: number = 3;

  validEncryptionTypes  = [
    { id: 3, name: "Aes" },
    { id: 2, name: "Des" },
    { id: 1, name: "Triple Des" },
    { id: 0, name: "None" }
  ];

  constructor() { }

  ngOnInit(): void {
  }

  onEncrypt() {

  }

  onDecrypt() {

  }

}
