import { IFileUploadParameters } from "./file-upload-parameters.model";
export interface IMultiPartFileUploadParameters extends IFileUploadParameters {
    action: string;
    totalNumberOfUploads: number;
    uploadNumber: number;
    retryNumber: number;
    startingByte: number;
    endingByte: number;
}
export declare class MultiPartFileUploadParameters implements IMultiPartFileUploadParameters {
    action: string;
    file: File;
    totalNumberOfUploads: number;
    chunkSize: number;
    endingByte: number;
    retryNumber: number;
    startingByte: number;
    uploadNumber: number;
    constructor(action: string, file: File, totalNumberOfUploads: number, chunkSize: number);
}
