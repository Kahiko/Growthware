import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
// Library
import { GWCommon } from '@growthware/common/services';
import { DirectoryTree, IDirectoryTree } from '@growthware/common/interfaces';
import { LoggingService, LogLevel } from '@growthware/core/logging';
// Feature
import { IFileInfoLight } from './interfaces/file-info-light.model';
import { IMultiPartFileUploadParameters, MultiPartFileUploadParameters } from './interfaces/multi-part-file-upload-parameters.model';
import { IUploadResponse } from './interfaces/upload-response.model';
import { IUploadStatus, UploadStatus } from './interfaces/upload-status.model';

@Injectable({
	providedIn: 'root'
})
export class FileManagerService {
	private _Api: string = '';
	private _Api_GetDirectories: string = '';
	private _Api_GetFile: string = '';
	private _Api_GetFiles: string = '';
	private _Api_CreateDirectory: string = '';
	private _Api_DeleteDirectory: string = '';
	private _Api_DeleteFile: string = '';
	private _Api_RenameDirectory: string = '';
	private _Api_RenameFile: string = '';
	private _Api_UploadFile: string = '';
	private _SelectedPath: string = '\\';

	ModalId_Rename_Directory: string = 'DirectoryTreeComponent.onMenuRenameClick';
	ModalId_CreateDirectory: string = 'CreateDirectoryForm';

	public get selectedPath(): string {
		return this._SelectedPath;
	}

	public readonly filesChanged$ = signal<Array<IFileInfoLight>>([] as Array<IFileInfoLight>);

	public directoriesChanged$ = signal<Array<IDirectoryTree>>([] as Array<IDirectoryTree>);

	public needToExpand: boolean = false;

	public selectedDirectoryChanged$ = signal<IDirectoryTree>({} as IDirectoryTree);

	public readonly uploadStatusChanged$ = signal<IUploadStatus>({ id: '' } as unknown as IUploadStatus);

	constructor(
		// private _DataSvc: DataService,
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
	) {
		this._Api = this._GWCommon.baseURL + 'GrowthwareFile/';
		this._Api_GetDirectories = this._Api + 'GetDirectories';
		this._Api_GetFile = this._Api + 'GetFile';
		this._Api_GetFiles = this._Api + 'GetFiles';
		this._Api_CreateDirectory = this._Api + 'CreateDirectory';
		this._Api_DeleteDirectory = this._Api + 'DeleteDirectory';
		this._Api_DeleteFile = this._Api + 'DeleteFile';
		this._Api_RenameDirectory = this._Api + 'RenameDirectory';
		this._Api_RenameFile = this._Api + 'RenameFile';
		this._Api_UploadFile = this._Api + 'UploadFile';
	}
	
	/********************************************* Directory related Functions **********************************************/

	public async createDirectory(action: string, newPath: string): Promise<boolean> {
		if (this._GWCommon.isNullOrEmpty(action)) {
			throw new Error('action can not be blank!');
		}
		if (this._GWCommon.isNullOrEmpty(newPath)) {
			throw new Error('newPath can not be blank!');
		}
		return new Promise((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.append('action', action)
				.append('selectedPath', this.selectedDirectoryChanged$().relitivePath)
				.append('newPath', newPath);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			const mCurrentPath = this.selectedDirectoryChanged$().relitivePath;
			this._HttpClient.post<never>(this._Api_CreateDirectory, null, mHttpOptions).subscribe({
				next: (response: boolean) => {
					this.getDirectories(action, this.selectedDirectoryChanged$().relitivePath).then(() => {
						this.needToExpand = true;
						this.setSelectedDirectory(mCurrentPath);
						resolve(response);
					}).catch((error) => {
						this._LoggingSvc.errorHandler(error, 'FileManagerService', 'createDirectory');
					});
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FileManagerService', 'createDirectory');
					reject(false);
				},
				complete: () => { }
			});
		});
	}

