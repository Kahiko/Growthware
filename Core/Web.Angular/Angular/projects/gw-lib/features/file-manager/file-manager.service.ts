import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
// Library
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
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
  private _Api_CreateDirectory: string = '';
  private _Api_DeleteDirectory: string = '';
  private _Api_DeleteFile: string = '';
  private _Api_RenameDirectory: string = '';
  private _Api_RenameFile: string = '';
  private _Api_UploadFile: string = '';
  private _CurrentDirectoryTree!: IDirectoryTree;
  private _SelectedPath: string = '\\';

  public set CurrentDirectoryTree(value: IDirectoryTree) {
    this._CurrentDirectoryTree = value;
  }

  public get SelectedPath(): string {
    return this._SelectedPath;
  };

  uploadStatusChanged:  Subject<IUploadStatus> = new Subject<IUploadStatus>();
  selectedDirectoryChanged:  Subject<IDirectoryTree> = new Subject<IDirectoryTree>();

  constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,    
  ) { 
    this._Api = this._GWCommon.baseURL + 'GrowthwareFile/';
    this._Api_GetDirectories = this._Api + 'GetDirectories';
    this._Api_GetFiles = this._Api + 'GetFiles';
    this._Api_CreateDirectory = this._Api + 'CreateDirectory';
    this._Api_DeleteDirectory = this._Api + 'DeleteDirectory';
    this._Api_DeleteFile = this._Api + 'DeleteFile';
    this._Api_RenameDirectory = this._Api + 'RenameDirectory';
    this._Api_RenameFile = this._Api + 'RenameFile';
    this._Api_UploadFile = this._Api + 'UploadFile';
  }

  /**
   * Creates a new directory in the currectly selected directory
   *
   * @param {string} action
   * @param {string} selectedPath
   * @param {string} newPath
   * @return {*}  {Promise<any>}
   * @memberof FileManagerService
   */
  async createDirectory(action: string, newPath: string): Promise<any>
  {
    if(this._GWCommon.isNullOrEmpty(action)) {
      throw new Error("action can not be blank!");
    }
    if(this._GWCommon.isNullOrEmpty(newPath)) {
      throw new Error("newPath can not be blank!");
    };
    let mQueryParameter: HttpParams = new HttpParams().append('action', action);
    mQueryParameter = mQueryParameter.append('selectedPath', this.SelectedPath);
    mQueryParameter = mQueryParameter.append('newPath', newPath);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<boolean>((resolve, reject) => {
      this._HttpClient.post<any>(this._Api_CreateDirectory, null, mHttpOptions).subscribe({
        next:( response: boolean ) => {
          resolve(response)
        },
        error:( error: any ) => {
          this._LoggingSvc.errorHandler(error, 'FileManagerService', 'createDirectory');
          reject(false);
        },
        complete:( ) => {}
      });
    });
  }

  /**
   * Deletes a directory, subdirectory and all files
   *
   * @param {string} action
   * @param {string} selectedPath
   * @return {*}  {Promise<boolean>}
   * @memberof FileManagerService
   */
  async deleteDirectory(action: string, selectedPath: string): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      if(this._GWCommon.isNullOrEmpty(action)) {
        throw new Error("action can not be blank!");
      }
      if(this._GWCommon.isNullOrEmpty(selectedPath)) {
        throw new Error("selectedPath can not be blank!");
      };
      let mQueryParameter: HttpParams = new HttpParams().append('action', action);
      mQueryParameter=mQueryParameter.append('selectedPath', selectedPath);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.delete<boolean>(this._Api_DeleteDirectory, mHttpOptions).subscribe({
        next:( response: boolean ) => {
          resolve(response)
        },
        error:( error: any ) => {
          this._LoggingSvc.errorHandler(error, 'FileManagerService', 'deleteFile');
          reject(false);
        },
        complete:( ) => {}
      });
    });
  }

  /**
   * @description Deletes a file from the currently selected path
   *
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @param {string} fileName The file name to delete
   * @return {*}  {Promise<boolean>}
   * @memberof FileManagerService
   */
  async deleteFile(action: string, fileName: string): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      if(this._GWCommon.isNullOrEmpty(action)) {
        throw new Error("action can not be blank!");
      }
      if(this._GWCommon.isNullOrEmpty(fileName)) {
        throw new Error("fileName can not be blank!");
      }
      let mQueryParameter: HttpParams = new HttpParams().append('action', action);
      mQueryParameter=mQueryParameter.append('selectedPath', this._SelectedPath);
      mQueryParameter=mQueryParameter.append('fileName', fileName);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.delete<boolean>(this._Api_DeleteFile, mHttpOptions).subscribe({
        next:( response: boolean ) => {
          resolve(response)
        },
        error:( error: any ) => {
          this._LoggingSvc.errorHandler(error, 'FileManagerService', 'deleteFile');
          reject(false);
        },
        complete:( ) => {}
      });
    });
  }

  /**
   * @description Retrieves an array of IDirectoryTree for the gien path
   *  
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @param {string} path The relative of the directory path 
   * @param {string} forControl - The control to notify.
   * @memberof FileManagerService
   * @returns {Promise<boolean>} A promise that resolves to a boolean indicating the success of the operation.
   */
  public async getDirectories(action: string, path: string, forControl: string): Promise<boolean> {
    let mQueryParameter: HttpParams = new HttpParams().append('action', action);
    mQueryParameter = mQueryParameter.append('selectedPath', path);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<boolean>((resolve, reject) => {
      this._HttpClient.get<IDirectoryTree>(this._Api_GetDirectories, mHttpOptions).subscribe({
        next: (response: IDirectoryTree) => {
          const mDirectoryTree = [];
          mDirectoryTree.push(response);
          this._DataSvc.notifyDataChanged(forControl, mDirectoryTree);
          resolve(true);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
          reject(false);
        },
        // complete: () => {}
      });
    });
  }

  /**
   * @description Retrives a array of IFileInfoLight for the given path
   *
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @param {string} controlId The id of the controler the files are for
   * @param {string} selectedPath The relative of the directory path 
   * @memberof FileManagerService
   */
  public getFiles(action: string, controlId: string, selectedPath: string) {
    let mQueryParameter: HttpParams = new HttpParams().append('action', action);
    mQueryParameter = mQueryParameter.append('selectedPath', selectedPath);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    this._SelectedPath = selectedPath;
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
   * @param {number} fileSize  The size of the file
   * @param {number} chunkSize The size of the chunck that the server can accept
   * @return {*}  {number}
   * @memberof FileManagerService
   */
  public getTotalNumberOfUploads(fileSize: number, chunkSize: number): number {
    const mRetVal = fileSize % chunkSize == 0 ? fileSize / chunkSize : Math.floor(fileSize / chunkSize) + 1;
    return mRetVal;    
  }

  /**
   * @description Uploads files that are too large to be sent in a single upload by using recursion
   * to call the API with a "slice" of the file.
   *
   * @private
   * @param {IMultiPartFileUploadParameters} parameters
   * @memberof FileManagerService
   */
  private multiPartFileUpload(parameters: IMultiPartFileUploadParameters) {
    const mParams = {...parameters}; // it's good practice to leave parameter values unchanged
    let mNextUploadNumber = mParams.uploadNumber + 1;
    // const mFileSize: number = mParams.file.size;
    const mMultiUploadFileName = mParams.file.name + "_UploadNumber_" + mNextUploadNumber;
    if(mParams.uploadNumber < mParams.totalNumberOfUploads) { // do we need to send any more pices of the file
      const mBlob: Blob = mParams.file.slice.call(mParams.file, mParams.startingByte, mParams.endingByte);
      const mFormData: FormData = new FormData();
      mFormData.append('action', mParams.action);
      mFormData.append('completed', 'false');
      mFormData.append('selectedPath', this._SelectedPath);
      mFormData.append(mMultiUploadFileName, mBlob);
      this._HttpClient.post<IUploadResponse>(this._Api_UploadFile, mFormData).subscribe({
        next: (response: IUploadResponse) => { // update the parameters can call this method again
          const mUploadStatus: IUploadStatus = new UploadStatus(mParams.action, response.fileName, response.data, false, response.isSuccess, mParams.totalNumberOfUploads, mParams.uploadNumber);
          mParams.uploadNumber = mNextUploadNumber;
          mParams.startingByte = parameters.endingByte;
          mParams.endingByte = parameters.endingByte + parameters.chunkSize
          this.uploadStatusChanged.next(mUploadStatus);
          this.multiPartFileUpload(mParams);
        },
        error: (error: any) => {
          if(parameters.retryNumber < 4) {
            mParams.retryNumber = mParams.retryNumber + 1;
            this.multiPartFileUpload(mParams);
          } else {
            this._LoggingSvc.errorHandler(error, 'FileManagementService', 'upload');
            // const mUploadStatus: IUploadStatus = new UploadStatus(mParams.action, mMultiUploadFileName, error, false, false, mParams.totalNumberOfUploads, mParams.uploadNumber);
            mParams.uploadNumber = mNextUploadNumber;
          }
        },
        // complete: () => {}
      });
    };
    if(mParams.uploadNumber == mParams.totalNumberOfUploads) { // make sure this is the last upload
      this.multiUploadComplete(mParams.action, mParams.file.name, this._Api_UploadFile).subscribe({
        next: (response: IUploadResponse) => {
          const mUploadStatus: IUploadStatus = new UploadStatus(mParams.action, response.fileName, response.data, true, response.isSuccess, mParams.totalNumberOfUploads, mParams.uploadNumber);
          this.uploadStatusChanged.next(mUploadStatus);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FileManagementService', 'upload');
          const mUploadStatus: IUploadStatus = new UploadStatus(mParams.action, mParams.file.name, error, true, false, mParams.totalNumberOfUploads, mParams.uploadNumber);
          this.uploadStatusChanged.next(mUploadStatus);
        },
        // complete: () => {}
      });
    }
  }

  /**
   * @description Helper method that calls the upload API so the merging of the file chunks (slices)
   * can be done
   *
   * @private
   * @param {string} action Used to determine the upload directory and enforce security on the server.
   * @param {string} fileName The file name for this Chunk.
   * @param {string} uri The Uniform Resource Identifier.
   * @return {*}  {Observable<IUploadResponse>}
   * @memberof FileManagerService
   */
  private multiUploadComplete(action: string, fileName: string, uri: string): Observable<IUploadResponse> {
    var mFormData = new FormData();
    mFormData.append('action', action);
    mFormData.append('completed', 'true');
    mFormData.append('fileName', fileName);
    mFormData.append('selectedPath', this._SelectedPath);
    return this._HttpClient.post<IUploadResponse>(uri, mFormData);    
  }

/**
 * @description Refreshes the specified action.
 *
 * @param {string} action - Used to determine the upload directory and enforce security on the server.
 * @param {IDirectoryTree} directoryTree - Optional directory tree to use to determine the selected path.
 * @return {Promise<any>} A Promise that resolves when the refresh is complete.
 * @memberof FileManagerService
 */
  async refresh(action: string, directoryTree?: IDirectoryTree): Promise<any> {
    const mAction = action.trim();
    let mSelectedNode = this._CurrentDirectoryTree;
    const mDirectoryControlName = mAction + '_Directories';
    const mFileControleName = mAction + '_Files';
    if(directoryTree != undefined) {
      mSelectedNode = directoryTree;
    }
    this.getDirectories(mAction, mDirectoryControlName, mSelectedNode.relitivePath).catch((error) => {
      this._LoggingSvc.errorHandler(error, 'FileManagerService', 'refresh/getDirectories');
    }).then((response: any) => {
      this.selectedDirectoryChanged.next(mSelectedNode);
      this._SelectedPath = mSelectedNode.relitivePath;
      this.getFiles(mAction, mFileControleName, this._SelectedPath);
    });
  }

  async renameDirectory(action: string, newName: string): Promise<boolean> {
    if(this._GWCommon.isNullOrEmpty(action)) {
      throw new Error("action can not be blank!");
    }
    if(this._GWCommon.isNullOrEmpty(newName)) {
      throw new Error("newName can not be blank!");
    }
    return new Promise<boolean>((resolve, reject) => {
      let mQueryParameter: HttpParams = new HttpParams().append('action', action);
      mQueryParameter=mQueryParameter.append('selectedPath', this._SelectedPath);
      mQueryParameter=mQueryParameter.append('newName', newName);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.post<boolean>(this._Api_RenameDirectory, null, mHttpOptions).subscribe({
        next:( response: boolean ) => {
          resolve(response)
        },
        error:( error: any ) => {
          this._LoggingSvc.errorHandler(error, 'FileManagerService', 'renameDirectory');
          reject(false);
        },
        complete:( ) => {}
      });
    });
  }

  /**
   * @description Renames an existing file
   *
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @param {string} oldName The name of the file to rename
   * @param {string} newName The new name of the file
   * @return {*}  {Promise<boolean>}
   * @memberof FileManagerService
   */
  async renameFile(action: string, oldName: string, newName: string): Promise<boolean> {
    if(this._GWCommon.isNullOrEmpty(action)) {
      throw new Error("action can not be blank!");
    }
    if(this._GWCommon.isNullOrEmpty(oldName)) {
      throw new Error("oldName can not be blank!");
    }
    if(this._GWCommon.isNullOrEmpty(newName)) {
      throw new Error("newName can not be blank!");
    }
    return new Promise<boolean>((resolve, reject) => {
      let mQueryParameter: HttpParams = new HttpParams().append('action', action);
      mQueryParameter=mQueryParameter.append('selectedPath', this._SelectedPath);
      mQueryParameter=mQueryParameter.append('oldName', oldName);
      mQueryParameter=mQueryParameter.append('newName', newName);
      const mHttpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        params: mQueryParameter,
      };
      this._HttpClient.post<boolean>(this._Api_RenameFile, null, mHttpOptions).subscribe({
        next:( response: boolean ) => {
          resolve(response)
        },
        error:( error: any ) => {
          this._LoggingSvc.errorHandler(error, 'FileManagerService', 'renameFile');
          reject(false);
        },
        complete:( ) => {}
      });
    });
  }

  /**
   * @description Performs a single file upload
   *
   * @private
   * @param {File} file the HTML "file" object
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @memberof FileManagerService
   */
  private singleFileUpload(file: File, action: string) {
    const mFormData = new FormData();
    mFormData.append('action', action);
    mFormData.append('selectedPath', this._SelectedPath);
    mFormData.append(file.name + '_UploadNumber_1', file);
    mFormData.append('fileName', file.name);
    mFormData.append('completed', 'true');
    this._HttpClient.post<IUploadResponse>(this._Api_UploadFile, mFormData).subscribe({
      next: (response: IUploadResponse) => {
        const mUploadStatus: IUploadStatus = new UploadStatus(action, response.fileName, response.data, true, response.isSuccess, 1, 1);
        this.uploadStatusChanged.next(mUploadStatus);
      },
      error: (error: any) => {
        this._LoggingSvc.errorHandler(error, 'FileManagerService', 'singleFileUpload');
        // const mUploadStatus: IUploadStatus = new UploadStatus(action, file.name, error, true, false, 1, 1);
      },
      // complete: () => {}
    });
  }

  public setSelectedDirectory(directoryTree: IDirectoryTree): void {
    this._CurrentDirectoryTree = directoryTree;
    this.selectedDirectoryChanged.next(directoryTree);
  }
  
  /**
   * @description Uploads file by calling either multiPartFileUpload or singleFileUpload depending on the file size.
   *
   * @param {string} action Used to determine the upload directory and enforce security on the server
   * @param {File} file An HTML "File" object
   * @param {number} [chunkSize=3072000] Used to break the "File" up if the file size is greater than the chunkSize.  The number is in direct relation to KestrelServerLimits.MaxRequestBodySize Property
   * @memberof FileManagerService
   */
  uploadFile(action: string, file: File, chunkSize: number = 29696000)
  {
    const mTotalNumberOfUploads: number = this.getTotalNumberOfUploads(file.size, chunkSize);
    if (mTotalNumberOfUploads > 1) {
      const mMultiPartFileUpload: IMultiPartFileUploadParameters = new MultiPartFileUploadParameters(
        action,
        file,
        mTotalNumberOfUploads,
        chunkSize
      )
      this.multiPartFileUpload(mMultiPartFileUpload);
    } else{
      this.singleFileUpload(file, action);
    }
  }
}
