export interface IUploadStatus {
    id: string;
    fileName: string;
    message: string;
    completed: boolean;
    success: boolean;
    totalNumberOfUploads: number;
    uploadNumber: number;
}
export declare class UploadStatus implements IUploadStatus {
    id: string;
    fileName: string;
    message: string;
    completed: boolean;
    success: boolean;
    totalNumberOfUploads: number;
    uploadNumber: number;
    constructor(id: string, fileName: string, message: string, completed: boolean, success: boolean, totalNumberOfUploads: number, uploadNumber: number);
}