	/**
	 * Deletes a directory from the server, updates the local directory tree, fires directoriesChanged$.update, calls setSelectedDirectory, and returns a promise of the result
	 * @param {string} action - 
	 * @param {string} selectedPath - 
	 * @return {Promise<boolean>} 
	 */
	public async deleteDirectory(action: string, selectedPath: string): Promise<boolean> {
		return new Promise((resolve, reject) => {
			const mDirectory = this._GWCommon.hierarchySearch(this.directoriesChanged$(), selectedPath, 'relitivePath') as IDirectoryTree;
			if (mDirectory) {
				const mQueryParameter: HttpParams = new HttpParams()
					.append('action', action)
					.append('selectedPath', selectedPath);
				const mHttpOptions = {
					headers: new HttpHeaders({
						'Content-Type': 'application/json',
					}),
					params: mQueryParameter,
				};
				this._HttpClient.delete<boolean>(this._Api_DeleteDirectory, mHttpOptions).subscribe({
					next: (response: boolean) => {
						if(response === true) {
							const mNewDirectoryArray = JSON.parse(JSON.stringify(this.directoriesChanged$()));
							this._GWCommon.hierarchyRemoveItem(mNewDirectoryArray, selectedPath, 'relitivePath');
							this.directoriesChanged$.update(() => mNewDirectoryArray);
							this.setSelectedDirectory(mDirectory.parentRelitivePath);
						}
						resolve(response);
					},
					error: (error) => {
						this._LoggingSvc.errorHandler(error, 'FileManagerService', 'deleteFile');
						reject(false);
					},
				//complete: () => { }
				});
			}			
		});
	}

	/**
	 * @description Retrieves an array of IDirectoryTree for the gien path
	 *  
	 * @param {string} action Used to determine the upload directory and enforce security on the server
	 * @param {string} path The relative of the directory path 
	 * @memberof FileManagerService
	 * @returns {Promise<boolean>} A promise that resolves to a boolean indicating the success of the operation.
	 */
	public async getDirectories(action: string, path: string): Promise<boolean> {
		return new Promise((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams()
				.append('action', action)
				.append('selectedPath', path);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.get<IDirectoryTree>(this._Api_GetDirectories, mHttpOptions).subscribe({
				next: (response: IDirectoryTree) => {
					const mDirectoryTree: Array<IDirectoryTree> = [];
					mDirectoryTree.push(response);
					this.directoriesChanged$.update(() => mDirectoryTree);
					resolve(true);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
					reject(false);
				},
			// complete: () => {}
			});
		});
	}

	refresh(action: string): void {
		this.getDirectories(action, this.selectedPath);
		this.getFiles(action, this.selectedPath);
	}

	/**
	 * Renames a directory, updates the local directory tree, and calls setSelectedDirectory.
	 *
	 * @param {string} action - description of parameter
	 * @param {string} newName - description of parameter
	 * @return {Promise<boolean>} description of return value
	 */
	public async renameDirectory(action: string, newName: string): Promise<boolean> {
		const mOriginalDirectory = this.selectedDirectoryChanged$();
		const mNewDirectory = JSON.parse(JSON.stringify(mOriginalDirectory));
		return new Promise((resolve, reject) => {
			if(mOriginalDirectory) {
				const mQueryParameter: HttpParams = new HttpParams().set('action', action)
					.set('selectedPath', mOriginalDirectory.relitivePath)
					.set('newName', newName);
				const mHttpOptions = {
					headers: new HttpHeaders({
						'Content-Type': 'application/json',
					}),
					params: mQueryParameter,
				};
				this._HttpClient.post<boolean>(this._Api_RenameDirectory, null, mHttpOptions).subscribe({
					next: () => {
						const mLastIndex = mOriginalDirectory.relitivePath.lastIndexOf(mOriginalDirectory.name);
						const mNewRelitivePath = mOriginalDirectory.relitivePath.substring(0, mLastIndex) + mOriginalDirectory.relitivePath.substring(mLastIndex).replace(mOriginalDirectory.name, newName);
						mNewDirectory.key = mOriginalDirectory.key.replace(mOriginalDirectory.name, newName);
						mNewDirectory.name = newName;
						mNewDirectory.relitivePath = mNewRelitivePath;
						const mNewDirectoryArray = JSON.parse(JSON.stringify(this.directoriesChanged$()));
						this._GWCommon.hierarchyReplaceItem(mNewDirectoryArray, mOriginalDirectory.key, 'key', 'children', mNewDirectory);
						this.directoriesChanged$.update(() => mNewDirectoryArray);
						this.setSelectedDirectory(mNewDirectory.relitivePath);
						resolve(true);
					},
					error: (error) => {
						this._LoggingSvc.errorHandler(error, 'FileManagerService', 'renameDirectory');
						reject(false);
					},
					complete: () => { }
				});
			}			
		});
	}

