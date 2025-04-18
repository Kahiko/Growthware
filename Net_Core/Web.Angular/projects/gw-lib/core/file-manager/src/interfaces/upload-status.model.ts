export interface IUploadStatus {
    id: string;
    fileId: string;
    fileName: string;
    message: string;
    completed: boolean;
    success: boolean;
    totalNumberOfUploads: number;
    uploadNumber: number;
  }
  
export class UploadStatus implements IUploadStatus {
	constructor(
      public id: string,
      public fileId: string,
      public fileName: string,
      public message: string,
      public completed: boolean,
      public success: boolean,
      public totalNumberOfUploads: number,
      public uploadNumber: number,
	) {}
}
  