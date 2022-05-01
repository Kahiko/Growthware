import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import {HttpHeaders} from '@angular/common/http'

export class DynamicTableService {
  private _HttpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this._HttpClient = httpClient;
  }
}