	/**
	 * Sets the selected directory based on the relative path.
	 *
	 * @param {string} relitivePath - the relative path to the directory
	 * @return {void} 
	 */
	public setSelectedDirectory(relitivePath: string): void {
		const mDirectory = this._GWCommon.hierarchySearch(this.directoriesChanged$(), relitivePath, 'relitivePath') as IDirectoryTree;
		// console.log('setSelectedDirectory.mDirectory', mDirectory);
		if(mDirectory) {
			this.selectedDirectoryChanged$.update(() => mDirectory);
		} else {
			this.selectedDirectoryChanged$.update(() => new DirectoryTree());
		}
	}

	/*********************************************  File Functions **********************************************/

	/**
	 * A method to delete a file.
	 *
	 * @param {string} action - the action to be performed
	 * @param {string} fileName - the name of the file to be deleted
	 * @return {Promise<boolean>} a promise that resolves with a boolean indicating success
	 */
	public async deleteFile(action: string, fileName: string): Promise<boolean> {
		return new Promise((resolve, reject) => {
			if (this._GWCommon.isNullOrEmpty(action)) {
				throw new Error('action can not be blank!');
			}
			if (this._GWCommon.isNullOrEmpty(fileName)) {
				throw new Error('fileName can not be blank!');
			}
			let mQueryParameter: HttpParams = new HttpParams().append('action', action);
			mQueryParameter = mQueryParameter.append('selectedPath', this._SelectedPath);
			mQueryParameter = mQueryParameter.append('fileName', fileName);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.delete<boolean>(this._Api_DeleteFile, mHttpOptions).subscribe({
				next: (response: boolean) => {
					this.getFiles(action, this._SelectedPath);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FileManagerService', 'deleteFile');
					reject(false);
				},
				complete: () => { }
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
		if (this._GWCommon.isNullOrEmpty(action)) {
			throw new Error('action can not be blank!');
		}
		if (this._GWCommon.isNullOrEmpty(oldName)) {
			throw new Error('oldName can not be blank!');
		}
		if (this._GWCommon.isNullOrEmpty(newName)) {
			throw new Error('newName can not be blank!');
		}
		return new Promise<boolean>((resolve, reject) => {
			const mQueryParameter: HttpParams = new HttpParams().append('action', action)
				.append('selectedPath', this._SelectedPath)
				.append('oldName', oldName)
				.append('newName', newName);
			const mHttpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
				}),
				params: mQueryParameter,
			};
			this._HttpClient.post<boolean>(this._Api_RenameFile, null, mHttpOptions).subscribe({
				next: (response: boolean) => {
					this.getFiles(action, this._SelectedPath);
					resolve(response);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FileManagerService', 'renameFile');
					reject(false);
				},
				complete: () => { }
			});
		});
	}

	/**
	 * @description Retrieves a file from the server.
	 *
	 * @param {string} action - The action to determine the upload directory and enforce security on the server.
	 * @param {string} selectedPath - The selected path.
	 * @param {string} fileName - The name of the file.
	 */
	public getFile(action: string, selectedPath: string, fileName: string) {
		let mSelectedPath = selectedPath;
		if (this._GWCommon.isNullOrEmpty(selectedPath)) {
			mSelectedPath = '//';
		}
		const mQueryParameter: HttpParams = new HttpParams().append('action', action)
			.append('selectedPath', mSelectedPath)
			.append('fileName', fileName);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			responseType: 'blob' as 'json',
			params: mQueryParameter,
		};
		this._HttpClient.get<Blob>(this._Api_GetFile, mHttpOptions).subscribe({
			next: (response: Blob) => {
				const mBlob: Blob = new Blob([response]);
				const mLink = document.createElement('a');
				mLink.href = window.URL.createObjectURL(mBlob);
				mLink.download = fileName;
				mLink.click();
				window.URL.revokeObjectURL(mLink.href);
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getFile');
				this._LoggingSvc.toast('Error downloading file "' + fileName + '"!', 'File Manager', LogLevel.Error);
			},
			// complete: () => {}
		});
	}

