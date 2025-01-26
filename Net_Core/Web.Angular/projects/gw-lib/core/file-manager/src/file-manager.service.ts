import { effect, Injectable, OnInit, signal } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { lastValueFrom, Observable } from 'rxjs';
// Library
import { ConfigurationService } from '@growthware/core/configuration';
import { DirectoryTree, IDirectoryTree } from '@growthware/common/interfaces';
import { GWCommon } from '@growthware/common/services';
import { LoggingService, LogLevel } from '@growthware/core/logging';
// Feature
import { IFileInfoLight } from './interfaces/file-info-light.model';
import { IFormDataParameters, FormDataParameters } from './interfaces/form-data-parameters.model';
import { IUploadResponse } from './interfaces/upload-response.model';
import { IUploadStatus, UploadStatus } from './interfaces/upload-status.model';

@Injectable({
	providedIn: 'root'
})
export class FileManagerService implements OnInit {
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
	private _ChunkSize: number = 29696000; // The default value is 30MB in Kestrel so this is a bit smaller
	private _Concurrency = 3; // Number of parallel uploads
	private _MaxRetries = 3;
	private _StartTime = new Date().getTime();

	MODAL_ID_RENAME_DIRECTORY: string = 'DirectoryTreeComponent.onMenuRenameClick';
	MODAL_ID_CREATE_DIRECTORY: string = 'CreateDirectoryForm';

	public get selectedPath(): string {
		return this._SelectedPath;
	}

	public readonly filesChanged$ = signal<Array<IFileInfoLight>>([] as Array<IFileInfoLight>);

	public directoriesChanged$ = signal<Array<IDirectoryTree>>([] as Array<IDirectoryTree>);

	public needToExpand: boolean = false;

	public selectedDirectoryChanged$ = signal<IDirectoryTree>({} as IDirectoryTree);

	public readonly uploadStatusChanged$ = signal<IUploadStatus>({ id: '' } as unknown as IUploadStatus);

