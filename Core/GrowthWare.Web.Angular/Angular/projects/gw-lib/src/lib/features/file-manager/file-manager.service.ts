import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService } from '@Growthware/Lib/src/lib/features/logging';
// Feature
import { IDirectoryTree, DirectoryTree } from './directory-tree.model';

@Injectable({
  providedIn: 'root'
})
export class FileManagerService {
  private _Api: string = '';
  private _Api_GetDirectories: string = '';

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,    
  ) { 
    this._Api = this._GWCommon.baseURL + 'GrowthwareFile/';
    this._Api_GetDirectories = this._Api + 'GetDirectories';
  }

  public async getDirectories(functionSeqId: number): Promise<Array<IDirectoryTree>> {
    const mQueryParameter: HttpParams = new HttpParams().append('functionSeqId', functionSeqId);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<Array<IDirectoryTree>>((resolve, reject) => {
      this._HttpClient.get<IDirectoryTree>(this._Api_GetDirectories, mHttpOptions).subscribe({
        next: (response: IDirectoryTree) => {
          // console.log(response);
          const mDirectoryTree = [];
          mDirectoryTree.push(response);
          resolve(mDirectoryTree);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }
}