	/**
	 * @description Retrives a array of IFileInfoLight for the given path, and updates this.filesChanged$.
	 *
	 * @param {string} action Used to determine the upload directory and enforce security on the server
	 * @param {string} selectedPath The relative of the directory path
	 * @return {void} 
	 */
	public getFiles(action: string, selectedPath: string): void {
		const mQueryParameter: HttpParams = new HttpParams()
			.append('action', action)
			.append('selectedPath', selectedPath);
		const mHttpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			params: mQueryParameter,
		};
		this._SelectedPath = selectedPath;
		this._HttpClient.get<IFileInfoLight[]>(this._Api_GetFiles, mHttpOptions).subscribe({
			next: (response) => {
				this.filesChanged$.update(() => response);
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getFiles');
			},
			// complete: () => {}
		});		
	}

	/*********************************************  Multi-Part File Upload Functions **********************************************/
	
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
		const mParams = { ...parameters }; // it's good practice to leave parameter values unchanged
		const mNextUploadNumber = mParams.uploadNumber + 1;
		// const mFileSize: number = mParams.file.size;
		const mMultiUploadFileName = mParams.file.name + '_UploadNumber_' + mNextUploadNumber;
		if (mParams.uploadNumber < mParams.totalNumberOfUploads) { // do we need to send any more pices of the file
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
					mParams.endingByte = parameters.endingByte + parameters.chunkSize;
					this.uploadStatusChanged$.update(() => mUploadStatus);
					this.multiPartFileUpload(mParams);
				},
				error: (error) => {
					if (parameters.retryNumber < 4) {
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
		}
		if (mParams.uploadNumber == mParams.totalNumberOfUploads) { // make sure this is the last upload
			this.multiUploadComplete(mParams.action, mParams.file.name, this._Api_UploadFile).subscribe({
				next: (response: IUploadResponse) => {
					const mUploadStatus: IUploadStatus = new UploadStatus(mParams.action, response.fileName, response.data, true, response.isSuccess, mParams.totalNumberOfUploads, mParams.uploadNumber);
					this.uploadStatusChanged$.update(() => mUploadStatus);
					this.getFiles(mParams.action, this._SelectedPath);
				},
				error: (error) => {
					this._LoggingSvc.errorHandler(error, 'FileManagementService', 'upload');
					const mUploadStatus: IUploadStatus = new UploadStatus(mParams.action, mParams.file.name, error, true, false, mParams.totalNumberOfUploads, mParams.uploadNumber);
					this.uploadStatusChanged$.update(() => mUploadStatus);
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
		const mFormData = new FormData();
		mFormData.append('action', action);
		mFormData.append('completed', 'true');
		mFormData.append('fileName', fileName);
		mFormData.append('selectedPath', this._SelectedPath);
		return this._HttpClient.post<IUploadResponse>(uri, mFormData);
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
				this.uploadStatusChanged$.update(() => mUploadStatus);
				if (mUploadStatus.completed) {
					this.getFiles(action, this._SelectedPath);
				}
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'FileManagerService', 'singleFileUpload');
				// const mUploadStatus: IUploadStatus = new UploadStatus(action, file.name, error, true, false, 1, 1);
			},
			// complete: () => {}
		});
	}

	/**
	 * @description Uploads file by calling either multiPartFileUpload or singleFileUpload depending on the file size.
	 *
	 * @param {string} action Used to determine the upload directory and enforce security on the server
	 * @param {File} file An HTML "File" object
	 * @param {number} [chunkSize=3072000] Used to break the "File" up if the file size is greater than the chunkSize.  The number is in direct relation to KestrelServerLimits.MaxRequestBodySize Property
	 * @memberof FileManagerService
	 */
	uploadFile(action: string, file: File, chunkSize: number = 29696000) {
		// TODO: When you upload an .ISO file it seems to get "crupted" and can not be mounted.  Need to investigate this.
		const mTotalNumberOfUploads: number = this.getTotalNumberOfUploads(file.size, chunkSize);
		if (mTotalNumberOfUploads > 1) {
			const mMultiPartFileUpload: IMultiPartFileUploadParameters = new MultiPartFileUploadParameters(
				action,
				file,
				mTotalNumberOfUploads,
				chunkSize
			);
			this.multiPartFileUpload(mMultiPartFileUpload);
		} else {
			this.singleFileUpload(file, action);
		}
	}
}
