import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { ClientChoices } from './client-choices.model';

@Injectable({
  providedIn: 'root'
})
export class ClientChoicesService {
  private _HttpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this._HttpClient = httpClient;
  }

  getClientChoices(accountId: number) {

  }

  updateClientChoices(clientChoices: ClientChoices) {

  }
}