	constructor(
		private _ConfigSvc: ConfigurationService,
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

	ngOnInit(): void {
		effect(() => {
			this._ChunkSize = this._ConfigSvc.chunkSize();
		});
	}

    /**
     * @description Generates a "unique" ID for a given file based on its name, size, and modified date.
     * @param {File} file - The file to generate the ID for
     * @returns {string} - The generated ID
     * @memberof FileManagerService
     */
    private _generateFileId(file: File): string {
        return `${file.name}-${file.size}-${file.lastModified}`; // Simple unique identifier
    }

	/**
	 * @description Uploads a file or part of a file (chunk/slice) to the server.
	 * @param {IFormDataParameters} formDataParameters - The parameters to add to the FormData.
	 * @returns {Promise<IUploadResponse>} - The response from the API.
	 * @memberof FileManagerService
	 */
	private async _uploadToServer(formDataParameters: IFormDataParameters): Promise<IUploadResponse> {
		const mFormData: FormData = this._uploadFormData(formDataParameters);
		return lastValueFrom(this._HttpClient.post<IUploadResponse>(this._Api_UploadFile, mFormData));
	}

	/**
	 * @description Attempts to upload a chunk of a file with a retry mechanism in case of failure.
	 *
	 * @private
	 * @param {string} action - Used to determine the upload directory and enforce security on the server.
	 * @param {boolean} doMerge - If true, the chunk is merged into an existing file. Otherwise, a new file is created.
	 * @param {string} fileId - A "unique" ID for a given file based on its name, size, and modified date.  See _generateFileId
	 * @param {Blob} chunk - The raw content of the file.
	 * @param {string} fileName - The name of the file being uploaded.
	 * @param {number} chunkIndex - The index of the chunk.
	 * @param {number} totalChunks - The total number of chunks.
	 * @param {number} maxRetries - The maximum number of retry attempts for a failed upload.
	 * @throws {Error} Throws an error if the maximum number of retries is reached.
	 * @memberof FileManagerService
	 */
    private async _uploadChunkWithRetry(action: string, doMerge: boolean, fileId: string, chunk: Blob, fileName: string, chunkIndex: number, totalChunks: number, maxRetries: number) {
        let attempt = 0;
        while (attempt < maxRetries) {
            try {
				const mFormDataParameters: IFormDataParameters = new FormDataParameters(action, doMerge, chunk, fileId, fileName, chunkIndex, totalChunks);
				await this._uploadToServer(mFormDataParameters).then((response: IUploadResponse) => {
					if (!response.isSuccess) {
						// This is an error that can not be recovered because the error is more than likely
						// related to code in this service. Missing data, etc.
						attempt = maxRetries + 1; // Force exit the loop
						this._LoggingSvc.toast(response.errorMessage, 'File Manager', LogLevel.Error);
						throw new Error(response.errorMessage);
					}
				}).catch((error) => {
					throw new Error(error);
				});
				return; // Success, exit function
            } catch (error: any) {
                attempt++;
				if (!this._GWCommon.isNullOrUndefined(error) && error.message !== null) {
					console.error(`FileManagerService._uploadChunkWithRetry: Chunk ${chunkIndex} failed (Attempt ${attempt}/${maxRetries})`);
				}
                if (attempt >= maxRetries) {
					throw new Error(`FileManagerService._uploadChunkWithRetry: Upload failed after ${maxRetries} attempts for chunk ${chunkIndex}`);
				}
                this._GWCommon.sleep(1000 * attempt); // Exponential backoff
            }
        }
    }

	/**
	 * Creates a FormData object for uploading a file or part of a file (chunk/slice).
	 *
	 * @param {IFormDataParameters} parameters - The parameters to add to the FormData.
	 * @returns {FormData} A FormData object containing all the given parameters.
	 * @memberof FileManagerService
	 */
	private _uploadFormData(parameters: IFormDataParameters): FormData {
		const mRetVal = new FormData();
        mRetVal.append('action', parameters.action);
		mRetVal.append('doMerge', parameters.doMerge.toString());
        mRetVal.append('fileId', parameters.fileId);
        mRetVal.append('fileName', parameters.fileName);
		if (parameters.formFile !== null && parameters.formFile !== undefined) {
			mRetVal.append('formFile', parameters.formFile);
		}
        mRetVal.append('uploadIndex', parameters.uploadIndex.toString());
        mRetVal.append('selectedPath', this._SelectedPath);
        mRetVal.append('totalUploads', parameters.totalUploads.toString());
		return mRetVal;
	}

	/**
	 * @description Uploads files that are too large to be sent in a single upload by using recursion
	 * to call the API with a "slice" of the file.
	 *
	 * @private
	 * @param {IFormDataParameters} parameters
	 * @memberof FileManagerService
	 */
    private async _uploadLargeFile(action: string, doMerge: boolean, file: File, onProgress: (uploadStatus: IUploadStatus) => void, onComplete: (uploadStatus: IUploadStatus) => void) {
        const fileId = this._generateFileId(file);
        const totalChunks = Math.ceil(file.size / this._ChunkSize);
        let uploadedChunks = 0;
        let chunkUploadPromises: Promise<void>[] = [];

        for (let chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++) {
            let start = chunkIndex * this._ChunkSize;
            let end = Math.min(start + this._ChunkSize, file.size);
            let chunk = file.slice(start, end);

            chunkUploadPromises.push(
                this._uploadChunkWithRetry(action, doMerge, fileId, chunk, file.name, chunkIndex, totalChunks, this._MaxRetries).then(() => {
                    uploadedChunks++;
                    const uploadStatus: IUploadStatus = new UploadStatus(action, fileId, file.name, '', false, true, totalChunks, chunkIndex);
                    onProgress(uploadStatus);
                })
            );
            // Control concurrency: Process only a limited number of chunks at a time
            if (chunkUploadPromises.length >= this._Concurrency) {
                await Promise.all(chunkUploadPromises);
                chunkUploadPromises = []; // Reset after batch completes
            }
        }

        // Wait for any remaining chunks to complete
        await Promise.all(chunkUploadPromises);
        const uploadStatus: IUploadStatus = new UploadStatus(action, fileId, file.name, '', false, true, totalChunks, totalChunks);
        onComplete(uploadStatus);
    }
	
	/**
	 * @description Makes the final call to the API to merge together all the "chunks" or slices of the file that were uploaded.
	 *
	 * @private
	 * @param {IFormDataParameters} parameters
	 * @memberof FileManagerService
	 */
	private _uploadLargeFileComplete(uploadStatus: IUploadStatus) {
		const mFormDataParameters: IFormDataParameters = new FormDataParameters(uploadStatus.id, true, null, uploadStatus.fileId, uploadStatus.fileName, uploadStatus.totalNumberOfUploads, uploadStatus.totalNumberOfUploads);
		const mFormData: FormData = this._uploadFormData(mFormDataParameters);
		this._HttpClient.post<IUploadResponse>(this._Api_UploadFile, mFormData).subscribe({
			next: (response: IUploadResponse) => {
				// update the file list
				this.getFiles(uploadStatus.id, this._SelectedPath);
				// Uncomment this code if you want to time the upload
				const mEndTime = new Date().getTime();
				const mDuration = mEndTime - this._StartTime;
				console.log('Duration: ', mDuration);

				// update the uploadStatusChanged signal so the UI can update
				this.uploadStatusChanged$.update(() => uploadStatus);
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'FileManagementService', 'upload');
				this.uploadStatusChanged$.update(() => uploadStatus);
			},
		});
	}

