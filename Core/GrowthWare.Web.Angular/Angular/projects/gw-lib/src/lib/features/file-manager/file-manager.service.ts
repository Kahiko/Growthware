import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
// Library
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService } from '@Growthware/Lib/src/lib/features/logging';
// Feature
import { IDirectoryTree } from './directory-tree.model';
import { IFileInfoLight } from './file-info-light.model';
import { IMultiPartFileUploadParameters, MultiPartFileUploadParameters } from './multi-part-file-upload-parameters.model';
import { IUploadResponse } from './upload-response.model';
import { IUploadStatus, UploadStatus } from './upload-status.model';

@Injectable({
  providedIn: 'root'
})
export class FileManagerService {
  private _Api: string = '';
  private _Api_GetDirectories: string = '';
  private _Api_GetFiles: string = '';
  private _Api_UploadFile: string = '';
  private _SelectedPath: string = '\\';

  uploadStatusChanged:  Subject<IUploadStatus> = new Subject<IUploadStatus>();
  get selectedPath(){
    return this._SelectedPath;
  };

  constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,    
  ) { 
    this._Api = this._GWCommon.baseURL + 'GrowthwareFile/';
    this._Api_GetDirectories = this._Api + 'GetDirectories';
    this._Api_GetFiles = this._Api + 'GetFiles';
    this._Api_UploadFile = this._Api + 'UploadFile';
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

  /**
   *
   *
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @param {string} controlId
   * @param {string} path
   * @memberof FileManagerService
   */
  public getFiles(action: string, controlId: string, path: string) {
    let mQueryParameter: HttpParams = new HttpParams().append('action', action);
    mQueryParameter = mQueryParameter.append('path', path);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    this._SelectedPath = path;
    this._HttpClient.get<IFileInfoLight[]>(this._Api_GetFiles, mHttpOptions).subscribe({
      next: (response) => {
        this._DataSvc.notifyDataChanged(controlId, response);
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getFiles');
      },
      // complete: () => {}
    });
  }

  /**
   * @description Calculates the total number of chuncks needed to upload a large file
   *
   * @param {number} fileSize
   * @param {number} chunkSize
   * @return {*}  {number}
   * @memberof FileManagerService
   */
  public getTotalNumberOfUploads(fileSize: number, chunkSize: number): number {
    const mRetVal = fileSize % chunkSize == 0 ? fileSize / chunkSize : Math.floor(fileSize / chunkSize) + 1;
    return mRetVal;    
  }

  private multiPartFileUpload(parameters: IMultiPartFileUploadParameters) {

  }

  private multiUploadComplete(fileName: string, uri: string): Observable<IUploadResponse> {
    var mFormData = new FormData();
    mFormData.append('fileName', fileName);
    mFormData.append('completed', 'true');
    return this._HttpClient.post<IUploadResponse>(uri, mFormData);    
  }

  /**
   * @description Performs a single file upload
   *
   * @private
   * @param {File} file the HTML "file" object
   * @param {string} selectedPath The selected path the file is bing uploaded to
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @memberof FileManagerService
   */
  private singleFileUpload(file: File, selectedPath: string, action: string) {
    const mFormData = new FormData();
    mFormData.append('action', action);
    mFormData.append('selectedPath', selectedPath);
    mFormData.append(file.name, file);
    mFormData.append('complete', 'true');
    this._HttpClient.post<IUploadResponse>(this._Api_UploadFile, mFormData).subscribe({
      next: (response: IUploadResponse) => {
        const mUploadStatus: IUploadStatus = new UploadStatus(action, response.fileName, response.data, true, response.isSuccess, 1, 1);
        this.uploadStatusChanged.next(mUploadStatus);
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'FileManagerService', 'singleFileUpload');
        const mUploadStatus: IUploadStatus = new UploadStatus(action, file.name, error, true, false, 1, 1);
      },
      // complete: () => {}
    });
  }
  
  /**
   * @description Uploads file by calling either multiPartFileUpload or singleFileUpload depending on the file size.
   *
   * @param {string} action Passed along to the API and used to enforce security on the server
   * @param {string} selectedPath The currently selected path
   * @param {File} file An HTML "File" object
   * @param {number} [chunkSize=3072000] Used to break the "File" up if the file size is greater than the chunkSize.  The number is in direct relation to KestrelServerLimits.MaxRequestBodySize Property
   * @memberof FileManagerService
   */
  uploadFile(action: string, selectedPath: string, file: File, chunkSize: number = 3072000)
  {
    const mTotalNumberOfUploads: number = this.getTotalNumberOfUploads(file.size, chunkSize);
    if (mTotalNumberOfUploads > 1) {
      const mMultiPartFileUpload: IMultiPartFileUploadParameters = new MultiPartFileUploadParameters(action, file, mTotalNumberOfUploads);
      mMultiPartFileUpload.selectedPath = selectedPath;
      this.multiPartFileUpload(mMultiPartFileUpload);
    } else{
      this.singleFileUpload(file, selectedPath, action);
    }
  }
}
