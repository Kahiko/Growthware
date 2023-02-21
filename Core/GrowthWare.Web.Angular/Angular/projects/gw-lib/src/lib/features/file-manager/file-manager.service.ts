import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService } from '@Growthware/Lib/src/lib/features/logging';
// Feature
import { IDirectoryTree } from './directory-tree.model';

export interface IFileInfoLight {
  creationTime: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class FileManagerService {
  private _Api: string = '';
  private _Api_GetDirectories: string = '';
  private _Api_GetFiles: string = '';

  constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,    
  ) { 
    this._Api = this._GWCommon.baseURL + 'GrowthwareFile/';
    this._Api_GetDirectories = this._Api + 'GetDirectories';
    this._Api_GetFiles = this._Api + 'GetFiles';
  }

  public getDirectories(action: string, name: string): void {
    const mQueryParameter: HttpParams = new HttpParams().append('action', action);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    this._HttpClient.get<IDirectoryTree>(this._Api_GetDirectories, mHttpOptions).subscribe({
      next: (response: IDirectoryTree) => {
        const mDirectoryTree = [];
        mDirectoryTree.push(response);
        this._DataSvc.notifyDataChanged(name, mDirectoryTree);
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
      },
      // complete: () => {}
    });
  }

  public getFiles(action: string, name: string, path: string) {
    let mQueryParameter: HttpParams = new HttpParams().append('action', action);
    mQueryParameter = mQueryParameter.append('path', path);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    this._HttpClient.get<IFileInfoLight[]>(this._Api_GetFiles, mHttpOptions).subscribe({
      next: (response) => {
        // console.log('response', response);
        this._DataSvc.notifyDataChanged(name, response);
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getFiles');
      },
      // complete: () => {}
    });
  }
}