	private _uploadProgress(uploadStatus: IUploadStatus) {
		this.uploadStatusChanged$.update(() => uploadStatus);
	}

	/**
	 * @description Performs a single file upload
	 *
	 * @private
	 * @param {File} file the HTML "file" object
	 * @param {string} action Used to determine the upload directory and enforce security on the server
	 * @memberof FileManagerService
	 */
	private _uploadSmallFile(file: File, action: string) {
		const mFileId = this._generateFileId(file);
		const mFormDataParameters: IFormDataParameters = new FormDataParameters(action, true, file, mFileId, file.name, 0, 1);
		const mFormData: FormData = this._uploadFormData(mFormDataParameters);
		this._HttpClient.post<IUploadResponse>(this._Api_UploadFile, mFormData).subscribe({
			next: (response: IUploadResponse) => {
				const mUploadStatus: IUploadStatus = new UploadStatus(action, mFileId, response.fileName, response.errorMessage, true, response.isSuccess, 1, 1);
				this.uploadStatusChanged$.update(() => mUploadStatus);
				if (mUploadStatus.completed) {
					this.getFiles(action, this._SelectedPath);
				}
			},
			error: (error) => {
				this._LoggingSvc.errorHandler(error, 'FileManagerService', 'singleFileUpload');
			},
			// complete: () => {}
		});
	}
	
	/**
	 * @description Creates a directory in the current directory.
	 *
	 * @param {string} action - The action to be performed
	 * @param {string} newPath - The name of the new directory
	 * @returns {Promise<boolean>} A promise that resolves to a boolean indicating the success of the operation.
	 * @memberof FileManagerService
	 */
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

	
	/**
	 * Refreshes the current directory and file list by retrieving the latest 
	 * directories and files from the server based on the provided action.
	 *
	 * @param {string} action - The action used to determine the upload directory 
	 * and enforce security on the server.
	 * @return {void} - This function does not return a value.
	 */
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
	
	/**
	 * @description Calculates the total number of chuncks needed to upload a large file
	 *
	 * @param {number} fileSize  The size of the file
	 * @return {*}  {number}
	 * @memberof FileManagerService
	 */
	public getTotalNumberOfUploads(fileSize: number): number {
		const mRetVal = Math.ceil(fileSize / this._ChunkSize);
		return mRetVal;
	}

	/**
	 * @description Uploads file by calling either _uploadLargeFile or _uploadSmallFile depending on the file size.
	 *
	 * @param {string} action Used to determine the upload directory and enforce security on the server
	 * @param {File} file An HTML "File" object
	 * @param {number} [chunkSize=3072000] Used to break the "File" up if the file size is greater than the chunkSize.  The number is in direct relation to KestrelServerLimits.MaxRequestBodySize Property
	 * @memberof FileManagerService
	 */
	uploadFile(action: string, file: File) {
		this._StartTime = new Date().getTime();
		const mTotalNumberOfUploads: number = this.getTotalNumberOfUploads(file.size);
		if (mTotalNumberOfUploads > 1) {
			
			this._uploadLargeFile(action, false, file, (uploadStatus) => {
				this.uploadProgress(uploadStatus);
			}, (uploadStatus) => {
				// It is good practice to leave parameters unchanged
				const mUploadStatus: IUploadStatus = {...uploadStatus};
				mUploadStatus.completed = true;
				mUploadStatus.uploadNumber = uploadStatus.totalNumberOfUploads;
				this._uploadLargeFileComplete(mUploadStatus);
			});
		} else {
			this._uploadSmallFile(file, action);
		}
	}

	private uploadProgress(uploadStatus: IUploadStatus) {
		this.uploadStatusChanged$.update(() => uploadStatus);
	}
}
